using System.ComponentModel.DataAnnotations;

namespace ProjectWeb.Shared.Modelo.DTO.Producto
{
    public class ProductoDropDTO
    {
        [Key]
        public int Id_producto { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;
    }
}
