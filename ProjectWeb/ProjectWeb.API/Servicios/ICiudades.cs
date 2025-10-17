using ProjectWeb.Shared.Modelo.DTO.Ciudad;
using ProjectWeb.Shared.Modelo.DTO.Provincia;

namespace ProjectWeb.API.Servicios
{
    public interface ICiudades
    {
        Task<List<CiudadDTO>> GetListaAllCiudades();
        Task<CiudadDTO> GetCiudad(int id);
        Task<CiudadDTO> CreateCiudad(CiudadDTO modelo);
        Task<bool> UpdateCiudad(CiudadDTO modelo);
        Task<bool> DeleteCiudad(int id);
        Task<List<CiudadDTO>> GetListCiudadActivo(string Estado_Activo);

        Task<List<CiudadDropDTO>> GetCiudadCombo(int id_provincia,string Estado_Activo);
        Task<bool> DeleteCiudadLogica(int id_Ciudad);
        Task<List<CiudadDTO>> GetCiudadByProvincia(int id_provincia);
    }
}
