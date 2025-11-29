using ProjectWeb.Shared.Modelo.DTO.Producto;
using ProjectWeb.Shared.Modelo.Entidades;

namespace ProjectWeb.Shared.Modelo.DTO.TemporalSale
{
    public class TemporalSaleDTO
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public float Quantity { get; set; } = 1;

        public string Remarks { get; set; } = string.Empty;

        public ProductoDTO? Product { get; set; }    
        public decimal Value => Product == null ? 0 : Product.Price * (decimal)Quantity;
    }
}
