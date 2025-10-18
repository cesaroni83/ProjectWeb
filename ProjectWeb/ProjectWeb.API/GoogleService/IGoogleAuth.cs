using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Microsoft.AspNetCore.Mvc;

namespace ProjectWeb.API.GoogleService
{
    public interface IGoogleAuth
    {
        string[] GetScope();
        string ScopeToString();

        ClientSecrets GetClientSecret();   


    }
    public class GoogleAuth(IConfiguration conf) : IGoogleAuth
    {
        public ClientSecrets GetClientSecret()
        {
            string clientId = conf["Google:ClientId"]!;
            string clientSecret = conf["Google:ClientSecret"]!;
            return new () { ClientId = clientId, ClientSecret = clientSecret };
        }

        public string[] GetScope()
        {
            var scope = new[]
            {
                Oauth2Service.Scope.Openid,
                Oauth2Service.Scope.UserinfoEmail,
                Oauth2Service.Scope.UserinfoProfile
            };
            return scope;
        }

        public string ScopeToString()=> string.Join(",",GetScope());
        
    }

}
