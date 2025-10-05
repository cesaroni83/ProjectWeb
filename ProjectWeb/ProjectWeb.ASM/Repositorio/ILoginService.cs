namespace ProjectWeb.ASM.Repositorio
{
    public interface ILoginService
    {
        Task LoginAsync(string token);
        Task LogoutAsync();
    }
}
