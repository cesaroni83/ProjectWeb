using Newtonsoft.Json;
using ProjectWeb.Shared.Modelo.DTO.Pais;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Shared.External
{
    public class ProvinciaGlobalDTO
    {
        public int Id_provincia { get; set; }
        public int Id_pais { get; set; }

        [JsonProperty("name")]
        public string Nombre_provincia { get; set; } = string.Empty;
        public string Informacion_provincia { get; set; } ="";
        public string Estado_provincia { get; set; } = "A";

        [JsonProperty("iso2")]
        public string iso2 { get; set; } = string.Empty;

        [JsonProperty("cities")]
        public List<CiudadGlobalDTO> Ciudades { get; set; } = new();
    }
}
