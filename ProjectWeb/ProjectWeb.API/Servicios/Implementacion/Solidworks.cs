using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectWeb.Shared.SolidworkClass;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Runtime.InteropServices;

namespace ProjectWeb.API.Servicios.Implementacion
{
    public class Solidworks : ISolidworks
    {
        private SldWorks _swApp;
        private readonly string wwwRoot;
        private readonly string ruta_template = "C:\\Solidwoks_Datos\\Template_Modelo\\Template\\";

        public Solidworks(IWebHostEnvironment env)
        {
            wwwRoot = env.WebRootPath;
        }


        private static extern void GetActiveObject(
            [MarshalAs(UnmanagedType.BStr)] string progID,
            IntPtr reserved,
            [MarshalAs(UnmanagedType.IUnknown)] out object obj);

        public Task<bool> CerrarSolidWorks()
        {
            try
            {
                if (_swApp != null)
                {
                    _swApp.CloseAllDocuments(true);
                    _swApp.ExitApp();
                    _swApp = null;
                }
                return Task.FromResult(true);
            }
            catch { return Task.FromResult(false); }
        }
        

        public Task<int> DeterminarTipoDoc(string RutaFile)
        {
            string ext = Path.GetExtension(RutaFile).ToLower();
            int tipo=ext switch
            {
                ".sldasm" => (int)swDocumentTypes_e.swDocASSEMBLY,
                ".sldprt" => (int)swDocumentTypes_e.swDocPART,
                ".slddrw" => (int)swDocumentTypes_e.swDocDRAWING,
                _ => (int)swDocumentTypes_e.swDocPART
            };
            return Task.FromResult(tipo);
        }

        public async Task<string> EjecutarPackAndGo(string rutaAssieme, string rutaDestino, string prefijo, string subfijo, bool incluirDrawing, bool incluirToolbox, bool incluirResultadosSim)
        {
            try
            {
                _swApp = ObtenerSolidWorks().Result;
                _swApp.Visible = true;

                int errores = 0, advertencias = 0;
                var model = _swApp.OpenDoc6(
                    rutaAssieme,
                    DeterminarTipoDoc(rutaAssieme).Result,
                    (int)swOpenDocOptions_e.swOpenDocOptions_Silent,
                    "",
                    ref errores,
                    ref advertencias
                );

                if (model == null)
                    throw new Exception($"No se pudo abrir el archivo. Código de error: {errores}");

                var swModel = (ModelDoc2)_swApp.ActiveDoc;
                if (swModel == null)
                    throw new Exception("No hay un documento activo en SolidWorks.");

                if (swModel.GetType() != (int)swDocumentTypes_e.swDocASSEMBLY)
                    throw new Exception("El archivo no es un ensamblaje (SldAsm).");

                if (!Directory.Exists(rutaDestino))
                    Directory.CreateDirectory(rutaDestino);

                ModelDocExtension swExt = swModel.Extension;
                PackAndGo swPackAndGo = (PackAndGo)swExt.GetPackAndGo();

                swPackAndGo.IncludeDrawings = incluirDrawing;
                swPackAndGo.IncludeToolboxComponents = incluirToolbox;
                swPackAndGo.IncludeSimulationResults = incluirResultadosSim;
                swPackAndGo.IncludeSuppressed = false;
                swPackAndGo.AddPrefix = prefijo;
                swPackAndGo.AddSuffix = subfijo;

                swPackAndGo.SetSaveToName(true, rutaDestino);
                swExt.SavePackAndGo(swPackAndGo);

                _swApp.CloseDoc(Path.GetFileName(rutaAssieme));

                return ($"✅ Pack and Go completado correctamente en: {rutaDestino}");
            }
            catch (Exception ex)
            {
                return $"❌ Error durante Pack and Go: {ex.Message}";
            }
            finally
            {
                await CloseDocSolidwoks(rutaAssieme);
            }
        }

