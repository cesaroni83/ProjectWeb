using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Shared.Modelo.DTO.Menu
{
    public class MenuDropDTO
    {
        public int Id_menu { get; set; }

        [Required(ErrorMessage = "Ingrese Nombre al Menu")]
        public string Descripcion { get; set; } = string.Empty;
    }
}
