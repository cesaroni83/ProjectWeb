using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.API.Helper;
using ProjectWeb.API.Servicios;
using ProjectWeb.Shared.Enums;
using ProjectWeb.Shared.External;
using ProjectWeb.Shared.Modelo.Entidades;
using System.Collections.Concurrent;



namespace ProjectWeb.API.Data
{
    public class SeedDB
    {
        private readonly AppDbContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IApiService _apiService;

        public SeedDB(AppDbContext context, IUserHelper userHelper, IApiService apiService)
        {
            _context = context;
            _userHelper = userHelper;
            _apiService = apiService;
        }
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            //await CheckPaisAsync();
            await SeedPaisesAsync();/// craga ma si de pais, provincias y cantones pero trae error con los cantones
            await CheckRolesAsync();
            await CheckUserAsync("Juana", "Pineda", "juana@hotmail.it", "322 311 4620", "Calle Luna Calle Sol", "", "Milagro", UserType.Employee);
            await CheckUserAsync( "Cesar Armando", "Morocho Pucuna", "cesarmop83@gmail.com", "3483304971", "Via Mascagni 6", "", "Milan",UserType.Admin);
            //await CheckUserAsync( "Christian", "Avila", "avila@hotmail.it", "322 311 4620", "Calle Luna Calle Sol", "", UserType.User);
        }
        private async Task CheckPaisAsync()
        {
            if (!_context.Tbl_Pais.Any())
            {
                _context.Tbl_Pais.Add(new Pais
                {
                    Nombre_pais = "Ecuador",
                    Informacion = "",
                    Foto_pais = null,
                    Date_reg = DateTime.Now,
                    Estado_pais = "A",
                    Provincias = new List<Provincia>
                    {
                        new Provincia {Nombre_provincia="Guayas",Informacion_provincia="",Date_reg=DateTime.Now, Estado_provincia="A",
                        Ciudades = new List<Ciudad>
                        {
                            new Ciudad {Nombre_ciudad="Guayaquil",Informacion_ciudad="",Date_reg=DateTime.Now,Estado_ciudad="A"},
                            new Ciudad {Nombre_ciudad="Milagro",Informacion_ciudad="",Date_reg=DateTime.Now,Estado_ciudad="A"},
                            new Ciudad {Nombre_ciudad="Naranjito",Informacion_ciudad="",Date_reg=DateTime.Now,Estado_ciudad="A"},
                        }}}
                });
                _context.Tbl_Pais.Add(new Pais { Nombre_pais = "Peru", Informacion = "", Foto_pais = null, Date_reg = DateTime.Now, Estado_pais = "A" });
                _context.Tbl_Pais.Add(new Pais { Nombre_pais = "Colombia", Informacion = "", Foto_pais = null, Date_reg = DateTime.Now, Estado_pais = "A" });
                _context.Tbl_Pais.Add(new Pais { Nombre_pais = "Bolivia", Informacion = "", Foto_pais = null, Date_reg = DateTime.Now, Estado_pais = "A" });
                _context.Tbl_Pais.Add(new Pais { Nombre_pais = "Chile", Informacion = "", Foto_pais = null, Date_reg = DateTime.Now, Estado_pais = "A" });
                _context.Tbl_Pais.Add(new Pais { Nombre_pais = "Argentina", Informacion = "", Foto_pais = null, Date_reg = DateTime.Now, Estado_pais = "A" });
                _context.Tbl_Pais.Add(new Pais { Nombre_pais = "Brazil", Informacion = "", Foto_pais = null, Date_reg = DateTime.Now, Estado_pais = "A" });
                _context.Tbl_Pais.Add(new Pais { Nombre_pais = "Paraguay", Informacion = "", Foto_pais = null, Date_reg = DateTime.Now, Estado_pais = "A" });
                _context.Tbl_Pais.Add(new Pais { Nombre_pais = "Uruguay", Informacion = "", Foto_pais = null, Date_reg = DateTime.Now, Estado_pais = "A" });
                _context.Tbl_Pais.Add(new Pais { Nombre_pais = "Venezuela", Informacion = "", Foto_pais = null, Date_reg = DateTime.Now, Estado_pais = "A" });
                _context.Tbl_Pais.Add(new Pais { Nombre_pais = "Estado Unidos", Informacion = "", Foto_pais = null, Date_reg = DateTime.Now, Estado_pais = "A" });
                _context.Tbl_Pais.Add(new Pais { Nombre_pais = "Italia", Informacion = "", Foto_pais = null, Date_reg = DateTime.Now, Estado_pais = "A" });
            }
            await _context.SaveChangesAsync();
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
            await _userHelper.CheckRoleAsync(UserType.Employee.ToString());
        }
        private async Task<User> CheckUserAsync(string firstName, string lastName, string email, string phone, string address, string image,  string ciudad_name, UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                var city = await _context.Tbl_Ciudad.FirstOrDefaultAsync(x => x.Nombre_ciudad == ciudad_name);
                if (city == null)
                {
                    city = await _context.Tbl_Ciudad.FirstOrDefaultAsync();
                }
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Id_ciudad = city!.Id_ciudad,
                    Ciudades = city,
                    UserType = userType,
                    Photo = null,
                };
                await _userHelper.AddUserAsync(user, "Carolina");
                //***CREA LA PERSONA ****//
                var persona = new Persona
                {
                    Id_user = user.Id,
                    Nombre = user.FirstName,
                    Apellido = user.LastName,
                    Id_ciudad = user.Id_ciudad,
                    Direccion_persona = user.Address,
                    Email = user.Email!,
                    Tipo_persona = userType.ToString(),
                    Estado_persona = "A"
                };
                _context.Add(persona);
                var confirma = await _context.SaveChangesAsync();
                //if (confirma <= 0)
                //    return BadRequest("No se pudo guardar la persona");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
        }

        //private async Task CheckCountriesAsync()
        //{
        //    if (!_context.Tbl_Pais.Any())
        //    {
        //        Response responseCountries = await _apiService.GetListAsync<CountryResponse>("/v1", "/countries");
        //        if (responseCountries.IsSuccess)
        //        {
        //            List<CountryResponse> countries = (List<CountryResponse>)responseCountries.Result!;
        //            foreach (CountryResponse countryResponse in countries)
        //            {
        //                // Verifica si el país ya existe en la base de datos
        //                var existingCountry = await _context.Tbl_Pais
        //                    .Include(p => p.Provincias!)
        //                    .ThenInclude(pr => pr.Ciudades)
        //                    .FirstOrDefaultAsync(c => c.Nombre_pais == countryResponse.Name);

        //                if (existingCountry == null)
        //                {
        //                    var country = new Pais
        //                    {
        //                        Nombre_pais = countryResponse.Name!,
        //                        Informacion = "",
        //                        Foto_pais = null,
        //                        Estado_pais = "A",
        //                        Provincias = new List<Provincia>()
        //                    };

        //                    Response responseStates = await _apiService.GetListAsync<StateResponse>("/v1", $"/countries/{countryResponse.Iso2}/states");
        //                    if (responseStates.IsSuccess)
        //                    {
        //                        List<StateResponse> states = (List<StateResponse>)responseStates.Result!;
        //                        foreach (StateResponse stateResponse in states)
        //                        {
        //                            // Verifica si la provincia ya existe
        //                            var existingState = await _context.Tbl_Provincia
        //                                .FirstOrDefaultAsync(s => s.Nombre_provincia == stateResponse.Name && s.Id_pais == country.Id_pais);

        //                            if (existingState == null)
        //                            {
        //                                var state = new Provincia
        //                                {
        //                                    Nombre_provincia = stateResponse.Name!,
        //                                    Informacion_provincia = "",
        //                                    Estado_provincia = "A",
        //                                    Ciudades = new List<Ciudad>()
        //                                };

        //                                Response responseCities = await _apiService.GetListAsync<CityResponse>("/v1", $"/countries/{countryResponse.Iso2}/states/{stateResponse.Iso2}/cities");
        //                                if (responseCities.IsSuccess)
        //                                {
        //                                    List<CityResponse> cities = (List<CityResponse>)responseCities.Result!;
        //                                    foreach (CityResponse cityResponse in cities)
        //                                    {
        //                                        if (cityResponse.Name == "Mosfellsbær" || cityResponse.Name == "Șăulița")
        //                                            continue;

        //                                        // Verifica si la ciudad ya existe en la BD
        //                                        var existingCity = await _context.Tbl_Ciudad
        //                                            .AsNoTracking()
        //                                            .FirstOrDefaultAsync(c => c.Nombre_ciudad == cityResponse.Name);

        //                                        if (existingCity == null)
        //                                        {
        //                                            state.Ciudades.Add(new Ciudad
        //                                            {
        //                                                Nombre_ciudad = cityResponse.Name!,
        //                                                Informacion_ciudad = "",
        //                                                Estado_ciudad = "A"
        //                                            });
        //                                        }
        //                                    }
        //                                }

        //                                // Solo agrega la provincia si tiene ciudades
        //                                if (state.Ciudades.Any())
        //                                {
        //                                    country.Provincias.Add(state);
        //                                }
        //                            }
        //                        }
        //                    }

        //                    // Solo agrega el país si tiene provincias
        //                    if (country.Provincias.Any())
        //                    {
        //                        _context.Tbl_Pais.Add(country);
        //                    }
        //                }
        //            }

        //            // ✅ Guardamos todos los cambios una sola vez, fuera del bucle
        //            try
        //            {
        //                await _context.SaveChangesAsync();
        //            }
        //            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
        //            {
        //                // 2601 = Violación de índice único
        //                Console.WriteLine($"⚠️ Ciudad duplicada ignorada: {sqlEx.Message}");
        //            }
        //        }
        //    }
        //}

        public async Task SeedPaisesAsync()
        {
            // 1️⃣ Verificar si ya hay datos
            if (_context.Tbl_Pais.Any())
                return;

            try
            {
                // 2️⃣ Llamar al ApiService para traer todo de una sola vez
                var response = await _apiService.GetPaisesConProvinciasYCiudadesAsync();

                if (!response.IsSuccess || response.Result == null)
                {
                    Console.WriteLine($"❌ Error al obtener países: {response.Message}");
                    return;
                }

                // 3️⃣ Convertir el Result a la lista de DTO
                var paises = (List<PaisGlobalDTO>)response.Result;

                foreach (var paisDto in paises)
                {
                    // Evitar duplicados por nombre de país
                    if (_context.Tbl_Pais.Any(p => p.Nombre_pais == paisDto.Nombre_pais))
                        continue;

                    // Mapear DTO a entidad, evitando duplicados de provincias y ciudades
                    var pais = new Pais
                    {
                        Nombre_pais = paisDto.Nombre_pais,
                        Informacion = paisDto.Informacion ?? "",
                        Foto_pais = paisDto.Foto_pais,
                        Estado_pais = paisDto.Estado_pais ?? "A",
                        Provincias = paisDto.Provincias?
                            .GroupBy(p => p.Nombre_provincia)    // Agrupa por nombre de provincia
                            .Select(g => g.First())               // Solo toma el primero
                            .Select(provDto => new Provincia
                            {
                                Nombre_provincia = provDto.Nombre_provincia,
                                Informacion_provincia = provDto.Informacion_provincia ?? "",
                                Estado_provincia = provDto.Estado_provincia ?? "A",
                                Ciudades = provDto.Ciudades?
                                    .GroupBy(c => c.Nombre_ciudad) // Agrupa por nombre de ciudad
                                    .Select(cg => cg.First())      // Solo toma el primero
                                    .Select(ciudadDto => new Ciudad
                                    {
                                        Nombre_ciudad = ciudadDto.Nombre_ciudad,
                                        Informacion_ciudad = ciudadDto.Informacion_ciudad ?? "",
                                        Estado_ciudad = ciudadDto.Estado_ciudad ?? "A"
                                    }).ToList() ?? new List<Ciudad>()
                            }).ToList() ?? new List<Provincia>()
                    };

                    _context.Tbl_Pais.Add(pais);
                }

                // 4️⃣ Guardar todos los cambios de una sola vez
                await _context.SaveChangesAsync();
                 Console.WriteLine("✅ Países, provincias y ciudades cargados correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en SeedPaisesAsync: {ex.Message}");
            }
        }


    }
}
