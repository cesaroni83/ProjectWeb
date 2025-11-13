using Newtonsoft.Json;
using ProjectWeb.Shared.Modelo.DTO.Provincia;
using System.ComponentModel.DataAnnotations;

namespace ProjectWeb.Shared.External
{
    public class PaisGlobalDTO
    {
       
        public int Id_pais { get; set; }

        [JsonProperty("name")]
        public string Nombre_pais { get; set; } = string.Empty;

        public string Informacion { get; set; } = "";

        public byte[]? Foto_pais { get; set; } = null;

        public string Estado_pais { get; set; } = "A";

        [JsonProperty("iso2")]
        public string iso2 { get; set; }= string.Empty;

        [JsonProperty("states")]
        public List<ProvinciaGlobalDTO> Provincias { get; set; } = new();
    }
}
