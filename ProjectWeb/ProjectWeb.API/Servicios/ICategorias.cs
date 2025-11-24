using ProjectWeb.Shared.Modelo.DTO.Categoria;

namespace ProjectWeb.API.Servicios
{
    public interface ICategorias
    {
        Task<List<CategoriaDTO>> GetListaAllCategorias();
        Task<CategoriaDTO> GetCategoria(int id);
        Task<CategoriaDTO> CreateCategoria(CategoriaDTO modelo);
        Task<bool> UpdateCategoria(CategoriaDTO modelo);
        Task<bool> DeleteCategoria(int id);
        Task<List<CategoriaDTO>> GetListCategoriaActivo(string Estado_Activo);
        Task<List<CategoriaDropDTO>> GetCategoriaCombo(string Estado_Activo);
        Task<bool> DeleteCategoriaLogica(int id_pais);
    }
}
