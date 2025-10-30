using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Shared.Modelo.DTO.Empresa
{
    public class EmpresaDropDTO
    {
        [Display(Name = "ID Empresa")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        public int Id_empresa { get; set; }

        [Display(Name = "Nombre Empresa")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Nombre_Empresa { get; set; } = null!;
    }
}
