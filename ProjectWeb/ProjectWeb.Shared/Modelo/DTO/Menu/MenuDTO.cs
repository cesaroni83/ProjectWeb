using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Shared.Modelo.DTO.Menu
{
    public class MenuDTO
    {
        [Key]
        [Display(Name = "ID Menu")]
        public int Id_menu { get; set; }

        [Required(ErrorMessage = "Ingrese Nombre al Menu")]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Display(Name = "Pagina")]
        public string? Referencia { get; set; } = string.Empty;

        [Display(Name = "Informacion")]
        public string? Informacion_menu { get; set; } = string.Empty;

        [Display(Name = "Icono")]
        public string? Icono_name { get; set; } = string.Empty;

        [Display(Name = "Color")]
        public string? Icono_color { get; set; } = string.Empty;

        
        public string? Id_parend { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese El Estado Al Menu")]

        [Display(Name = "Estado")]
        public string Estado_menu { get; set; } = string.Empty;

        [Display(Name = "SubMenu")]
        public string Nombre_padre { get; set; } = string.Empty;
    }
}
