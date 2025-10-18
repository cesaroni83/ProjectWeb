using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProjectWeb.API.Data;
using ProjectWeb.Shared.Google;
using System.Security.Claims;
using System.Text.Encodings.Web;


namespace ProjectWeb.API.GoogleService
{
    public class GoogleAccessTokenAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions>
        options,ILoggerFactory logger, UrlEncoder encoder, TimeProvider timeProvider,IGoogleAuthorization googleAuthorization,AppDbContext context):
        AuthenticationHandler<AuthenticationSchemeOptions>(options,logger,encoder)
    {
        private readonly TimeProvider timeProvider=timeProvider;

        protected override async Task <AuthenticateResult>HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authoiation Header");
            string authHeader = Request.Headers.Authorization!;
            if (!authHeader.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.Fail("Invalid Authoiation Header");
            string accessToken =authHeader["Beare ".Length..].ToString();
            var userCredential = await googleAuthorization.ValidateToken(accessToken);
            Credential? user = await GetUserCredential(userCredential.Token.AccessToken);
            if (user == null)
                AuthenticateResult.Fail("Invalid Access Token Provided");
            List<Claim> claims = [new(ClaimTypes.NameIdentifier, user!.UserId.ToString())];
            var identity =new ClaimsIdentity(claims,Constant.Scheme);
            return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Constant.Scheme));
        }

        private async Task<Credential?> GetUserCredential(string accessToken) =>
            await context.Credentials.FirstOrDefaultAsync(c => c.AccessToken == accessToken);
        
    }
}
