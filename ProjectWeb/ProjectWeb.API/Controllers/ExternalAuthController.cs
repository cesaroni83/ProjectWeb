using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
        public ExternalAuthController(IConfiguration configuration)
        {
            _configuration = configuration;
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
            /////*   crear el usuario si no exite**//
            //var user = await _userService.GetByEmailAsync(email);
            //if (user == null)
            //{
            //    // Crear usuario si no existe
            //    user = await _userService.CreateUserAsync(new User
            //    {
            //        Name = name,
            //        Email = email,
            //        PhotoUrl = photo,
            //        LoginProvider = "Facebook",
            //        CreatedAt = DateTime.UtcNow
            //    });
            //}


            //////////////////////////////////
            // ✅ Usa tu configuración actual sin romper nada
            var jwtKey = _configuration["jwtKey"]; // <-- usa el campo que ya tienes
            if (string.IsNullOrEmpty(jwtKey))
                return BadRequest("Falta la configuración jwtKey en appsettings.json");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, name),
            new Claim(ClaimTypes.Email, email),
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
            var redirect = $"{Url}?token={jwt}";
            return Redirect(redirect);
        }

        
    }
}
