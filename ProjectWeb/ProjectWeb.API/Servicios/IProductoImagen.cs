
using ProjectWeb.Shared.Modelo.DTO.ProductImage;
using ProjectWeb.Shared.Modelo.DTO.Producto;

namespace ProjectWeb.API.Servicios
{
    public interface IProductoImagen
    {
        Task<List<ImagenProdDTO>> GetListaAllImagenProd();
        Task<ImagenProdDTO> GetImagenProd(int id);
        Task<ImagenProdDTO> CreateImagenProd(ImagenProdDTO modelo);
        Task<bool> UpdateImagenProd(ImagenProdDTO modelo);
        Task<bool> DeleteImagenProd(int id);
        Task<List<ImagenProdDTO>> GetListImagenProdActivo(string Estado_Activo);
        //Task<List<ImagenProdDropDTO>> GetImagenProdCombo(string Estado_Activo);
        Task<bool> DeleteImagenProdLogica(int id_pais);
        Task<List<ImagenProdDTO>> GetImagenByProducto(int id);
    }
}
