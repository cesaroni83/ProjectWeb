using ProjectWeb.Shared.Modelo.DTO.Provincia;

namespace ProjectWeb.API.Servicios
{
    public interface IProvincias
    {
        Task<List<ProvinciaDTO>> GetListaAllProvincia();
        Task<ProvinciaDTO> GetProvincia(int id);
        Task<ProvinciaDTO> CreateProvincia(ProvinciaDTO modelo);
        Task<bool> UpdateProvincia(ProvinciaDTO modelo);
        Task<bool> DeleteProvincia(int id);
        Task<List<ProvinciaDTO>> GetListProvinciaActivo(string Estado_Activo);

        Task<List<ProvinciaDropDTO>> GetProvinciaCombo(int id_pais, string Estado_Activo);

        Task<string> GetProvinciaName(int id_Provincia);

        Task<bool> DeleteProvinciaLogica(int id_Provincia);
    }
}
