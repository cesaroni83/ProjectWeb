namespace ProjectWeb.Shared.Modelo.Estatico
{
    public class Estado_Civil
    {
        public string? Id { get; set; }
        public string? Text { get; set; }

        public static List<Estado_Civil> GetItems() => new()
        {
            new Estado_Civil { Id = "S", Text = "Soltero" },
            new Estado_Civil { Id = "C", Text = "Casado" },
            new Estado_Civil { Id = "D", Text = "Divorsiado" },
            new Estado_Civil { Id = "V", Text = "Viudo" },
        };
        public static string GetTextEstadoCivil(string id)
        {
            var estado = GetItems().FirstOrDefault(e => e.Id == id);
            return estado?.Text ?? string.Empty; // Retorna vacío si no encuentra
        }
    }
}
