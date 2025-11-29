using Microsoft.EntityFrameworkCore;
using ProjectWeb.API.Helper;
using ProjectWeb.API.Servicios;
using ProjectWeb.Shared.Enums;
using ProjectWeb.Shared.External;
using ProjectWeb.Shared.Modelo.Entidades;



namespace ProjectWeb.API.Data
{
    public class SeedDB
    {
        private readonly AppDbContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IApiService _apiService;
        string baseUrl = "https://localhost:7135";

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
            await SeedPaisesAsync();/// 
            await CheckRolesAsync();
            await CheckUserAsync("Juana", "Pineda", "juana@hotmail.it", "322 311 4620", "Calle Luna Calle Sol", "", "Milagro", UserType.Employee);
            await CheckUserAsync("Cesar Armando", "Morocho Pucuna", "cesarmop83@gmail.com", "3483304971", "Via Mascagni 6", "", "Melzo", UserType.Admin);
            //await CheckUserAsync( "Christian", "Avila", "avila@hotmail.it", "322 311 4620", "Calle Luna Calle Sol", "", UserType.User);
            await CreateMenu();
            await CheckCategoriesAsync();
            await CheckProductsAsync();
        }
        private async Task CreateMenu()
        {
            if (!_context.Tbl_Menu.Any())
            {
                // 1. Menús padres
                var menuHome = new Menu { Descripcion = "Home", Referencia = "/", Icono_name = "1", Icono_color = "1", Estado_menu = "A" };
                var menuDashboard = new Menu { Descripcion = "DashBoard", Referencia = "/DashBoard", Icono_name = "95", Icono_color = "1", Estado_menu = "A" };
                var menuMantenimiento = new Menu { Descripcion = "Mantenimiento", Referencia = "#", Icono_name = "5", Icono_color = "1", Estado_menu = "A" };
                var menuAdmin = new Menu { Descripcion = "Admin", Referencia = "#", Icono_name = "2", Icono_color = "1", Estado_menu = "A" };
                var menuAuthentication = new Menu { Descripcion = "Authentication", Referencia = "#", Icono_name = "18", Icono_color = "1", Estado_menu = "A" };
                _context.Tbl_Menu.AddRange(menuHome, menuDashboard, menuMantenimiento, menuAdmin, menuAuthentication);
                await _context.SaveChangesAsync(); // Aquí se generan los IDs

                // 2. Menús hijos usando los IDs generados
                var menuPaises = new Menu { Descripcion = "Paises", Referencia = "/ListPaises", Icono_name = "92", Icono_color = "1", Id_parend = menuMantenimiento.Id_menu.ToString(), Estado_menu = "A" };
                var menuEmpresa = new Menu { Descripcion = "Empresa", Referencia = "/ListEmpresa", Icono_name = "94", Icono_color = "1", Id_parend = menuMantenimiento.Id_menu.ToString(), Estado_menu = "A" };
                var menuMenu = new Menu { Descripcion = "Menu", Referencia = "/ListMenu", Icono_name = "93", Icono_color = "1", Id_parend = menuMantenimiento.Id_menu.ToString(), Estado_menu = "A" };
                var menuCategoria = new Menu { Descripcion = "Categoria Producto", Referencia = "/ListCategoria", Icono_name = "52", Icono_color = "1", Id_parend = menuMantenimiento.Id_menu.ToString(), Estado_menu = "A" };
                var menuUser = new Menu { Descripcion = "User", Referencia = "/ListUsers", Icono_name = "87", Icono_color = "1", Id_parend = menuAdmin.Id_menu.ToString(), Estado_menu = "A" };
                var menuLogout = new Menu { Descripcion = "Logout", Referencia = "/logout", Icono_name = "19", Icono_color = "1", Id_parend = menuAuthentication.Id_menu.ToString(), Estado_menu = "A" };
                _context.Tbl_Menu.AddRange(menuPaises, menuEmpresa, menuMenu, menuCategoria, menuUser, menuLogout);
                await _context.SaveChangesAsync();
            }
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
                _context.Tbl_Pais.Add(new Pais
                {
                    Nombre_pais = "Italia",
                    Informacion = "",
                    Foto_pais = null,
                    Date_reg = DateTime.Now,
                    Estado_pais = "A",
                    Provincias = new List<Provincia>
                    {
                        new Provincia {Nombre_provincia="Milan",Informacion_provincia="",Date_reg=DateTime.Now, Estado_provincia="A",
                        Ciudades = new List<Ciudad>
                        {
                            new Ciudad {Nombre_ciudad="Melzo",Informacion_ciudad="",Date_reg=DateTime.Now,Estado_ciudad="A"},
                            new Ciudad {Nombre_ciudad="Inzago",Informacion_ciudad="",Date_reg=DateTime.Now,Estado_ciudad="A"},
                            new Ciudad {Nombre_ciudad="Peschiera Borromeo",Informacion_ciudad="",Date_reg=DateTime.Now,Estado_ciudad="A"},
                        }}}
                });
            }
            await _context.SaveChangesAsync();
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
            await _userHelper.CheckRoleAsync(UserType.Employee.ToString());
        }
        private async Task<User> CheckUserAsync(string firsname, string lastname, string email, string phone, string address, string image, string ciudad_name, UserType userType)
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
                    FirstName = firsname,
                    LastName = lastname,
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
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
        }

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

        private async Task CheckProductsAsync()
        {
            if (!_context.Tbl_Producto.Any())
            {
                await AddProductAsync("Adidas Barracuda", 270000M, 12F, "Deportes", new List<string>() { "adidas_barracuda.png" });
                await AddProductAsync("Adidas Superstar", 250000M, 12F, "Deportes", new List<string>() { "Adidas_superstar.png" });
                await AddProductAsync("AirPods", 1300000M, 12F, "Apple", new List<string>() { "airpos.png", "airpos2.png" });
                await AddProductAsync("Audifonos Bose", 870000M, 12F, "Tecnología", new List<string>() { "audifonos_bose.png" });
                await AddProductAsync("Bicicleta Ribble", 12000000M, 6F, "Deportes", new List<string>() { "bicicleta_ribble.png" });
                await AddProductAsync("Camisa Cuadros", 56000M, 24F, "Ropa", new List<string>() { "camisa_cuadros.png" });
                await AddProductAsync("Casco Bicicleta", 820000M, 12F, "Deportes", new List<string>() { "casco_bicicleta.png", "casco.png" });
                await AddProductAsync("iPad", 2300000M, 6F, "Apple", new List<string>() { "ipad.png" });
                await AddProductAsync("iPhone 13", 5200000M, 6F, "Apple", new List<string>() { "iphone13.png", "iphone13b.png", "iphone13c.png", "iphone13d.png" });
                await AddProductAsync("Mac Book Pro", 12100000M, 6F, "Apple", new List<string>() { "mac_book_pro.png" });
                await AddProductAsync("Mancuernas", 370000M, 12F, "Deportes", new List<string>() { "mancuernas.png" });
                await AddProductAsync("Mascarilla Cara", 26000M, 100F, "Belleza", new List<string>() { "mascarilla_cara.png" });
                await AddProductAsync("New Balance 530", 180000M, 12F, "Deportes", new List<string>() { "newbalance530.png" });
                await AddProductAsync("New Balance 565", 179000M, 12F, "Deportes", new List<string>() { "newbalance565.png" });
                await AddProductAsync("Nike Air", 233000M, 12F, "Deportes", new List<string>() { "nike_air.png" });
                await AddProductAsync("Nike Zoom", 249900M, 12F, "Deportes", new List<string>() { "nike_zoom.png" });
                await AddProductAsync("Buso Adidas Mujer", 134000M, 12F, "Deportes", new List<string>() { "buso_adidas.png" });
                await AddProductAsync("Suplemento Boots Original", 15600M, 12F, "Nutrición", new List<string>() { "Boost_Original.png" });
                await AddProductAsync("Whey Protein", 252000M, 12F, "Nutrición", new List<string>() { "whey_protein.png" });
                await AddProductAsync("Arnes Mascota", 25000M, 12F, "Mascotas", new List<string>() { "arnes_mascota.png" });
                await AddProductAsync("Cama Mascota", 99000M, 12F, "Mascotas", new List<string>() { "cama_mascota.png" });
                await AddProductAsync("Teclado Gamer", 67000M, 12F, "Gamer", new List<string>() { "teclado_gamer.png" });
                await AddProductAsync("Silla Gamer", 980000M, 12F, "Gamer", new List<string>() { "silla_gamer.png" });
                await AddProductAsync("Mouse Gamer", 132000M, 12F, "Gamer", new List<string>() { "mouse_gamer.png" });
                await _context.SaveChangesAsync();
            }
        }
        private async Task<Categoria> GetCategoryAsync(string categoryDescription)
        {
            // Recupera la categoria dalla base di dati
            return await _context.Tbl_Categoria.FirstOrDefaultAsync(c => c.Descripcion_Cat == categoryDescription);
        }

        private async Task AddProductAsync(string name,decimal price,float stock,string categories, List<string> images)
        {
            // Crear producto
            Producto prodcut = new()
            {
                Description = name,
                Name = name,
                Price = price,
                Stock = stock,
                Estado_Producto = "A",
                Id_Categoria = (await GetCategoryAsync(categories)).Id_Categoria,
                ProductImages = new List<ImagenProd>()
            };

            // =========================================================
            // 1. IMAGEN PRINCIPAL
            // =========================================================
            string? mainImage = images.FirstOrDefault();

            if (!string.IsNullOrEmpty(mainImage))
            {
                string mainImagePath = Path.Combine(
                    Environment.CurrentDirectory,
                    "wwwroot",
                    "Productos",
                    "Imagenes",
                    mainImage
                );

                if (System.IO.File.Exists(mainImagePath))
                {
                    // URL pública usando la variable baseUrl
                    string mainImageUrl = $"{baseUrl}/Productos/Imagenes/{mainImage}";
                    prodcut.MainImage = mainImageUrl;
                }
                else
                {
                    Console.WriteLine($"Imagen principal no encontrada: {mainImagePath}");
                }
            }

            // Guardar el producto para obtener el ID
            _context.Tbl_Producto.Add(prodcut);
            await _context.SaveChangesAsync();

            // =========================================================
            // 2. AGREGAR TODAS LAS IMÁGENES COMO ImagenProd
            // =========================================================
            foreach (string image in images)
            {
                try
                {
                    string filePath = Path.Combine(
                        Environment.CurrentDirectory,
                        "wwwroot",
                        "Productos",
                        "Imagenes",
                        image
                    );

                    if (!System.IO.File.Exists(filePath))
                    {
                        Console.WriteLine($"Imagen no encontrada: {filePath}");
                        continue;
                    }

                    // URL pública usando la variable baseUrl
                    string publicUrl = $"{baseUrl}/Productos/Imagenes/{image}";

                    var newImage = new ImagenProd
                    {
                        Name_imagen = image,
                        Descripcion_imagen = "Descripción imagen",
                        Foto_Producto = publicUrl,
                        Tipo_Imagen = "True",
                        Estado_Imagen = "A",
                        Date_reg = DateTime.Now,
                        Id_producto = prodcut.Id_producto
                    };

                    prodcut.ProductImages.Add(newImage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error durante el tratamiento de la imagen {image}: {ex.Message}");
                }
            }

            // Guardar todas las imágenes asociadas al producto
            await _context.SaveChangesAsync();
        }
        private async Task CheckCategoriesAsync()
        {
            if (!_context.Tbl_Categoria.Any())
            {
                _context.Tbl_Categoria.Add(new Categoria { Descripcion_Cat = "Apple" });
                _context.Tbl_Categoria.Add(new Categoria { Descripcion_Cat = "Autos" });
                _context.Tbl_Categoria.Add(new Categoria { Descripcion_Cat = "Belleza" });
                _context.Tbl_Categoria.Add(new Categoria { Descripcion_Cat = "Calzado" });
                _context.Tbl_Categoria.Add(new Categoria { Descripcion_Cat = "Comida" });
                _context.Tbl_Categoria.Add(new Categoria { Descripcion_Cat = "Cosmeticos" });
                _context.Tbl_Categoria.Add(new Categoria { Descripcion_Cat = "Deportes" });
                _context.Tbl_Categoria.Add(new Categoria { Descripcion_Cat = "Erótica" });
                _context.Tbl_Categoria.Add(new Categoria { Descripcion_Cat = "Ferreteria" });
                _context.Tbl_Categoria.Add(new Categoria { Descripcion_Cat = "Gamer" });
                _context.Tbl_Categoria.Add(new Categoria { Descripcion_Cat = "Hogar" });
                _context.Tbl_Categoria.Add(new Categoria { Descripcion_Cat = "Jardín" });
                _context.Tbl_Categoria.Add(new Categoria { Descripcion_Cat = "Jugetes" });
                _context.Tbl_Categoria.Add(new Categoria { Descripcion_Cat = "Lenceria" });
                _context.Tbl_Categoria.Add(new Categoria { Descripcion_Cat = "Mascotas" });
                _context.Tbl_Categoria.Add(new Categoria { Descripcion_Cat = "Nutrición" });
                _context.Tbl_Categoria.Add(new Categoria { Descripcion_Cat = "Ropa" });
                _context.Tbl_Categoria.Add(new Categoria { Descripcion_Cat = "Tecnología" });
                await _context.SaveChangesAsync();
            }
        }

    }
}
