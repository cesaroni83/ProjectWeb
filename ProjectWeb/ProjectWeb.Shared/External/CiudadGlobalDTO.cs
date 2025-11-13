using Newtonsoft.Json;

namespace ProjectWeb.Shared.External
{
    public class CiudadGlobalDTO
    {
        public int Id_ciudad { get; set; }
        public int Id_provincia { get; set; }

        [JsonProperty("name")]
        public string Nombre_ciudad { get; set; } = string.Empty;
        public string Informacion_ciudad { get; set; } = "";
        public string Estado_ciudad { get; set; } = "A";
       
    }
}
