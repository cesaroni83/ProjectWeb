namespace ProjectWeb.Shared.Modelo.Estatico
{
    public class Tipo_Documento
    {
        public string Id { get; set; }  // Id como string para mayor flexibilidad
        public string? Nombre { get; set; }

        // Método para obtener la lista de tipos de documento
        public static List<Tipo_Documento> GetItems() => new()
        {
            new Tipo_Documento { Id = "1", Nombre = "DNI" },
            new Tipo_Documento { Id = "2", Nombre = "Pasaporte" },
            new Tipo_Documento { Id = "3", Nombre = "RUC" },
            new Tipo_Documento { Id = "4", Nombre = "Carnet de Extranjería" },
            new Tipo_Documento { Id = "5", Nombre = "Licencia de Conducir" },
            new Tipo_Documento { Id = "6", Nombre = "Otros" }
        };
        public static string GetTextTipoDocumento(string id)
        {
            var estado = GetItems().FirstOrDefault(e => e.Id == id);
            return estado?.Nombre ?? string.Empty; // Retorna vacío si no encuentra
        }
    }
}
