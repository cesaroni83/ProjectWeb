using ProjectWeb.Shared.Modelo.DTO.Empresa;


namespace ProjectWeb.API.Servicios
{
    public interface IEmpresas
    {
        Task<List<EmpresaDTO>> GetListaAllEmpresas();
        Task<EmpresaDTO> GetEmpresa(int id);
        Task<EmpresaDTO> GetEmpresaAllDate(int id);
        Task<EmpresaDTO> CreateEmpresa(EmpresaDTO modelo);
        Task<bool> UpdateEmpresa(EmpresaDTO modelo);
        Task<bool> DeleteEmpresa(int id);
        Task<List<EmpresaDTO>> GetListEmpresaActivo(string Estado_Activo);
        Task<List<EmpresaDropDTO>> GetEmpresaCombo(string Estado_Activo);
        Task<bool> DeleteEmpresaLogica(int id);
    }
}
