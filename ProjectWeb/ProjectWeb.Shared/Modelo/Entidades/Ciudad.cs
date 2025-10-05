using ProjectWeb.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectWeb.Shared.Modelo.Entidades
{
    public class Ciudad
    {
        [Key]
        [Display(Name = "ID Ciudad")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        public int Id_ciudad { get; set; }


        [Display(Name = "Provincia")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        public int Id_provincia { get; set; }

        [Display(Name = "Ciudad")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Nombre_ciudad { get; set; } = null!;

        [Display(Name = "Informacion De Ciudad")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Informacion_ciudad { get; set; } = string.Empty;

        [Display(Name = "Fecha De Registro")]
        public DateTime Date_reg { get; set; } = DateTime.Now;


        [Display(Name = "Estado Ciudad")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(10, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Estado_ciudad { get; set; } = string.Empty;

        public Provincia? Provincias { get; set; }

        public ICollection<User>? Users { get; set; }
        public ICollection<Empresa>? Empresas { get; set; }
        public ICollection<Persona>? Personas { get; set; }
        public ICollection<Sucursal>? Sucursales { get; set; }
    }
}
