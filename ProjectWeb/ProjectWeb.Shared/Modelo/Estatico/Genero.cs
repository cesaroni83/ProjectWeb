namespace ProjectWeb.Shared.Modelo.Estatico
{
    public class Genero
    {
        public string? Id { get; set; }
        public string? Text { get; set; }

        public static List<Genero> GetItems() => new()
        {
            new Genero { Id = "M", Text = "Masculino" },
            new Genero { Id = "F", Text = "Feminino" },
            new Genero { Id = "O", Text = "Otro" },
        };
        public static string GetTextGenero(string id)
        {
            var estado = GetItems().FirstOrDefault(e => e.Id == id);
            return estado?.Text ?? string.Empty; // Retorna vacío si no encuentra
        }
    }
}
