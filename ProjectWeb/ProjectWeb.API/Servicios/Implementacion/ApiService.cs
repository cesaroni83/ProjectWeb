using Newtonsoft.Json;
using ProjectWeb.Shared.Enums;
using ProjectWeb.Shared.External;
using Response = ProjectWeb.Shared.Enums.Response;

namespace ProjectWeb.API.Servicios.Implementacion
{
    public class ApiService : IApiService
    {
        private readonly string _urlBase;
        private readonly string _tokenName;
        private readonly string _tokenValue;

        // 🔹 HttpClient reutilizable
        private static readonly HttpClient _httpClient = new HttpClient();

        public ApiService(IConfiguration configuration)
        {
            _urlBase = configuration["CoutriesAPI:urlBase"]!;
            _tokenName = configuration["CoutriesAPI:tokenName"]!;
            _tokenValue = configuration["CoutriesAPI:tokenValue"]!;

            // ⚙️ Configuración inicial del HttpClient
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(_urlBase);
                _httpClient.Timeout = TimeSpan.FromSeconds(30); // ⏱️ timeout
                if (!_httpClient.DefaultRequestHeaders.Contains(_tokenName))
                    _httpClient.DefaultRequestHeaders.Add(_tokenName, _tokenValue);
            }
        }

        public async Task<Response> GetListAsync<T>(string servicePrefix, string controller)
        {
            try
            {
                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = $"Error {(int)response.StatusCode}: {response.ReasonPhrase} -> {result}"
                    };
                }

                List<T>? list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"❌ Error HTTP: {ex.Message}");
                return new Response
                {
                    IsSuccess = false,
                    Message = $"Error de red: {ex.Message}"
                };
            }
            catch (TaskCanceledException)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "La solicitud superó el tiempo de espera (timeout)."
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = $"Error inesperado: {ex.Message}"
                };
            }
        }

        public async Task<Response> GetPaisesConProvinciasYCiudadesAsync()
        {
            try
            {
                Console.WriteLine("🌍 Iniciando carga de países...");

                // 1️⃣ Obtener todos los países
                var countriesResponse = await GetListAsync<CountryResponse>("/v1", "/countries");
                if (!countriesResponse.IsSuccess || countriesResponse.Result == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "No se pudieron obtener los países"
                    };
                }

                var countries = (List<CountryResponse>)countriesResponse.Result!;
                var paisesCompletos = new List<PaisGlobalDTO>();

                // 🔒 Limitar concurrencia de países
                var countrySemaphore = new SemaphoreSlim(60);
                var citySemaphore = new SemaphoreSlim(60); // 🔒 Limitar concurrencia de ciudades

                var countryTasks = countries.Select(async country =>
                {
                    await countrySemaphore.WaitAsync();
                    try
                    {
                        var paisDto = new PaisGlobalDTO
                        {
                            Nombre_pais = country.Name ?? string.Empty,
                            Estado_pais = "A",
                            Informacion = "",
                            Provincias = new List<ProvinciaGlobalDTO>()
                        };

                        // 🏙️ Obtener estados/provincias del país
                        var statesResponse = await GetListAsync<StateResponse>("/v1", $"/countries/{country.Iso2}/states");
                        if (statesResponse.IsSuccess && statesResponse.Result != null)
                        {
                            var states = (List<StateResponse>)statesResponse.Result!;
                            var provinceTasks = states.Select(async state =>
                            {
                                var provinciaDto = new ProvinciaGlobalDTO
                                {
                                    Nombre_provincia = state.Name ?? string.Empty,
                                    Estado_provincia = "A",
                                    Informacion_provincia = "",
                                    Ciudades = new List<CiudadGlobalDTO>()
                                };

                                if (!string.IsNullOrWhiteSpace(state.Iso2))
                                {
                                    await citySemaphore.WaitAsync();
                                    try
                                    {
                                        var citiesResponse = await GetListAsync<CityResponse>(
                                            "/v1",
                                            $"/countries/{country.Iso2}/states/{state.Iso2}/cities"
                                        );

                                        if (citiesResponse.IsSuccess && citiesResponse.Result != null)
                                        {
                                            var ciudades = (List<CityResponse>)citiesResponse.Result!;
                                            provinciaDto.Ciudades = ciudades
                                                .Where(c => c.Name != "Mosfellsbær" && c.Name != "Șăulița")
                                                .Select(c => new CiudadGlobalDTO
                                                {
                                                    Nombre_ciudad = c.Name ?? string.Empty,
                                                    Estado_ciudad = "A",
                                                    Informacion_ciudad = ""
                                                })
                                                .ToList();
                                        }
                                        else
                                        {
                                            Console.WriteLine($"⚠️ No se pudieron obtener ciudades de {state.Name} ({country.Name})");
                                        }
                                    }
                                    finally
                                    {
                                        citySemaphore.Release();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"⚠️ Estado sin ISO2: {state.Name} ({country.Name})");
                                }

                                return provinciaDto;
                            });

                            paisDto.Provincias = (await Task.WhenAll(provinceTasks)).ToList();
                        }

                        return paisDto;
                    }
                    finally
                    {
                        countrySemaphore.Release();
                    }
                });

                paisesCompletos = (await Task.WhenAll(countryTasks)).ToList();

                Console.WriteLine($"✅ Carga completa. Países totales: {paisesCompletos.Count}");

                return new Response
                {
                    IsSuccess = true,
                    Message = "Datos cargados correctamente",
                    Result = paisesCompletos
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error general: {ex.Message}");
                return new Response
                {
                    IsSuccess = false,
                    Message = $"Error al cargar países con provincias y ciudades: {ex.Message}"
                };
            }
        }
    }
}
