using Microsoft.EntityFrameworkCore;
using ProjectWeb.API.Helper;
using ProjectWeb.API.Helper.Implementacion;
using ProjectWeb.Shared.Enums;
using ProjectWeb.Shared.Modelo.Entidades;
using System.Runtime.InteropServices;

namespace ProjectWeb.API.Data
{
    public class SeedDB
    {
        private readonly AppDbContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDB(AppDbContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckPaisAsync();
            await CheckRolesAsync();
            await CheckUserAsync("Juana", "Pineda", "juana@hotmail.it", "322 311 4620", "Calle Luna Calle Sol", "", UserType.Employee);
            await CheckUserAsync( "Cesar Armando", "Morocho Pucuna", "cesarmop83@gmail.com", "3483304971", "Via Mascagni 6", "", UserType.Admin);
            await CheckUserAsync( "Christian", "Avila", "avila@hotmail.it", "322 311 4620", "Calle Luna Calle Sol", "", UserType.User);
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
        private async Task<User> CheckUserAsync( string firstName, string lastName, string email, string phone, string address, string image, UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                var city = await _context.Tbl_Ciudad.FirstOrDefaultAsync(x => x.Nombre_ciudad == "Naranjito");
                if (city == null)
                {
                    city = await _context.Tbl_Ciudad.FirstOrDefaultAsync();
                }

                //string filePath;
                //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                //{
                //    filePath = $"{Environment.CurrentDirectory}\\Images\\users\\{image}";
                //}
                //else
                //{
                //    filePath = $"{Environment.CurrentDirectory}/Images/users/{image}";
                //}

                //var fileBytes = File.ReadAllBytes(filePath);
                //var imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "users");

                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Id_ciudad = city!.Id_ciudad,
                    Ciudades=city,
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
                    Tipo_persona= userType.ToString(),
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
    }
}
