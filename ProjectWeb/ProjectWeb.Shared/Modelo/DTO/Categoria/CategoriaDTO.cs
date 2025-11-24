using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Shared.Modelo.DTO.Categoria
{
    public class CategoriaDTO
    {
        public int Id_Categoria { get; set; }

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Descripcion_Cat { get; set; } = null!;

        [Display(Name = "Informacion De Categoria")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Informacion_Cat { get; set; } = string.Empty;

        [Display(Name = "Estado Catergoria")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(10, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Estado_Cat { get; set; } = string.Empty;
    }
}
