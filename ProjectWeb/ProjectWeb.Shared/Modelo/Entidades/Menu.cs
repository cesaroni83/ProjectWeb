using System.ComponentModel.DataAnnotations;

namespace ProjectWeb.Shared.Modelo.Entidades
{
    public class Menu
    {
        [Key]
        public int Id_menu { get; set; }

        [Display(Name = "Texto")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Descripcion { get; set; } = null!;

        [Display(Name = "Pagina Referencia")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Referencia { get; set; } = string.Empty;

        [Display(Name = "Informacion")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Informacion_menu { get; set; } = string.Empty;

        [Display(Name = "Icono")]
        public string? Icono_name { get; set; }

        [Display(Name = "Color Icono")]
        public string? Icono_color { get; set; }

        [Display(Name = "Menu Padre")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string? Id_parend { get; set; }

        public DateTime Date_reg { get; set; } = DateTime.Now;

        [Display(Name = "Estado Menu")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(10, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Estado_menu { get; set; } = string.Empty;

    }
}
