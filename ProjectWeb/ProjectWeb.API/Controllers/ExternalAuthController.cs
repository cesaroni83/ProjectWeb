using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectWeb.API.Data;
using ProjectWeb.API.Helper;
using ProjectWeb.API.Helper.Implementacion;
using ProjectWeb.Shared.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalAuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserHelper _userHelper;
        private readonly AppDbContext _context;
        private readonly IFileStorage _fileStorage;
        public ExternalAuthController(IConfiguration configuration, IUserHelper userHelper, AppDbContext context, IFileStorage fileStorage)
        {
            _configuration = configuration;
            _userHelper = userHelper;
            _context = context;
            _fileStorage = fileStorage;
        }
        [HttpGet("facebook-login")]
        public IActionResult FacebookLogin(string? returnUrl = "/")
        {
            var redirectUrl = Url.Action("FacebookResponse", "ExternalAuth", new { returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }
        [HttpGet("facebook-response")]
        public async Task<IActionResult> FacebookResponse(string? returnUrl = "https://localhost:7188/")
        {
            // 1️⃣ Autenticar con Facebook
            var result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return BadRequest("Error al autenticar con Facebook.");

            var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value ?? "";
            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value ?? "";
            var providerKey = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var pictureClaim = result.Principal.FindFirst("urn:facebook:picture")?.Value
                               ?? result.Principal.FindFirst("picture")?.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(providerKey))
                return BadRequest("No se pudo obtener el correo o el Id de Facebook.");

            // 2️⃣ Buscar usuario por email
            var user = await _userHelper.GetUserAsync(email);

            if (user == null)
            {
                // Crear usuario nuevo
                var names = name.Split(' ', 2);
                user = new User
                {
                    Email = email,
                    UserName = email,
                    FirstName = names.Length > 0 ? names[0] : name,
                    LastName = names.Length > 1 ? names[1] : "",
                    Address = "Null",
                    UserType = UserType.User,
                    Id_ciudad = 1,
                    EmailConfirmed = true
                };

                // Descargar foto de Facebook si existe
                if (!string.IsNullOrEmpty(pictureClaim))
                {
                    try
                    {
                        user.Photo = await _fileStorage.SaveImageFromUrlAsync(pictureClaim);
                    }
                    catch
                    {
                        user.Photo = pictureClaim; // fallback si falla descarga
                    }
                }

                // Guardar usuario con password aleatorio
                var createResult = await _userHelper.AddUserAsync(user, Guid.NewGuid().ToString());
                if (!createResult.Succeeded)
                    return BadRequest(createResult.Errors.FirstOrDefault());

                // Recargar usuario para asegurarnos de que tenga Id
                user = await _userHelper.GetUserAsync(email);

                // Asignar rol
                await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());
            }
            else
            {
                // Usuario existente → actualizar datos si cambiaron
                var updateNeeded = false;
                var nameParts = name.Split(' ', 2);

                if (user.FirstName != nameParts[0]) { user.FirstName = nameParts[0]; updateNeeded = true; }
                if (user.LastName != (nameParts.Length > 1 ? nameParts[1] : "")) { user.LastName = nameParts.Length > 1 ? nameParts[1] : ""; updateNeeded = true; }

                if (!string.IsNullOrEmpty(pictureClaim))
                {
                    string? newPhoto = null;
                    try
                    {
                        newPhoto = await _fileStorage.SaveImageFromUrlAsync(pictureClaim);

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
                        // Fallback a URL original si falla
                        if (user.Photo != pictureClaim)
                        {
                            user.Photo = pictureClaim;
                            updateNeeded = true;
                        }
                    }
                }

                if (updateNeeded)
                    await _userHelper.UpdateUserAsync(user);
            }

            // 3️⃣ Asociar login externo en AspNetUserLogins
            var existingLogin = await _context.UserLogins
                .FirstOrDefaultAsync(l => l.LoginProvider == FacebookDefaults.AuthenticationScheme && l.ProviderKey == providerKey);

            if (existingLogin == null)
            {
                var loginInfo = new UserLoginInfo(FacebookDefaults.AuthenticationScheme, providerKey, "Facebook");
                var addLoginResult = await _userHelper.AddLoginAsync(user, loginInfo);
                if (!addLoginResult.Succeeded)
                    return BadRequest(addLoginResult.Errors.FirstOrDefault());
            }

            // 4️⃣ Crear o actualizar persona asociada
            var confirma = await _userHelper.AddOrUpdateUserWithPersonaAsync(user);
            if (!confirma.Succeeded)
                return BadRequest(confirma.Errors.FirstOrDefault());

            // 5️⃣ Generar JWT
            var jwtKey = _configuration["jwtKey"];
            if (string.IsNullOrEmpty(jwtKey))
                return BadRequest("Falta la configuración jwtKey en appsettings.json");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, user.Email!),
        new Claim(ClaimTypes.Role, user.UserType.ToString()),
        new Claim("FirstName", user.FirstName),
        new Claim("LastName", user.LastName),
        new Claim("Address", user.Address),
        new Claim("CityId", user.Id_ciudad.ToString()),
        new Claim("Photo", user.Photo ?? ""),
        new Claim("LoginProvider", "Facebook")
    };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            // 6️⃣ Redirigir al frontend con token
            string redirectUrl = "https://localhost:7188/login-facebook";
            return Redirect($"{redirectUrl}?token={Uri.EscapeDataString(jwt)}");
        }

    }
}