        public Task<string> CrearDirectoriosSolidWorks(SolidworksFormModel formulario)
        {
            if (formulario == null)
                throw new ArgumentNullException(nameof(formulario), "El formulario no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(formulario.NumeroOrden))
                throw new ArgumentException("El número de commessa (NumeroOrden) no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(formulario.NumeroMaquina))
                throw new ArgumentException("El número de máquina (NumeroMaquina) no puede estar vacío.");

            // Validar y crear wwwRoot si no existe
            if (!Directory.Exists(wwwRoot))
            {
                Console.WriteLine($"⚠️ wwwRoot no existía, se creará: {wwwRoot}");
                Directory.CreateDirectory(wwwRoot);
            }

            // Validar y crear carpeta principal SolidWorks
            string rootSolidWorks = Path.Combine(wwwRoot, "SolidWorks");
            if (!Directory.Exists(rootSolidWorks))
            {
                Console.WriteLine($"⚠️ Carpeta principal no existía, se creará: {rootSolidWorks}");
                Directory.CreateDirectory(rootSolidWorks);
            }
            else
            {
                Console.WriteLine($"✅ Carpeta principal ya existe: {rootSolidWorks}");
            }

            // Crear carpeta Commessa
            string commessaFolder = Path.Combine(rootSolidWorks, formulario.NumeroOrden);
            if (!Directory.Exists(commessaFolder))
            {
                Console.WriteLine($"⚠️ Carpeta Commessa no existía, se creará: {commessaFolder}");
                Directory.CreateDirectory(commessaFolder);
            }
            else
            {
                Console.WriteLine($"✅ Carpeta Commessa ya existe: {commessaFolder}");
            }

            // Crear carpeta NumeroMaquina
            string maquinaFolder = Path.Combine(commessaFolder, formulario.NumeroMaquina);
            if (!Directory.Exists(maquinaFolder))
            {
                Console.WriteLine($"⚠️ Carpeta NumeroMaquina no existía, se creará: {maquinaFolder}");
                Directory.CreateDirectory(maquinaFolder);
            }
            else
            {
                Console.WriteLine($"✅ Carpeta NumeroMaquina ya existe: {maquinaFolder}");
            }

            // Crear subcarpetas según ArchivosTxt con nombres únicos
            //var nombresUsados = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var archivo in formulario.ArchivosTxt)
            {
                if (string.IsNullOrWhiteSpace(archivo.Nombre))
                {
                    Console.WriteLine("⚠️ Se omitió un archivo sin nombre en ArchivosTxt");
                    continue;
                }

                // Nombre de subcarpeta: NumeroOrden_NumeroMaquina_NombreArchivo
                string nombreSubCarpeta = Path.GetFileNameWithoutExtension(archivo.Nombre);
                string subFolder = Path.Combine(maquinaFolder, nombreSubCarpeta);

                if (!Directory.Exists(subFolder))
                {
                    Directory.CreateDirectory(subFolder);
                    Console.WriteLine($"✅ Subcarpeta creada: {subFolder}");
                }
                else
                {
                    Console.WriteLine($"ℹ️ Subcarpeta ya existía: {subFolder}");
                }
            }

            return Task.FromResult(maquinaFolder);
        }

        public Task<bool> CerrarProceso()
        {
            string processName = "sldworks";
            var processes = System.Diagnostics.Process.GetProcessesByName(processName);

            if (processes.Length > 0)
            {
                foreach (var p in processes)
                {
                    try
                    {
                        p.Kill();
                        Console.WriteLine("SolidWorks se ha cerrado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al cerrar SolidWorks: " + ex.Message);

                    }
                }
            }
            else
            {
                Console.WriteLine("No se encontraron procesos de SolidWorks.");
            }
            return Task.FromResult(true);
        }

        public Task<bool> AggiornaAssieme(List<(string, string)> datosTxt, string rutaFile)
        {
            AssemblyDoc assieme = null;
            string idMaterial = "";

            try
            {
                if (_swApp == null)
                {
                    try
                    {
                        // Iniciar SolidWorks
                        _swApp = ObtenerSolidWorks().Result;
                        int errores = 0, advertencias = 0;
                        var model = _swApp.OpenDoc6(
                            rutaFile,
                            DeterminarTipoDoc(rutaFile).Result,
                            (int)swOpenDocOptions_e.swOpenDocOptions_Silent,
                            "",
                            ref errores,
                            ref advertencias
                        ) as ModelDoc2;
                        if (model == null)
                        {
                            throw new Exception($"No se pudo abrir el archivo: {rutaFile}, errores={errores}");
                        }
                        // Obtener solo el nombre del archivo (con extensión)
                        ModelDoc2 swModel = model;

                        if (swModel != null && swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
                        {
                            foreach (var datos in datosTxt)
                            {
                                string paramName = datos.Item1.Trim();
                                string paramValue = datos.Item2.Trim();

                                if (paramName.Equals("MATERIAL", StringComparison.OrdinalIgnoreCase))
                                {
                                    idMaterial = paramValue;
                                }
                                else
                                {
                                    EquationMgr swEquations = swModel.GetEquationMgr();
                                    int ind = GetIndex_Global_Variable(swModel, paramName).Result;

                                    double value = Convert.ToDouble(paramValue);
                                    string eq = $"\"{paramName}\"= {value}";
                                    swEquations.Equation[ind] = eq;
                                    swModel.EditRebuild3();
                                }
                            }

                            swModel.EditRebuild3();
                            swModel.Save2(true);

                            //AggiornaConfigurazion(idMaterial, appSolid, rutaFile);

                            //appSolid.RunMacro(pathMacro + "Sist_Disegno.swp", "Sist_Disegno1", "main");

                            //ExportFile(appSolid);
                            CloseDocSolidwoks(rutaFile);
                            return Task.FromResult(true); // ✅ Todo correcto
                        }

                        return Task.FromResult(false); // No es un ensamblaje
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Error al abrir o actualizar SolidWorks: " + ex.Message);
                        //CloseSolidWorks(rutaFile);
                        return Task.FromResult(false);
                    }
                }
                else
                {
                    //MessageBox.Show("SolidWorks ya está iniciado.");
                    return Task.FromResult(false);
                }
            }
            catch (Exception ex)
            {

                //CloseSolidWorks(rutaFile);
                return Task.FromResult(false);
            }
        }

        public Task<int> GetIndex_Global_Variable(ModelDoc2 swModel, string variableName)
        {
            EquationMgr swEqMgr = swModel.GetEquationMgr();

            // Limpiar variableName de comillas dobles
            string cleanVariableName = variableName.Trim('\"');

            for (int i = 0; i < swEqMgr.GetCount(); i++)
            {
                if (swEqMgr.GlobalVariable[i])
                {
                    string eq = swEqMgr.Equation[i];  // Ej: "\"DAMPER_H\" = 1000"

                    if (eq.IndexOf($"\"{cleanVariableName}\"", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return Task.FromResult(i);
                    }
                }
            }

            return Task.FromResult(-1);
        }

        public Task<SldWorks> ObtenerSolidWorks()
        {
            if (_swApp == null)
            {
                try
                {
                    _swApp = new SldWorks { Visible = true };
                }
                catch
                {
                    _swApp = null;
                }
            }
            return Task.FromResult(_swApp);
        }

        public Task<bool> CloseDocSolidwoks(string File)
        {
            try
            {
                if (_swApp != null)
                {
                    _swApp.CloseDoc(Path.GetFileName(File));
                    _swApp.ExitApp();
                    _swApp = null;
                    CerrarProceso();
                }
                return Task.FromResult(true);
            }
            catch { return Task.FromResult(false); }
        }

        public Task<string> CreateAssembly(SolidworksFormModel formulario)
        {
            if (formulario == null)
                return Task.FromResult("Formulario no puede ser nulo.");

            try
            {
                // 1️⃣ Crear carpetas necesarias
                 string maquinaFolder = CrearDirectoriosSolidWorks(formulario).Result; // usa el método que ya definimos antes

                // 2️⃣ Recorrer cada archivo y ejecutar Pack & Go
                foreach (var archivo in formulario.ArchivosTxt)
                {
                    if (string.IsNullOrWhiteSpace(archivo.Nombre))
                        continue;

                    string archivo_origen = Path.Combine(
                        ruta_template,
                        archivo.Tipo,
                         archivo.Cartegoria,
                         archivo.Cartegoria + ".sldasm"
                        ); // <- cierre de Path.Combine y punto y coma
                           // Ruta de la carpeta donde quieres guardar

                    // Nombre original del archivo con extensión
                    string nombreArchivo = Path.GetFileName(archivo.Nombre); // ej: 253526_001_101LOU10X0AA.txt

                    // Nombre sin extensión
                    string nombreSinExtension = Path.GetFileNameWithoutExtension(nombreArchivo);

                    // Ruta completa destino SIN extensión
                    string archivoDestino = Path.Combine(maquinaFolder, nombreSinExtension);

                    // Llama al método PackAndGoSolidWorks
                    //  Prefijo y sufijo se pueden definir según tus necesidades, aquí vacíos
                    var resultadoPack = EjecutarPackAndGo(
                        rutaAssieme: archivo_origen,
                        rutaDestino: archivoDestino,
                        prefijo: archivo.Comessa + "_" + archivo.unidad + "_" + archivo.Elemento,
                        subfijo: string.Empty,
                        incluirDrawing: true,
                        incluirToolbox: false,
                        incluirResultadosSim: false
                    );
                    if (resultadoPack != null)
                    {

                        try
                        {
                            var datosTupla = archivo.Contenido
                                .Select(x => (x.Nombre, x.Valor))
                                .ToList();
                            string FileNew = archivoDestino + "\\" + nombreSinExtension + ".sldasm";
                            bool UpdateVarGlobal = AggiornaAssieme(datosTupla, FileNew).Result;
                            if (!UpdateVarGlobal)
                            {
                                return Task.FromResult($"No se pudo actualizar variables del archivo {archivo.Nombre}");
                            }
                        }
                        catch (Exception ex)
                        {
                            return Task.FromResult($"Error al actualizar archivo {archivo.Nombre}: {ex.Message}");
                        }

                    }

                    //Console.WriteLine($"Archivo {archivo.Nombre}: {resultadoPack}");
                }

                return Task.FromResult("OK");
            }
            catch (Exception ex)
            {
                return Task.FromResult($"Error procesando SolidWorks: {ex.Message}");
            }
        }
    }
}
