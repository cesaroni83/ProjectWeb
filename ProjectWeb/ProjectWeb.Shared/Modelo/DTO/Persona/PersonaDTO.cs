using ProjectWeb.Shared.Enums;
using ProjectWeb.Shared.Modelo.DTO.Ciudad;
using System.ComponentModel.DataAnnotations;

namespace ProjectWeb.Shared.Modelo.DTO.Persona
{
    public class PersonaDTO
    {
        [Key]
        [Display(Name = "ID Persona")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        public int Id_persona { get; set; }

        [Display(Name = "ID Usuario")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        public string Id_user { get; set; } = null!;

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Apellido { get; set; } = null!;

        [Display(Name = "Tipo Documento")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Tipo_documento { get; set; } = null!;

        [Display(Name = "Numero Documento")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Numero_documento { get; set; } = null!;

        [Display(Name = "Fecha De Nacimiento")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        public DateTime Date_nacimiento { get; set; }

        [Display(Name = "Genero")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(10, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Genero { get; set; } = null!;

        [Display(Name = "Estado Civil")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(10, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Estado_civil { get; set; } = null!;

        [Display(Name = "Nacionalidad")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Nacionalidad { get; set; } = null!;

        [Display(Name = "Ocupacion")]
        [MaxLength(50, ErrorMessage = "El Campo {0} no puede mas de {5} Caracteres")]
        public string Ocupacion { get; set; } = null!;

        [Display(Name = "Nivel De Estudio")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Nivel_estudio { get; set; } = null!;

        public CiudadDTO? Ciudades { get; set; }

        [Display(Name = "Ciudad")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        public int Id_ciudad { get; set; }

        [Display(Name = "CAP")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(10, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Cap_persona { get; set; } = string.Empty;

        [Display(Name = "Direccion")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Direccion_persona { get; set; } = null!;

        [Display(Name = "Telefono")]
        //[Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(20, ErrorMessage = "El Campo {0} no puede mas de {2} Caracteres")]
        public string Telefono { get; set; } = null!;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Foto")]
        public byte[]? Foto { get; set; }


        [Display(Name = "Informacion Personal")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Informacion { get; set; } = string.Empty;

        [Display(Name = "Estado Persona")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(10, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Estado_persona { get; set; } = string.Empty;

        [Display(Name = "Tipo Persona")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        public string? Tipo_persona { get; set; }
    }
}
