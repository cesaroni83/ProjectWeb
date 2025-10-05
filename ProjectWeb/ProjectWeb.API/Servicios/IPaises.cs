using ProjectWeb.Shared.Modelo.DTO.Pais;

namespace ProjectWeb.API.Servicios
{
    public interface IPaises
    {
        Task<List<PaisDTO>> GetListaAllPaises();
        Task<PaisDTO> GetPais(int id);
        Task<PaisDTO> CreatePais(PaisDTO modelo);
        Task<bool> UpdatePais(PaisDTO modelo);
        Task<bool> DeletePais(int id);
        Task<List<PaisDTO>> GetListPaisActivo(string Estado_Activo);

        Task<List<PaisDropDTO>> GetPaisCombo(string Estado_Activo);
        Task<bool> DeletePaisLogica(int id_pais);
    }
}
