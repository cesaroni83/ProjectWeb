using ProjectWeb.Shared.Modelo.DTO.Menu;

namespace ProjectWeb.API.Servicios
{
    public interface IMenus
    {
        Task<List<MenuDTO>> GetMenuAll();
        Task<MenuDTO> GetMenu(int id);
        Task<MenuDTO> CreateMenu(MenuDTO modelo);
        Task<bool> UpdateMenu(MenuDTO modelo);
        Task<bool> DeleteMenu(int id);
        Task<bool> DeleteMenuLogica(int id);
        Task<List<MenuDropDTO>> GetParendMenu(string Estado);
        Task<string> Name_Menu(int id_menu);
    }
}
