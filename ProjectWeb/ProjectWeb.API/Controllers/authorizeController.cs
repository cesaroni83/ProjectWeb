using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectWeb.API.Data;
using ProjectWeb.API.GoogleService;
using ProjectWeb.API.Helper;
using ProjectWeb.Shared.Account;
using ProjectWeb.Shared.Enums;
using ProjectWeb.Shared.Google;
using ProjectWeb.Shared.Modelo.Entidades;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ProjectWeb.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class authorizeController : ControllerBase
    {
        private readonly IGoogleAuthorization googleAuthorization;
        private readonly AppDbContext context;
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        public authorizeController(
            IGoogleAuthorization googleAuthorization,
            AppDbContext context,
            IUserHelper userHelper,
            IConfiguration configuration)
        {
            this.googleAuthorization = googleAuthorization;
            this.context = context;
            _userHelper = userHelper;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Authorize() => Ok(googleAuthorization.GetAuthorizationurl());

        [HttpGet("callback")]
        public async Task<IActionResult> callback(string code)
        {
            // 1️⃣ Intercambiar code por token de Google
            var userCredential = await googleAuthorization.ExchangeCodeforToken(code);

            // 2️⃣ Obtener info del usuario de Google
            var googleUser = await googleAuthorization.GetUserInfoAsync(userCredential.Token.AccessToken);

            // 3️⃣ Buscar usuario en AspNetUsers
            var user = await _userHelper.GetUserAsync(googleUser.Email);
            
            if (user == null)
            {
                // 4️⃣ Crear usuario nuevo
                var names = googleUser.Name.Split(' ', 2);
                user = new User
                {
                    Email = googleUser.Email,
                    UserName = googleUser.Email,
                    FirstName = names.Length > 0 ? names[0] : googleUser.Name,
                    LastName = names.Length > 1 ? names[1] : "",
                    Address = "Null",
                    UserType = UserType.User,
                    Id_ciudad = 1
                };

                // Guardar usuario con password aleatorio
                var pass = Guid.NewGuid().ToString();
                var result = await _userHelper.AddUserAsync(user, pass);
                if (!result.Succeeded)
                    return BadRequest(result.Errors.FirstOrDefault());
                
                // Asignar rol
                await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());
            }
            else
            {
                // 5️⃣ Actualizar info principal si cambió
                var names = googleUser.Name.Split(' ', 2);
                user.FirstName = names.Length > 0 ? names[0] : user.FirstName;
                user.LastName = names.Length > 1 ? names[1] : user.LastName;
                user.Photo = googleUser.Picture ?? user.Photo;
                await _userHelper.UpdateUserAsync(user);
                
            }
            // crea persona
            var confirma = await _userHelper.AddOrUpdateUserWithPersonaAsync(user);
            if (!confirma.Succeeded)
                return BadRequest(confirma.Errors.FirstOrDefault());

            // 🔹 Llamar al método que elimina credenciales previas
            await RemoveCredentialByToken(userCredential.Token.AccessToken);

            // 6️⃣ Generar JWT propio
            var tokenDto = BuildToken(user);

            // 7️⃣ Redirigir al frontend con JWT
            return Redirect($"https://localhost:7188/auth/callback?token={tokenDto.Token}");
        }

        [HttpGet("token/{userId}")]
        public async Task<IActionResult> GetAccessToken(string userId)
        {
            if (!Guid.TryParse(userId, out var _userId))
                return Unauthorized();

            var credential = await context.Credentials.FirstOrDefaultAsync(c => c.UserId == _userId);
            return Ok(JsonSerializer.Serialize(new Token(credential!.AccessToken, credential.UserId.ToString())));
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
        private async Task RemoveCredentialByToken(string accessToken)
        {
            var credential = await context.Credentials.FirstOrDefaultAsync(c => c.AccessToken == accessToken);
            if (credential != null)
            {
                context.Credentials.Remove(credential);
                await context.SaveChangesAsync();
            }
        }
    }
}
