using ProjectWeb.Shared.SolidworkClass;
using SolidWorks.Interop.sldworks;

namespace ProjectWeb.API.Servicios
{
    public interface ISolidworks
    {
        Task<bool> CerrarSolidWorks();
        Task<bool> CloseDocSolidwoks(string File);
        Task<int> DeterminarTipoDoc(string RutaFile);

        Task<string> EjecutarPackAndGo(string rutaAssieme,
                                     string rutaDestino,
                                     string prefijo,
                                     string subfijo,
                                     bool incluirDrawing,
                                     bool incluirToolbox,
                                     bool incluirResultadosSim);
        Task<string> CrearDirectoriosSolidWorks(SolidworksFormModel formulario);
        Task<bool> CerrarProceso();
        Task<bool> AggiornaAssieme(List<(string, string)> datosTxt, string rutaFile);
        Task<int> GetIndex_Global_Variable(ModelDoc2 swModel, string variableName);
        Task<SldWorks> ObtenerSolidWorks();

        Task<string> CreateAssembly(SolidworksFormModel formulario);
    }
}
