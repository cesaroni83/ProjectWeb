namespace ProjectWeb.Shared.Modelo.Estatico
{
    public class Estado
    {
        public string? Id { get; set; }
        public string? Text { get; set; }

        public static List<Estado> GetItems() => new()
        {
            new Estado { Id = "A", Text = "Activo" },
            new Estado { Id = "I", Text = "Inactivo" },
        };
        public static string GetTextEstado(string id)
        {
            var estado = GetItems().FirstOrDefault(e => e.Id == id);
            return estado?.Text ?? string.Empty; // Retorna vacío si no encuentra
        }
    }
}
