using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Shared.SolidworkClass
{
    public class SolidworksFormModel
    {
        // Información general del formulario
        public string PathEnsamblaje { get; set; } = string.Empty;
        public string NumeroOrden { get; set; } = string.Empty;
        public string NumeroMaquina { get; set; } = string.Empty;

        // Lista de archivos seleccionados
        public List<FileTxt> ArchivosTxt { get; set; } = new List<FileTxt>();

        // Lista de nombres de archivos para el textarea
        public string ListaNombresArchivos { get; set; } = string.Empty;

        // Opciones de exportación
        public bool STD { get; set; } = true;
        public bool Excel { get; set; } = false;
        public bool Step { get; set; } = false;
        public bool Dxf { get; set; } = false;
        public bool PDF { get; set; } = false;
        public bool DWG { get; set; } = false;

        // Método para limpiar el formulario
        public void Limpiar()
        {
            PathEnsamblaje = string.Empty;
            NumeroOrden = string.Empty;
            NumeroMaquina = string.Empty;
            ArchivosTxt.Clear();
            ListaNombresArchivos = string.Empty;

            STD = true;
            Excel = Step = Dxf = PDF = DWG = false;
        }
    }
}
