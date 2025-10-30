using ProjectWeb.Shared.Modelo.DTO.Empresa;
using ProjectWeb.Shared.Modelo.DTO.Sucursal;

namespace ProjectWeb.API.Servicios
{
    public interface ISucursales
    {
        Task<List<SucursalDTO>> GetListaAllSucursales();
        Task<SucursalDTO> GetSucursal(int id);

        Task<SucursalDTO> GetSucursalAllDate(int id);
        Task<List<SucursalDTO>> GetSucursalByEmpresa(int id);
        Task<SucursalDTO> CreateSucursal(SucursalDTO modelo);
        Task<bool> UpdateSucursal(SucursalDTO modelo);
        Task<bool> DeleteSucursal(int id);
        Task<List<SucursalDTO>> GetListSucursalActivo(string Estado_Activo);

        Task<List<SucursalDropDTO>> GetSucursalCombo(int id, string Estado_Activo);

        Task<string> GetSucursalName(int id);

        Task<bool> DeleteSucursalLogica(int id);
    }
}
