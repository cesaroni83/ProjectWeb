using Azure.Core;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ProjectWeb.API.Data;
using ProjectWeb.Shared.Google;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ProjectWeb.API.GoogleService
{
    public interface IGoogleAuthorization
    {
        string GetAuthorizationurl();
        Task<UserCredential> ExchangeCodeforToken(string code);
        Task<UserCredential> ValidateToken(string accessToken);
        Task<GoogleUserInfo> GetUserInfoAsync(string accessToken); // <-- método aquí

    }

    public class GoogleAuthorization(AppDbContext context, IGoogleAuth googleHelper, IConfiguration config) : IGoogleAuthorization
    {
        private string RedirectUrl = config["Google:RedirectUri"]!;

        public async Task<UserCredential> ExchangeCodeforToken(string code)
        {
            var flow = new GoogleAuthorizationCodeFlow(
                 new GoogleAuthorizationCodeFlow.Initializer
                 {
                     ClientSecrets = googleHelper.GetClientSecret(),
                     Scopes = googleHelper.GetScope()
                 });
          var token =await flow.ExchangeCodeForTokenAsync("user",code,RedirectUrl,CancellationToken.None);
            //***** creo que guarda en DB el usuario
            context.Add(new Credential
                {
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken,
                    ExpiresInSeconds = token.ExpiresInSeconds,
                    IdToken = token.IdToken,
                    UserId = Guid.NewGuid(),
                    IssuedUtc = token.IssuedUtc
                });
            await context.SaveChangesAsync();
            return new UserCredential(flow, "user", token);
        }

        public string GetAuthorizationurl() => new GoogleAuthorizationCodeFlow(
                new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets=googleHelper.GetClientSecret(),
                    Scopes= googleHelper.GetScope(),
                    Prompt="consent"
                }
            ).CreateAuthorizationCodeRequest(RedirectUrl).Build().ToString();
        
        public async Task<UserCredential> ValidateToken(string accessToken)
        {
            var _credential =await context.Credentials.FirstOrDefaultAsync(c=> c.AccessToken == accessToken)??
                throw new UnauthorizedAccessException("No Se encontro el Token de Authentication.... Login Again");
            var flow = new GoogleAuthorizationCodeFlow(
                new GoogleAuthorizationCodeFlow.Initializer
                { 
                    ClientSecrets=googleHelper.GetClientSecret(),
                    Scopes=googleHelper.GetScope(),
                });
            var tokenResponse = new TokenResponse
            {
                AccessToken = _credential.AccessToken,
                RefreshToken = _credential.RefreshToken,
                ExpiresInSeconds = _credential.ExpiresInSeconds,
                IdToken = _credential.IdToken,
                IssuedUtc = _credential.IssuedUtc
            };
            return new UserCredential(flow, "user", tokenResponse);
        }

        public async Task<GoogleUserInfo> GetUserInfoAsync(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new ArgumentException("Access token no puede ser nulo", nameof(accessToken));

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo");
            if (!response.IsSuccessStatusCode)
                throw new Exception("No se pudo obtener información del usuario de Google");

            var content = await response.Content.ReadAsStringAsync();
            var userInfo = JsonSerializer.Deserialize<GoogleUserInfo>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (userInfo == null)
                throw new Exception("No se pudo deserializar la información del usuario");

            return userInfo;
        }
    }

    public class GoogleUserInfo
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Given_Name { get; set; } = null!; // opcional
        public string Family_Name { get; set; } = null!; // opcional
        
        public string Picture { get; set; } = null!;
    }

}
