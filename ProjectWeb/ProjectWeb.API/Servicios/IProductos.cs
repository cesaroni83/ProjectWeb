using ProjectWeb.Shared.Modelo.DTO.Producto;

namespace ProjectWeb.API.Servicios
{
    public interface IProductos
    {
        Task<List<ProductoDTO>> GetListaAllProductos();
        Task<ProductoDTO> GetProducto(int id);
        Task<ProductoDTO> CreateProducto(ProductoDTO modelo);
        Task<bool> UpdateProducto(ProductoDTO modelo);
        Task<bool> DeleteProducto(int id);
        Task<List<ProductoDTO>> GetListProductoActivo(string Estado_Activo);
        Task<List<ProductoDropDTO>> GetProductoCombo(string Estado_Activo);
        Task<bool> DeleteProductoLogica(int id);
        Task<List<ProductoDTO>> GetProductoByCategoria(int id);
    }
}
