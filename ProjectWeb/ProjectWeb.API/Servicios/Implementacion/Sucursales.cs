using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.API.Data;
using ProjectWeb.API.InterfazGeneral;
using ProjectWeb.Shared.Modelo.DTO.Empresa;
using ProjectWeb.Shared.Modelo.DTO.Sucursal;
using ProjectWeb.Shared.Modelo.Entidades;

namespace ProjectWeb.API.Servicios.Implementacion
{
    public class Sucursales:ISucursales
    {
        public readonly IGenericoModelo<Sucursal> _modeloRepositorio;
        public readonly IMapper _mapper;
        public readonly AppDbContext _context;
        // private object fromDBmodelo;
        public Sucursales(IGenericoModelo<Sucursal> modeloRepositorio, IMapper mapper, AppDbContext context)
        {
            _modeloRepositorio = modeloRepositorio;
            _mapper = mapper;
            _context = context;
        }

        public async Task<SucursalDTO> GetSucursalAllDate(int id)
        {
            var sucursal = await _context.Tbl_Sucursal
                .Include(u => u!.Ciudades)
                .ThenInclude(c => c!.Provincias!)
                .ThenInclude(s => s!.Paises!)
                .Include(p => p!.Personas!)
                .FirstOrDefaultAsync(x => x.Id_sucursal == id);
            if (sucursal is null)
                return null; // o lancia eccezione

            var sucursalDto = _mapper.Map<SucursalDTO>(sucursal);
            return sucursalDto;
        }
        public async Task<SucursalDTO> CreateSucursal(SucursalDTO modelo)
        {
            try
            {
                var dbModelo = _mapper.Map<Sucursal>(modelo);

                var RspModelo = await _modeloRepositorio.CreateReg(dbModelo);
                if (RspModelo.Id_sucursal != 0)
                    return _mapper.Map<SucursalDTO>(RspModelo);
                else
                    throw new TaskCanceledException("Nose puede crear");

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteSucursal(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_sucursal == id);
                var fromDbmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDbmodelo != null)
                {
                    var respuesta = await _modeloRepositorio.Delete(fromDbmodelo);
                    if (!respuesta)
                        throw new TaskCanceledException("No se puedo Eliminar");
                    return respuesta;
                }
                else
                { throw new TaskCanceledException("No nose encontraron datos"); }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteSucursalLogica(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_sucursal == id);
                var fromDbmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDbmodelo != null)
                {
                    fromDbmodelo.Estado_sucursal = "I";
                    var respuesta = await _modeloRepositorio.Upadate(fromDbmodelo);
                    if (!respuesta)
                        throw new TaskCanceledException("No se puedo Eliminar");
                    return respuesta;
                }
                else
                { throw new TaskCanceledException("No nose encontraron datos"); }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<SucursalDTO>> GetListaAllSucursales()
        {

            try
            {
                var consulta = _modeloRepositorio.GetAll().OrderBy(m => m.Id_sucursal);
                List<SucursalDTO> lista = _mapper.Map<List<SucursalDTO>>(await consulta.ToListAsync());
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<List<SucursalDTO>> GetListSucursalActivo(string Estado_Activo)
        {
            try
            {
                ///con referencia
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Estado_sucursal == Estado_Activo);

                var fromDBmodelo = await consulta.ToListAsync();
                if (fromDBmodelo != null && fromDBmodelo.Any())
                {
                    return _mapper.Map<List<SucursalDTO>>(fromDBmodelo);
                }
                else
                { throw new TaskCanceledException("No nose encontraron considencia"); }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<SucursalDTO> GetSucursal(int Id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_sucursal == Id);
                var fromDBmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDBmodelo != null)
                {
                    return _mapper.Map<SucursalDTO>(fromDBmodelo);
                }
                else
                { throw new TaskCanceledException("No nose encontraron considencia"); }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<SucursalDTO>> GetSucursalByEmpresa(int id)
        {
            try
            {
                //var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_empresa == id).OrderBy(m => m.Id_sucursal);
                var consulta = _modeloRepositorio.GetAll()
                .Where(p => p.Id_empresa == id)
                .Include(s => s.Ciudades)
                    .ThenInclude(c => c!.Provincias!)      // si quieres incluir provincias también
                    .ThenInclude(p => p!.Paises!)    // si quieres incluir pais
                .Include(s => s!.Personas!)               // incluir gerente o persona asociada
                .OrderBy(m => m.Id_sucursal);
                List<SucursalDTO> lista = _mapper.Map<List<SucursalDTO>>(await consulta.ToListAsync());
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<SucursalDropDTO>> GetSucursalCombo(int id, string Estado_Activo)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(x => x.Id_empresa == id && x.Estado_sucursal == Estado_Activo).OrderBy(m => m.Id_sucursal);
                List<SucursalDropDTO> lista = _mapper.Map<List<SucursalDropDTO>>(await consulta.ToListAsync());
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<string> GetSucursalName(int Id)
        {
            try
            {
                var consulta = await _modeloRepositorio.GetAllWithWhere(p => p.Id_sucursal == Id).FirstOrDefaultAsync();
                if (consulta != null)
                {
                    return consulta.Nombre_sucursal ?? "";

                }
                else
                { throw new TaskCanceledException("No nose encontraron datos"); }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateSucursal(SucursalDTO modelo)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_sucursal == modelo.Id_sucursal);
                var fromDbmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDbmodelo != null)
                {
                    fromDbmodelo.Id_empresa = modelo.Id_empresa;
                    fromDbmodelo.Nombre_sucursal = modelo.Nombre_sucursal;
                    fromDbmodelo.Id_ciudad = modelo.Id_ciudad;
                    fromDbmodelo.Direccion_sucursal = modelo.Direccion_sucursal;
                    fromDbmodelo.Cap_sucursal = modelo.Cap_sucursal;
                    fromDbmodelo.Telefono = modelo.Telefono!;
                    fromDbmodelo.Telefono_secundario = modelo.Telefono_secundario!;
                    fromDbmodelo.Email = modelo.Email;
                    fromDbmodelo.Id_persona = modelo.Id_persona;
                    fromDbmodelo.Horario_atencion = modelo.Horario_atencion;
                    fromDbmodelo.Informacion_sucursal = modelo.Informacion_sucursal!;
                    fromDbmodelo.Estado_sucursal = modelo.Estado_sucursal;
                    var respuesta = await _modeloRepositorio.Upadate(fromDbmodelo);
                    if (!respuesta)
                        throw new TaskCanceledException("No se puedo Editar");
                    return respuesta;
                }
                else
                { throw new TaskCanceledException("No nose encontraron datos"); }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
