using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectWeb.API.Data;
using ProjectWeb.API.Helper;
using ProjectWeb.API.Servicios;
using ProjectWeb.Shared.Account;
using ProjectWeb.Shared.Enums;
using ProjectWeb.Shared.Modelo.AuthenDTO;
using ProjectWeb.Shared.Modelo.DTO.Pais;
using ProjectWeb.Shared.Modelo.DTO.User;
using ProjectWeb.Shared.Modelo.Entidades;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;

namespace ProjectWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly IFileStorage _fileStorage;
        private readonly IMailHelper _mailHelper;
        private readonly AppDbContext _context;
        private readonly string _container;
        private readonly IPersonas _persona;
        private readonly IMapper _mapper;

        public AccountsController(IUserHelper userHelper, IConfiguration configuration, IFileStorage fileStorage, IMailHelper mailHelper, AppDbContext context, IPersonas persona, IMapper mapper)
        {
            _userHelper = userHelper;
            _configuration = configuration;
            _fileStorage = fileStorage;
            _mailHelper = mailHelper;
            _context = context;
            _container = "users";
            _persona = persona;
            _mapper = mapper;
        }

        [HttpPost("RecoverPassword")]
        public async Task<ActionResult> RecoverPassword([FromBody] EmailDTO model)
        {
            User user = await _userHelper.GetUserAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }

            var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
            var tokenLink = Url.Action("ResetPassword", "Accounts", new
            {
                userid = user.Id,
                token = myToken
            }, HttpContext.Request.Scheme, _configuration["UrlWEB"]);

            var response = _mailHelper.SendMail(user.FullName, user.Email!,
                $"Sales - Recuperación de contraseña",
                $"<h1>Sales - Recuperación de contraseña</h1>" +
                $"<p>Para recuperar su contraseña, por favor hacer clic 'Recuperar Contraseña':</p>" +
                $"<b><a href ={tokenLink}>Recuperar Contraseña</a></b>");

            if (response.IsSuccess)
            {
                return NoContent();
            }

            return BadRequest(response.Message);
        }

        [HttpPost("ResetPassword")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            User user = await _userHelper.GetUserAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors.FirstOrDefault()!.Description);
        }

        [HttpPost("changePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> ChangePasswordAsync(ChangePasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userHelper.GetUserAsync(User.Identity!.Name!);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault()!.Description);
            }

            return NoContent();
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(User user)
        {
            try
            {
                if (!string.IsNullOrEmpty(user.Photo))
                {

                    var photoUser = await _fileStorage.SaveImageAsync(user.Photo);
                    user.Photo = photoUser;
                }

                var currentUser = await _userHelper.GetUserAsync(user.Email!);
                if (currentUser == null)
                {
                    return NotFound();
                }
                // currentUser.Document = user.Document;
                if (!string.IsNullOrEmpty(currentUser.Photo))
                {
                    await _fileStorage.DeleteImageAsync(currentUser.Photo);
                }
                currentUser.FirstName = user.FirstName;
                currentUser.LastName = user.LastName;
                currentUser.Address = user.Address;
                currentUser.PhoneNumber = user.PhoneNumber;
                currentUser.Photo = !string.IsNullOrEmpty(user.Photo) && user.Photo != currentUser.Photo ? user.Photo : currentUser.Photo;
                currentUser.Id_ciudad = user.Id_ciudad;
                // crea o modifica persona
                var confirma = await _userHelper.AddOrUpdateUserWithPersonaAsync(currentUser);
                if (!confirma.Succeeded)
                    return BadRequest(confirma.Errors.FirstOrDefault());
                var result = await _userHelper.UpdateUserAsync(currentUser);
                if (result.Succeeded)
                {
                    return Ok(BuildToken(currentUser));
                }

                return BadRequest(result.Errors.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Get()
        {
            return Ok(await _userHelper.GetUserAsync(User.Identity!.Name!));
        }

        [AllowAnonymous]
        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateUser([FromBody] UserDTO model)
        {
            User user = model;
            if (!string.IsNullOrEmpty(model.Photo))
            {
                var photoUser = await _fileStorage.SaveImageAsync(model.Photo);
                model.Photo = photoUser;
            }

            var result = await _userHelper.AddUserAsync(user, model.Password);
            if (result.Succeeded)
            {
                ////***CREA LA PERSONA ****//
                var confirma = await _userHelper.AddOrUpdateUserWithPersonaAsync(user);
                if (!confirma.Succeeded)
                    return BadRequest(result.Errors.FirstOrDefault());
                //**********************************************************************
                await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());
                var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                var tokenLink = Url.Action("ConfirmEmail", "Accounts", new
                {
                    userid = user.Id,
                    token = myToken
                }, HttpContext.Request.Scheme, _configuration["UrlWEB"]);

                var response = _mailHelper.SendMail(user.FullName, user.Email!,
                    $"Sales - Confirmación de cuenta",
                    $"<h1>Sales - Confirmación de cuenta</h1>" +
                    $"<p>Para habilitar el usuario, por favor hacer clic 'Confirmar Email':</p>" +
                    $"<b><a href ={tokenLink}>Confirmar Email</a></b>");

                if (response.IsSuccess)
                {
                    return NoContent();
                }

                return BadRequest(response.Message);
            }

            return BadRequest(result.Errors.FirstOrDefault());
        }

        [HttpPost("ResedToken")]
        public async Task<ActionResult> ResedToken([FromBody] EmailDTO model)
        {
            User user = await _userHelper.GetUserAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }

            //TODO: Improve code 
            var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
            var tokenLink = Url.Action("ConfirmEmail", "Accounts", new
            {
                userid = user.Id,
                token = myToken
            }, HttpContext.Request.Scheme, _configuration["UrlWEB"]);

            var response = _mailHelper.SendMail(user.FullName, user.Email!,
                $"Saless- Confirmación de cuenta",
                $"<h1>Sales - Confirmación de cuenta</h1>" +
                $"<p>Para habilitar el usuario, por favor hacer clic 'Confirmar Email':</p>" +
                $"<b><a href ={tokenLink}>Confirmar Email</a></b>");

            if (response.IsSuccess)
            {
                return NoContent();
            }

            return BadRequest(response.Message);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmailAsync(string userId, string token)
        {
            token = token.Replace(" ", "+");
            var user = await _userHelper.GetUserAsync(new Guid(userId));
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault());
            }

            return NoContent();
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var result = await _userHelper.LoginAsync(model);
            if (result.Succeeded)
            {
                var user = await _userHelper.GetUserAsync(model.Email);
                return Ok(BuildToken(user));
            }

            if (result.IsLockedOut)
            {
                return BadRequest("Ha superado el máximo número de intentos, su cuenta está bloqueada, intente de nuevo en 5 minutos.");
            }

            if (result.IsNotAllowed)
            {
                return BadRequest("El usuario no ha sido habilitado, debes de seguir las instrucciones del correo enviado para poder habilitar el usuario.");
            }

            return BadRequest("Email o contraseña incorrectos.");
        }

        private TokenDTO BuildToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email!),
                new Claim(ClaimTypes.Role, user.UserType.ToString()),
                //new Claim("Document", user.Document),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("Address", user.Address),
                new Claim("Photo", user.Photo ?? string.Empty),
                new Claim("CityId", user.Id_ciudad.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(30);
            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new TokenDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        
        [HttpGet("ListUser")]
        public async Task<List<UsersDTO>> GetListaAllUsers()
        {

            var users = await _context.Users
            .Include(u => u.Ciudades)
                .ThenInclude(c => c!.Provincias)
                    .ThenInclude(s => s!.Paises)
            .ToListAsync();

            // Mapear con AutoMapper
            var userDtos = _mapper.Map<List<UsersDTO>>(users);

            return userDtos;
        }


        [HttpGet("UserView/{email}")]
        public async Task<ActionResult<UsersDTO>> GetViewUser(string email)
        {
            var user = await _context.Users
            .Include(u => u.Ciudades)
            .ThenInclude(c => c!.Provincias)
            .ThenInclude(s => s!.Paises)
            .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return NotFound();

            var userDto = _mapper.Map<UsersDTO>(user);
            return Ok(userDto);

        }

        [AllowAnonymous]
        [HttpPost("CreateUserByAdmin")]
        public async Task<ActionResult<UsersDTO>> CreateUserByAdmin([FromBody] UsersDTO model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.Photo))
                {
                    var photoUser = await _fileStorage.SaveImageAsync(model.Photo);
                    model.Photo = photoUser;
                }

                var user = _mapper.Map<User>(model);
                user.UserName = model.Email;
                user.Email = model.Email;

                var result = await _userHelper.AddUserAsync(user, model.Passwords);
                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                var persona = new Persona
                {
                    Id_user = user.Id,
                    Nombre = user.FirstName,
                    Apellido = user.LastName,
                    Id_ciudad = user.Id_ciudad,
                    Direccion_persona = user.Address,
                    Email = user.Email!,
                    Tipo_persona = user.UserType.ToString(),
                    Estado_persona = "A"
                };

                _context.Tbl_Persona.Add(persona);
                await _context.SaveChangesAsync();

                await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

                var userDto = _mapper.Map<UsersDTO>(user);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("UpdateUserByAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> PutFull(UsersDTO user)
        {
            try
            {
                if (!string.IsNullOrEmpty(user.Photo))
                {
                    var photoUser = await _fileStorage.SaveImageAsync(user.Photo);
                    user.Photo = photoUser;
                }

                var currentUser = await _userHelper.GetUserAsync(user.Email!);
                if (currentUser == null)
                {
                    return NotFound();
                }

                // Eliminar foto antigua si existe
                if (!string.IsNullOrEmpty(currentUser.Photo) && currentUser.Photo != user.Photo)
                {
                    await _fileStorage.DeleteImageAsync(currentUser.Photo);
                }

                // Actualizar datos generales
                currentUser.FirstName = user.FirstName;
                currentUser.LastName = user.LastName;
                currentUser.Address = user.Address;
                currentUser.PhoneNumber = user.PhoneNumber;
                currentUser.Photo = !string.IsNullOrEmpty(user.Photo) ? user.Photo : currentUser.Photo;
                currentUser.Id_ciudad = user.Id_ciudad;

                // 🔒 Actualizar estado de bloqueo
                if (user.LockoutEnabled)
                {
                    currentUser.LockoutEnabled = true;
                    currentUser.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100); // bloqueado indefinidamente
                }
                else
                {
                    currentUser.LockoutEnabled = false;
                    currentUser.LockoutEnd = null; // desbloqueado
                }


                // 👤 Actualizar tipo de usuario
                currentUser.UserType = user.UserType;

                // 🔑 Actualizar contraseña si viene diferente
                if (!string.IsNullOrEmpty(user.Passwords))
                {
                    var token = await _userHelper.GeneratePasswordResetTokenAsync(currentUser);
                    var passResult = await _userHelper.ResetPasswordAsync(currentUser, token, user.Passwords);
                    if (!passResult.Succeeded)
                        return BadRequest(passResult.Errors.FirstOrDefault());
                }

                // Crear o modificar persona asociada
                var confirma = await _userHelper.AddOrUpdateUserWithPersonaAsync(currentUser);
                if (!confirma.Succeeded)
                    return BadRequest(confirma.Errors.FirstOrDefault());

                var result = await _userHelper.UpdateUserAsync(currentUser);
                if (result.Succeeded)
                {
                    return Ok(BuildToken(currentUser));
                }

                return BadRequest(result.Errors.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
