
using ProjectWeb.Shared.Enums;

namespace ProjectWeb.API.Servicios
{
    public interface IApiService
    {
        Task<Response> GetListAsync<T>(string servicePrefix, string controller);

        Task<Response> GetPaisesConProvinciasYCiudadesAsync();
    }
}
