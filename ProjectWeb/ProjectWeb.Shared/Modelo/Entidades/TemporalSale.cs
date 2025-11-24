using ProjectWeb.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectWeb.Shared.Modelo.Entidades
{
    public class TemporalSale
    {
        [Required]
        [Key]
        public int Id { get; set; }

        public User? User { get; set; }

        public string? UserId { get; set; }

        public Producto? Product { get; set; }

        public int Id_producto { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }
        public decimal Value => Product == null ? 0 : Product.Price * (decimal)Quantity;
    }
}
