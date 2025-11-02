using Microsoft.AspNetCore.Mvc;
using ProjectWeb.API.Servicios;
using ProjectWeb.Shared.SolidworkClass;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Runtime.InteropServices;

namespace ProjectWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolidWorksController : ControllerBase
    {
        private readonly ISolidworks _solid;

        

        public SolidWorksController(ISolidworks solid)
        {
           _solid = solid;
        }

        [HttpPost("AutomationSolidWorks/CreateAssembly")]
        public IActionResult ProcesarSolidWorks([FromBody] SolidworksFormModel formulario)
        {


            if (formulario == null)
                return BadRequest("Formulario no puede ser nulo.");
           var process = _solid.CreateAssembly(formulario).Result;
            if (process == "OK")
                return NoContent();   // ✅ Devuelve IActionResult correcto
            else
                return BadRequest(process); // ✅ Devuelve mensaje de error

        }

        //public bool AggiornaAssieme(List<(string, string)> datosTxt, string rutaFile)
        //{
        //    AssemblyDoc assieme = null;
        //    string idMaterial = "";

        //    try
        //    {
        //        if (_swApp == null)
        //        {
        //            try
        //            {
        //                // Iniciar SolidWorks
        //                _swApp = ObtenerSolidWorks();
        //                int errores = 0, advertencias = 0;
        //                var model = _swApp.OpenDoc6(
        //                    rutaFile,
        //                    DeterminarTipoDoc(rutaFile),
        //                    (int)swOpenDocOptions_e.swOpenDocOptions_Silent,
        //                    "",
        //                    ref errores,
        //                    ref advertencias
        //                ) as ModelDoc2;
        //                if (model == null)
        //                {
        //                    throw new Exception($"No se pudo abrir el archivo: {rutaFile}, errores={errores}");
        //                }
        //                // Obtener solo el nombre del archivo (con extensión)
        //                ModelDoc2 swModel = model;

        //                if (swModel != null && swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
        //                {
        //                    foreach (var datos in datosTxt)
        //                    {
        //                        string paramName = datos.Item1.Trim();
        //                        string paramValue = datos.Item2.Trim();

        //                        if (paramName.Equals("MATERIAL", StringComparison.OrdinalIgnoreCase))
        //                        {
        //                            idMaterial = paramValue;
        //                        }
        //                        else
        //                        {
        //                            EquationMgr swEquations = swModel.GetEquationMgr();
        //                            int ind = GetIndex_Global_Variable(swModel, paramName);

        //                            double value = Convert.ToDouble(paramValue);
        //                            string eq = $"\"{paramName}\"= {value}";
        //                            swEquations.Equation[ind] = eq;
        //                        }
        //                    }

        //                    swModel.ForceRebuild3(true);
        //                    swModel.Save2(true);

        //                    //AggiornaConfigurazion(idMaterial, appSolid, rutaFile);

        //                    //appSolid.RunMacro(pathMacro + "Sist_Disegno.swp", "Sist_Disegno1", "main");

        //                    //ExportFile(appSolid);

        //                    return true; // ✅ Todo correcto
        //                }

        //                return false; // No es un ensamblaje
        //            }
        //            catch (Exception ex)
        //            {
        //                //MessageBox.Show("Error al abrir o actualizar SolidWorks: " + ex.Message);
        //                //CloseSolidWorks(rutaFile);
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            //MessageBox.Show("SolidWorks ya está iniciado.");
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
               
        //        //CloseSolidWorks(rutaFile);
        //        return false;
        //    }
        //}
        //private int GetIndex_Global_Variable(ModelDoc2 swModel, string variableName)
        //{
        //    EquationMgr swEqMgr = swModel.GetEquationMgr();

        //    // Itera a través de todas las ecuaciones
        //    for (int i = 0; i < swEqMgr.GetCount(); i++)
        //    {
        //        // Verifica si la ecuación es una variable global
        //        if (swEqMgr.GlobalVariable[i])
        //        {
        //            // Obtiene la ecuación y comprueba si el nombre coincide
        //            string eq = swEqMgr.Equation[i];
        //            if (eq.Contains($"\"{variableName}\""))
        //            {
        //                return i; // Retorna el índice
        //            }
        //        }
        //    }

        //    // Si no se encuentra, retorna -1 (en C# no usamos 'Nothing')
        //    return -1;
        //}
        //private SldWorks ObtenerSolidWorks()
        //{
        //    SldWorks app = null;
        //    try
        //    {
        //        GetActiveObject("SldWorks.Application", IntPtr.Zero, out object obj);
        //        app = (SldWorks)obj;
        //    }
        //    catch
        //    {
        //        app = new SldWorks { Visible = true };
        //    }
        //    return app;
        //}

        //// =============================
        //// 📂 ABRIR ARCHIVO
        //// =============================
        //[HttpGet("AbrirFile/{RutaFile}")]
        //public IActionResult AbrirArchivo(string RutaFile)
        //{
        //    if (!System.IO.File.Exists(RutaFile))
        //        return NotFound($"Archivo no encontrado: {RutaFile}");

        //    int errores = 0, advertencias = 0;
        //    CerrarSolidWorks();
        //    _swApp = ObtenerSolidWorks();

        //    var doc = _swApp.OpenDoc6(
        //        RutaFile,
        //        DeterminarTipoDoc(RutaFile),
        //        (int)swOpenDocOptions_e.swOpenDocOptions_Silent,
        //        "",
        //        ref errores,
        //        ref advertencias
        //    );

        //    if (doc == null)
        //        return BadRequest($"No se pudo abrir el archivo {RutaFile}. Error: {errores}");

        //    return Ok($"Archivo abierto correctamente: {RutaFile}");
        //}

        //private int DeterminarTipoDoc(string ruta)
        //{
        //    string ext = Path.GetExtension(ruta).ToLower();
        //    return ext switch
        //    {
        //        ".sldasm" => (int)swDocumentTypes_e.swDocASSEMBLY,
        //        ".sldprt" => (int)swDocumentTypes_e.swDocPART,
        //        ".slddrw" => (int)swDocumentTypes_e.swDocDRAWING,
        //        _ => (int)swDocumentTypes_e.swDocPART
        //    };
        //}

        //private void CerrarSolidWorks()
        //{
        //    try
        //    {
        //        if (_swApp != null)
        //        {
        //            _swApp.CloseAllDocuments(true);
        //            _swApp.ExitApp();
        //            _swApp = null;
        //        }
        //    }
        //    catch { /* Ignorar errores */ }
        //}

    //    [HttpPost("PackAndGo")]
    //    public IActionResult PackAndGoSolidWorks(
    //[FromQuery] string rutaAssieme,
    //[FromQuery] string rutaDestino,
    //[FromQuery] string prefijo,
    //[FromQuery] string subfijo,
    //[FromQuery] bool incluirDrawing,
    //[FromQuery] bool incluirToolbox,
    //[FromQuery] bool incluirResultadosSim)
    //    {
    //        try
    //        {
    //            // Crear estructura de carpetas antes del Pack&Go
    //            //string rutaDestino = CrearDirectoriosSolidWorks();

    //            var resultado = EjecutarPackAndGo(
    //                rutaAssieme,
    //                rutaDestino,
    //                prefijo,
    //                subfijo,
    //                incluirDrawing,
    //                incluirToolbox,
    //                incluirResultadosSim
    //            );

    //            return Ok(resultado);
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest($"❌ Error durante PackAndGo: {ex.Message}");
    //        }
    //    }


    //    private string EjecutarPackAndGo(
    //        string rutaAssieme,
    //        string rutaDestino,
    //        string prefijo,
    //        string subfijo,
    //        bool incluirDrawing,
    //        bool incluirToolbox,
    //        bool incluirResultadosSim)
    //    {
    //        try
    //        {
    //            _swApp = ObtenerSolidWorks();
    //            _swApp.Visible = true;

    //            int errores = 0, advertencias = 0;
    //            var model = _swApp.OpenDoc6(
    //                rutaAssieme,
    //                DeterminarTipoDoc(rutaAssieme),
    //                (int)swOpenDocOptions_e.swOpenDocOptions_Silent,
    //                "",
    //                ref errores,
    //                ref advertencias
    //            );

    //            if (model == null)
    //                throw new Exception($"No se pudo abrir el archivo. Código de error: {errores}");

    //            var swModel = (ModelDoc2)_swApp.ActiveDoc;
    //            if (swModel == null)
    //                throw new Exception("No hay un documento activo en SolidWorks.");

    //            if (swModel.GetType() != (int)swDocumentTypes_e.swDocASSEMBLY)
    //                throw new Exception("El archivo no es un ensamblaje (SldAsm).");

    //            if (!Directory.Exists(rutaDestino))
    //                Directory.CreateDirectory(rutaDestino);

    //            ModelDocExtension swExt = swModel.Extension;
    //            PackAndGo swPackAndGo = (PackAndGo)swExt.GetPackAndGo();

    //            swPackAndGo.IncludeDrawings = incluirDrawing;
    //            swPackAndGo.IncludeToolboxComponents = incluirToolbox;
    //            swPackAndGo.IncludeSimulationResults = incluirResultadosSim;
    //            swPackAndGo.IncludeSuppressed = false;
    //            swPackAndGo.AddPrefix = prefijo;
    //            swPackAndGo.AddSuffix = subfijo;

    //            swPackAndGo.SetSaveToName(true, rutaDestino);
    //            swExt.SavePackAndGo(swPackAndGo);

    //            _swApp.CloseDoc(Path.GetFileName(rutaAssieme));

    //            return $"✅ Pack and Go completado correctamente en: {rutaDestino}";
    //        }
    //        catch (Exception ex)
    //        {
    //            return $"❌ Error durante Pack and Go: {ex.Message}";
    //        }
    //        finally
    //        {
    //            CerrarSolidWorks(rutaAssieme);
    //        }
    //    }

    //    private void CerrarSolidWorks(string ruta)
    //    {
    //        try
    //        {
    //            if (_swApp != null)
    //            {
    //                _swApp.CloseDoc(Path.GetFileName(ruta));
    //                _swApp.ExitApp();
    //                _swApp = null;
    //                //CerrarProceso();
    //            }
    //        }
    //        catch { /* Ignorar errores */ }
    //    }

        //// =============================
        //// 📁 MÉTODO INTERNO: CREAR ESTRUCTURA DE CARPETAS
        //// =============================
        //private string CrearDirectoriosSolidWorks(SolidworksFormModel formulario)
        //{
        //    if (formulario == null)
        //        throw new ArgumentNullException(nameof(formulario), "El formulario no puede ser nulo.");

        //    if (string.IsNullOrWhiteSpace(formulario.NumeroOrden))
        //        throw new ArgumentException("El número de commessa (NumeroOrden) no puede estar vacío.");

        //    if (string.IsNullOrWhiteSpace(formulario.NumeroMaquina))
        //        throw new ArgumentException("El número de máquina (NumeroMaquina) no puede estar vacío.");

        //    // Validar y crear wwwRoot si no existe
        //    if (!Directory.Exists(wwwRoot))
        //    {
        //        Console.WriteLine($"⚠️ wwwRoot no existía, se creará: {wwwRoot}");
        //        Directory.CreateDirectory(wwwRoot);
        //    }

        //    // Validar y crear carpeta principal SolidWorks
        //    string rootSolidWorks = Path.Combine(wwwRoot, "SolidWorks");
        //    if (!Directory.Exists(rootSolidWorks))
        //    {
        //        Console.WriteLine($"⚠️ Carpeta principal no existía, se creará: {rootSolidWorks}");
        //        Directory.CreateDirectory(rootSolidWorks);
        //    }
        //    else
        //    {
        //        Console.WriteLine($"✅ Carpeta principal ya existe: {rootSolidWorks}");
        //    }

        //    // Crear carpeta Commessa
        //    string commessaFolder = Path.Combine(rootSolidWorks, formulario.NumeroOrden);
        //    if (!Directory.Exists(commessaFolder))
        //    {
        //        Console.WriteLine($"⚠️ Carpeta Commessa no existía, se creará: {commessaFolder}");
        //        Directory.CreateDirectory(commessaFolder);
        //    }
        //    else
        //    {
        //        Console.WriteLine($"✅ Carpeta Commessa ya existe: {commessaFolder}");
        //    }

        //    // Crear carpeta NumeroMaquina
        //    string maquinaFolder = Path.Combine(commessaFolder, formulario.NumeroMaquina);
        //    if (!Directory.Exists(maquinaFolder))
        //    {
        //        Console.WriteLine($"⚠️ Carpeta NumeroMaquina no existía, se creará: {maquinaFolder}");
        //        Directory.CreateDirectory(maquinaFolder);
        //    }
        //    else
        //    {
        //        Console.WriteLine($"✅ Carpeta NumeroMaquina ya existe: {maquinaFolder}");
        //    }

        //    // Crear subcarpetas según ArchivosTxt con nombres únicos
        //    //var nombresUsados = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        //    foreach (var archivo in formulario.ArchivosTxt)
        //    {
        //        if (string.IsNullOrWhiteSpace(archivo.Nombre))
        //        {
        //            Console.WriteLine("⚠️ Se omitió un archivo sin nombre en ArchivosTxt");
        //            continue;
        //        }

        //        // Nombre de subcarpeta: NumeroOrden_NumeroMaquina_NombreArchivo
        //        string nombreSubCarpeta = Path.GetFileNameWithoutExtension(archivo.Nombre);
        //        string subFolder = Path.Combine(maquinaFolder, nombreSubCarpeta);

        //        if (!Directory.Exists(subFolder))
        //        {
        //            Directory.CreateDirectory(subFolder);
        //            Console.WriteLine($"✅ Subcarpeta creada: {subFolder}");
        //        }
        //        else
        //        {
        //            Console.WriteLine($"ℹ️ Subcarpeta ya existía: {subFolder}");
        //        }
        //    }

        //    return maquinaFolder;
        //}

        
    }
}
