using ProjectWeb.Shared.Modelo.DTO.Persona;

namespace ProjectWeb.API.Servicios
{
    public interface IPersonas
    {
        Task<List<PersonaDTO>> GetListaAllPersonas();
        Task<PersonaDTO> GetPersona(int id);
        Task<PersonaDTO> GetPersonaByUser(string email);
        Task<PersonaDTO> CreatePersona(PersonaDTO modelo);
        Task<bool> UpdatePersona(PersonaDTO modelo);
        Task<bool> DeletePersona(int id);
        Task<List<PersonaDTO>> GetListPersonaActivo(string Estado_Activo);
        Task<List<PersonaDropDTO>> GetPersonaCombo(string Estado_Activo);
        Task<List<PersonaDropDTO>> GetPersonaType(string Type,string Estado);
        Task<bool> DeletePersonaLogica(int id);
    }
}
