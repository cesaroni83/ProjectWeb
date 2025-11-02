using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Shared.SolidworkClass
{
    public class FileTxt
    {
        public string Nombre { get; set; } = string.Empty;

        public string Comessa { get; set; } = string.Empty;

        public string unidad { get; set; } = string.Empty;

        public string Cartegoria { get; set; } = string.Empty;

        public string Tipo { get; set; } = string.Empty;

        public string Elemento { get; set; } = string.Empty;

        public List<LineaArchivo> Contenido { get; set; } = new List<LineaArchivo>();
    }
}
