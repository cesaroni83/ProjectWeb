using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.API.Data;
using ProjectWeb.API.InterfazGeneral;
using ProjectWeb.Shared.Modelo.DTO.Empresa;
using ProjectWeb.Shared.Modelo.Entidades;

namespace ProjectWeb.API.Servicios.Implementacion
{
    public class Empresas:IEmpresas
    {
        public readonly IGenericoModelo<Empresa> _modeloRepositorio;
        public readonly IMapper _mapper;
        public readonly AppDbContext _context;
        // private object fromDBmodelo;
        public Empresas(IGenericoModelo<Empresa> modeloRepositorio, IMapper mapper, AppDbContext context)
        {
            _modeloRepositorio = modeloRepositorio;
            _mapper = mapper;
            _context = context; 
        }

        public async Task<EmpresaDTO> CreateEmpresa(EmpresaDTO modelo)
        {
            try
            {
                var dbModelo = _mapper.Map<Empresa>(modelo);

                var RspModelo = await _modeloRepositorio.CreateReg(dbModelo);
                if (RspModelo.Id_empresa != 0)
                    return _mapper.Map<EmpresaDTO>(RspModelo);
                else
                    throw new TaskCanceledException("Nose puede crear");

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteEmpresa(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_empresa == id);
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

        public async Task<bool> DeleteEmpresaLogica(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_empresa == id);
                var fromDbmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDbmodelo != null)
                {
                    fromDbmodelo.Estado_empresa = "I";
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

        public async Task<List<EmpresaDTO>> GetListaAllEmpresas()
        {

            try
            {
                var consulta = _modeloRepositorio.GetAll()
                    .Include(c => c.Ciudades)
                    .OrderBy(m => m.Id_empresa);
                List<EmpresaDTO> lista = _mapper.Map<List<EmpresaDTO>>(await consulta.ToListAsync());
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<List<EmpresaDTO>> GetListEmpresaActivo(string Estado_Activo)
        {
            try
            {
                ///con referencia
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Estado_empresa == Estado_Activo);

                var fromDBmodelo = await consulta.ToListAsync();
                if (fromDBmodelo != null && fromDBmodelo.Any())
                {
                    return _mapper.Map<List<EmpresaDTO>>(fromDBmodelo);
                }
                else
                { throw new TaskCanceledException("No nose encontraron considencia"); }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<EmpresaDTO> GetEmpresa(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_empresa == id);
                var fromDBmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDBmodelo != null)
                {
                    return _mapper.Map<EmpresaDTO>(fromDBmodelo);
                }
                else
                { throw new TaskCanceledException("No nose encontraron considencia"); }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<EmpresaDropDTO>> GetEmpresaCombo(string Estado_Activo)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(x => x.Estado_empresa == Estado_Activo).OrderBy(m => m.Id_empresa);
                List<EmpresaDropDTO> lista = _mapper.Map<List<EmpresaDropDTO>>(await consulta.ToListAsync());
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<string> GetEmpresaName(int id)
        {
            try
            {
                var consulta = await _modeloRepositorio.GetAllWithWhere(p => p.Id_empresa == id).FirstOrDefaultAsync();
                if (consulta != null)
                {
                    return consulta.Nombre_Empresa ?? "";

                }
                else
                { throw new TaskCanceledException("No nose encontraron datos"); }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateEmpresa(EmpresaDTO modelo)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_empresa == modelo.Id_empresa);
                var fromDbmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDbmodelo != null)
                {
                    fromDbmodelo.Nombre_Empresa = modelo.Nombre_Empresa;
                    fromDbmodelo.Razon_social = modelo.Razon_social;
                    fromDbmodelo.Ruc = modelo.Ruc;
                    fromDbmodelo.Id_ciudad = modelo.Id_ciudad;
                    fromDbmodelo.Direccion = modelo.Direccion;
                    fromDbmodelo.Cap = modelo.Cap;
                    fromDbmodelo.Telefono = modelo.Telefono!;
                    fromDbmodelo.Telefono_secundario = modelo.Telefono_secundario!;
                    fromDbmodelo.Pagina_web = modelo.Pagina_web!;
                    fromDbmodelo.Email = modelo.Email;
                    fromDbmodelo.Tipo_empresa = modelo.Tipo_empresa!;
                    fromDbmodelo.Representante_legal = modelo.Representante_legal!;
                    fromDbmodelo.Capital_social = modelo.Capital_social;
                    fromDbmodelo.Logo = modelo.Logo;
                    fromDbmodelo.Estado_empresa = modelo.Estado_empresa;
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

        public async Task<EmpresaDTO> GetEmpresaAllDate(int id)
        {
            var empresa = await _context.Tbl_Empresa
                .Include(u => u!.Ciudades)
                .ThenInclude(c => c!.Provincias!)
                .ThenInclude(s => s!.Paises!)
                .FirstOrDefaultAsync(x => x.Id_empresa == id);
            if (empresa is null)
                return null; // o lancia eccezione

            var empresaDto = _mapper.Map<EmpresaDTO>(empresa);
            return empresaDto;
        }
    }
}
