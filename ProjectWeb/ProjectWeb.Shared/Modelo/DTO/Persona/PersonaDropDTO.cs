using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Shared.Modelo.DTO.Persona
{
    public class PersonaDropDTO
    {
        [Key]
        [Display(Name = "ID Persona")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        public int Id_persona { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Apellido { get; set; } = null!;
    }
}
