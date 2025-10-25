using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectWeb.API.Helper;
using ProjectWeb.Shared.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ProjectWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalAuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserHelper _userHelper;
        public ExternalAuthController(IConfiguration configuration, IUserHelper userHelper)
        {
            _configuration = configuration;
            _userHelper = userHelper;
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
            var result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return BadRequest("Error al autenticar con Facebook.");

            var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value ?? "";
            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value ?? "";
            var pictureClaim = result.Principal.FindFirst("urn:facebook:picture")?.Value
                ?? result.Principal.FindFirst("picture")?.Value;

            string? photoUrl = pictureClaim; // ya es la URL directa
            //var photo = result.Principal.FindFirst("picture")?.Value;
            /////*   crear el usuario si no exite**//
            // 3️⃣ Buscar usuario en AspNetUsers
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                // 4️⃣ Crear usuario nuevo
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
                    Photo = photoUrl

                };

                // Guardar usuario con password aleatorio
                var resultado = await _userHelper.AddUserAsync(user, Guid.NewGuid().ToString());
                if (!resultado.Succeeded)
                    return BadRequest(resultado.Errors.FirstOrDefault());

                // Asignar rol
                await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());
            }
            else
            {
                var names = name.Split(' ', 2);
                user.FirstName = names.Length > 0 ? names[0] : user.FirstName;
                user.LastName = names.Length > 1 ? names[1] : user.LastName;
                user.Photo = photoUrl ?? user.Photo;
                await _userHelper.UpdateUserAsync(user);
                
            }


            //////////////////////////////////
            // ✅ Usa tu configuración actual sin romper nada
            var jwtKey = _configuration["jwtKey"]; // <-- usa el campo que ya tienes
            if (string.IsNullOrEmpty(jwtKey))
                return BadRequest("Falta la configuración jwtKey en appsettings.json");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, email),
                 new Claim("Photo", photoUrl ?? ""),
                new Claim("LoginProvider", "Facebook")
        };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            string? Url = "https://localhost:7188/login-facebook";
            // 🔹 Redirige al frontend con el token
            //var redirect = $"{Url}?token={jwt}";
            var redirect = $"{Url}?token={Uri.EscapeDataString(jwt)}";
            return Redirect(redirect);
        }

        
    }
}
