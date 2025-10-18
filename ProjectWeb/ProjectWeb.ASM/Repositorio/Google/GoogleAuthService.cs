using ProjectWeb.Shared.Google;

namespace ProjectWeb.ASM.Repositorio.Google
{
    public class GoogleAuthService
    {
        private readonly IRepository _repository;

        public GoogleAuthService(IRepository repository)
        {
            _repository = repository;
        }

        // Obtener URL de autorización de Google
        public async Task<string?> GetAuthorizationUrlAsync()
        {
            var response = await _repository.Get<string>("https://localhost:7135/authorize");
            if (!response.Error)
                return response.Response;
            return null;
        }

        // Obtener el token por userId
        public async Task<Token?> GetTokenAsync(Guid userId)
        {
            var response = await _repository.Get<Token>($"authorize/token/{userId}");
            if (!response.Error)
                return response.Response;
            return null;
        }
    }
}
