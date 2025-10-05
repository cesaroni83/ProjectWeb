using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProjectWeb.Shared.Modelo.Entidades
{
    public class Empresa
    {
        [Key]
        [Display(Name = "ID Empresa")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        public int Id_empresa { get; set; }

        [Display(Name = "Nombre Empresa")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Nombre_Empresa { get; set; } = null!;

        [Display(Name = "Razon Social")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Razon_social { get; set; } = null!;

        [Display(Name = "RUC")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Ruc { get; set; } = null!;

        [Display(Name = "Ciudad")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        public int Id_ciudad { get; set; }


        [Display(Name = "Direccion")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Direccion { get; set; } = null!;


        [Display(Name = "CAP")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(10, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Cap { get; set; } = string.Empty;

        [Display(Name = "Telefono")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(20, ErrorMessage = "El Campo {0} no puede mas de {2} Caracteres")]
        public string Telefono { get; set; } = null!;


        [Display(Name = "Telefono Secundario")]
        [MaxLength(20, ErrorMessage = "El Campo {0} no puede mas de {2} Caracteres")]
        public string Telefono_secundario { get; set; } = string.Empty;

        [Display(Name = "Pagina Web")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Pagina_web { get; set; } = null!;


        [Display(Name = "Email")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Tipo Empresa")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Tipo_empresa { get; set; } = null!;


        [Display(Name = "Representante Legal")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Representante_legal { get; set; } = string.Empty;

        [Display(Name = "Capital Social")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [Range(0, 99999999999.99, ErrorMessage = "El Campo {0} debe estar entre {1} y {2}")]
        [Precision(13, 2)]
        public Decimal Capital_social { get; set; }


        [Display(Name = "Logo")]
        public byte[]? Logo { get; set; }


        [Display(Name = "Fecha De Registro")]
        public DateTime Date_reg { get; set; } = DateTime.Now;


        [Display(Name = "Estado Empresa")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(10, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Estado_empresa { get; set; } = string.Empty;


        public Ciudad? Ciudades { get; set; }

        public ICollection<Sucursal>? Sucursales { get; set; }
        public int SucursalCount => Sucursales == null ? 0 : Sucursales.Count();

    }
}
