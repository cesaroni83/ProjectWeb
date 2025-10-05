using ProjectWeb.API.Helper;
using ProjectWeb.Shared.Enums;
using ProjectWeb.Shared.Modelo.Entidades;

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
        }
    }
}
