namespace ProjectWeb.Shared.Modelo.Estatico
{
    public class Nivel_Estudio
    {

        public string? Id { get; set; }
        public string? Nombre { get; set; }

        public static List<Nivel_Estudio> GetItems() => new()
        {
            new Nivel_Estudio { Id = "1", Nombre = "Sin Estudios" },
            new Nivel_Estudio { Id = "2", Nombre = "Primaria Completa" },
            new Nivel_Estudio { Id = "3", Nombre = "Secundaria Completa" },
            new Nivel_Estudio { Id = "4", Nombre = "Técnico" },
            new Nivel_Estudio { Id = "5", Nombre = "Bachillerato" },
            new Nivel_Estudio { Id = "6", Nombre = "Pregrado/Universitario" },
            new Nivel_Estudio { Id = "7", Nombre = "Postgrado / Maestría" },
            new Nivel_Estudio { Id = "8", Nombre = "Doctorado" }
        };
        public static string GetTextNivelEstudio(string id)
        {
            var estado = GetItems().FirstOrDefault(e => e.Id == id);
            return estado?.Nombre ?? string.Empty; // Retorna vacío si no encuentra
        }
    }
}
