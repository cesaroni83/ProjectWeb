using ProjectWeb.Shared.Modelo.Entidades;
using System.ComponentModel.DataAnnotations;

namespace ProjectWeb.Shared.Modelo.DTO.Pais
{
    public class PaisDTO
    {
        [Key]
        [Display(Name = "ID Pais", Order = 2)]
        public int Id_pais { get; set; }

        [Display(Name = "Pais", Order = 3)]
       // [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Nombre_pais { get; set; } = string.Empty;

        [Display(Name = "Informacion De Pais", Order = 4)]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Informacion { get; set; } = string.Empty;

        [Display(Name = "Foto", Order = 1)]
        public byte[]? Foto_pais { get; set; } 

        [Display(Name = "Estado De Pais", Order = 5)]
        //[Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(10, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Estado_pais { get; set; } = string.Empty;

    }
}
