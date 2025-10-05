using System.ComponentModel.DataAnnotations;

namespace ProjectWeb.Shared.Modelo.DTO.Pais
{
    public class PaisDropDTO
    {
        [Key]
        [Display(Name = "ID Pais", Order = 1)]
        public int Id_pais { get; set; }

        [Display(Name = "Pais", Order = 2)]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Nombre_pais { get; set; } = null!;
    }
}
