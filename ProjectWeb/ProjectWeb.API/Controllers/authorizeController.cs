
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectWeb.API.Data;
using ProjectWeb.API.GoogleService;
using ProjectWeb.API.Helper;
using ProjectWeb.API.Helper.Implementacion;
using ProjectWeb.Shared.Account;
using ProjectWeb.Shared.Enums;
using ProjectWeb.Shared.Google;
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
        private readonly IGoogleAuthorization _googleAuthorization;
        private readonly AppDbContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly IFileStorage _fileStorage;

        public authorizeController(
            IGoogleAuthorization googleAuthorization,
            AppDbContext context,
            IUserHelper userHelper,
            IConfiguration configuration,
            IFileStorage fileStorage)
        {
            _googleAuthorization = googleAuthorization;
            _context = context;
            _userHelper = userHelper;
            _configuration = configuration;
            _fileStorage = fileStorage;
        }

        [HttpGet]
        public IActionResult Authorize() => Ok(_googleAuthorization.GetAuthorizationurl());

       

        [HttpGet("token/{userId}")]
        public async Task<IActionResult> GetAccessToken(string userId)
        {
            if (!Guid.TryParse(userId, out var _userId))
                return Unauthorized();

            var credential = await _context.Credentials.FirstOrDefaultAsync(c => c.UserId == _userId);
            return Ok(JsonSerializer.Serialize(new Token(credential!.AccessToken, credential.UserId.ToString())));
        }

        // 🔹 Generar JWT interno
        private TokenDTO BuildToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email!),
                new Claim(ClaimTypes.Role, user.UserType.ToString()),
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
            var credential = await _context.Credentials.FirstOrDefaultAsync(c => c.AccessToken == accessToken);
            if (credential != null)
            {
                _context.Credentials.Remove(credential);
                await _context.SaveChangesAsync();
            }
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string code)
        {
            // 1️⃣ Intercambiar el code por token de Google
            var userCredential = await _googleAuthorization.ExchangeCodeforToken(code);

            // 2️⃣ Obtener datos del usuario de Google
            var googleUser = await _googleAuthorization.GetUserInfoAsync(userCredential.Token.AccessToken);
            if (googleUser == null || string.IsNullOrEmpty(googleUser.Email))
                return BadRequest("No se pudo obtener la información del usuario desde Google.");

            // 3️⃣ Intentar login externo directamente
            var signInResult = await _userHelper.ExternalLoginSignInAsync("Google", googleUser.Id);
            User user;

            if (signInResult.Succeeded)
            {
                // 🔹 Usuario ya vinculado con Google → obtenerlo
                user = await _userHelper.GetUserAsync(googleUser.Email);
            }
            else
            {
                // 🔹 Usuario nuevo o sin login vinculado
                user = await _userHelper.GetUserAsync(googleUser.Email);

                if (user == null)
                {
                    // Crear usuario nuevo
                    var names = googleUser.Name.Split(' ', 2);
                    user = new User
                    {
                        Email = googleUser.Email,
                        UserName = googleUser.Email,
                        FirstName = names.Length > 0 ? names[0] : googleUser.Name,
                        LastName = names.Length > 1 ? names[1] : "",
                        Address = "Null",
                        UserType = UserType.User,
                        Id_ciudad = 1,
                        EmailConfirmed = true
                    };

                    // Descargar foto desde Google si existe
                    if (!string.IsNullOrEmpty(googleUser.Picture))
                    {
                        try
                        {
                            user.Photo = await _fileStorage.SaveImageFromUrlAsync(googleUser.Picture);
                        }
                        catch
                        {
                            user.Photo = googleUser.Picture; // fallback si falla descarga
                        }
                    }

                    var createResult = await _userHelper.AddUserAsync(user, Guid.NewGuid().ToString());
                    if (!createResult.Succeeded)
                        return BadRequest(createResult.Errors.FirstOrDefault()?.Description);

                    await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());
                }

                // Asociar login externo (solo si no existe)
                var existingLogin = await _context.UserLogins
                    .FirstOrDefaultAsync(l => l.LoginProvider == "Google" && l.ProviderKey == googleUser.Id);

                if (existingLogin == null)
                {
                    var loginInfo = new UserLoginInfo("Google", googleUser.Id, "Google");
                    var addLoginResult = await _userHelper.AddLoginAsync(user, loginInfo);
                    if (!addLoginResult.Succeeded)
                        return BadRequest(addLoginResult.Errors.FirstOrDefault()?.Description);
                }
            }

            // 4️⃣ Actualizar datos básicos si cambiaron
            var updateNeeded = false;
            var nameParts = googleUser.Name.Split(' ', 2);

            if (user.FirstName != nameParts[0]) { user.FirstName = nameParts[0]; updateNeeded = true; }
            if (user.LastName != (nameParts.Length > 1 ? nameParts[1] : "")) { user.LastName = nameParts.Length > 1 ? nameParts[1] : ""; updateNeeded = true; }

            if (!string.IsNullOrEmpty(googleUser.Picture))
            {
                string? newPhoto = null;
                try
                {
                    newPhoto = await _fileStorage.SaveImageFromUrlAsync(googleUser.Picture);

                    // Borrar foto anterior si existe y es diferente
                    if (!string.IsNullOrEmpty(user.Photo) && user.Photo != newPhoto)
                        await _fileStorage.DeleteImageAsync(user.Photo);

                    if (newPhoto != user.Photo)
                    {
                        user.Photo = newPhoto;
                        updateNeeded = true;
                    }
                }
                catch
                {
                    // fallback a URL original si falla descarga
                    if (user.Photo != googleUser.Picture)
                    {
                        user.Photo = googleUser.Picture;
                        updateNeeded = true;
                    }
                }
            }

            if (updateNeeded)
                await _userHelper.UpdateUserAsync(user);

            // 5️⃣ Crear o actualizar persona asociada
            var confirma = await _userHelper.AddOrUpdateUserWithPersonaAsync(user);
            if (!confirma.Succeeded)
                return BadRequest(confirma.Errors.FirstOrDefault()?.Description);

            // 6️⃣ Eliminar credenciales previas en tu tabla de integración
            await RemoveCredentialByToken(userCredential.Token.AccessToken);

            // 7️⃣ Generar JWT
            var tokenDto = BuildToken(user);

            // 8️⃣ Redirigir al frontend con token
            return Redirect($"https://localhost:7188/auth/callback?token={tokenDto.Token}");
        }

    }
}
