using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.API.Data;
using ProjectWeb.API.InterfazGeneral;
using ProjectWeb.Shared.Modelo.DTO.Pais;
using ProjectWeb.Shared.Modelo.DTO.Provincia;
using ProjectWeb.Shared.Modelo.Entidades;

namespace ProjectWeb.API.Servicios.Implementacion
{
    public class Provincias : IProvincias
    {
        public readonly IGenericoModelo<Provincia> _modeloRepositorio;
        public readonly IMapper _mapper;
        public readonly AppDbContext _context;
        // private object fromDBmodelo;
        public Provincias(IGenericoModelo<Provincia> modeloRepositorio, IMapper mapper, AppDbContext context)
        {
            _modeloRepositorio = modeloRepositorio;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ProvinciaDTO> CreateProvincia(ProvinciaDTO modelo)
        {
            try
            {
                var dbModelo = _mapper.Map<Provincia>(modelo);

                var RspModelo = await _modeloRepositorio.CreateReg(dbModelo);
                if (RspModelo.Id_provincia != 0)
                    return _mapper.Map<ProvinciaDTO>(RspModelo);
                else
                    throw new TaskCanceledException("Nose puede crear");

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteProvincia(int id_provincia_aux)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_provincia == id_provincia_aux);
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

        public async Task<bool> DeleteProvinciaLogica(int Id_provincia_aux)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_provincia == Id_provincia_aux);
                var fromDbmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDbmodelo != null)
                {
                    fromDbmodelo.Estado_provincia = "I";
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

        public async Task<List<ProvinciaDTO>> GetListaAllProvincia()
        {

            try
            {
                var consulta = _modeloRepositorio.GetAll().OrderBy(m => m.Id_provincia);
                List<ProvinciaDTO> lista = _mapper.Map<List<ProvinciaDTO>>(await consulta.ToListAsync());
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<List<ProvinciaDTO>> GetListProvinciaActivo(string Estado_Activo)
        {
            try
            {
                ///con referencia
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Estado_provincia == Estado_Activo);

                var fromDBmodelo = await consulta.ToListAsync();
                if (fromDBmodelo != null && fromDBmodelo.Any())
                {
                    return _mapper.Map<List<ProvinciaDTO>>(fromDBmodelo);
                }
                else
                { throw new TaskCanceledException("No nose encontraron considencia"); }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

      public async Task<ProvinciaDTO> GetProvincia(int Id_provincia_aux)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_provincia == Id_provincia_aux);
                var fromDBmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDBmodelo != null)
                {
                    return _mapper.Map<ProvinciaDTO>(fromDBmodelo);
                }
                else
                { throw new TaskCanceledException("No nose encontraron considencia"); }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<ProvinciaDTO>> GetProvinciaByPais(int id_pais)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p=> p.Id_pais==id_pais).OrderBy(m => m.Id_provincia);
                List<ProvinciaDTO> lista = _mapper.Map<List<ProvinciaDTO>>(await consulta.ToListAsync());
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<ProvinciaDropDTO>> GetProvinciaCombo(int id_pais, string Estado_Activo)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(x => x.Paises.Id_pais == id_pais && x.Estado_provincia == Estado_Activo).OrderBy(m => m.Nombre_provincia);
                List<ProvinciaDropDTO> lista = _mapper.Map<List<ProvinciaDropDTO>>(await consulta.ToListAsync());
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<string> GetProvinciaName(int Id_provincia_aux)
        {
            try
            {
                var consulta = await _modeloRepositorio.GetAllWithWhere(p => p.Id_provincia == Id_provincia_aux).FirstOrDefaultAsync();
                if (consulta != null)
                {
                    return consulta.Nombre_provincia ?? "";

                }
                else
                { throw new TaskCanceledException("No nose encontraron datos"); }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateProvincia(ProvinciaDTO modelo)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_provincia == modelo.Id_provincia);
                var fromDbmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDbmodelo != null)
                {
                    fromDbmodelo.Id_pais = modelo.Id_pais;
                    fromDbmodelo.Nombre_provincia = modelo.Nombre_provincia;
                    fromDbmodelo.Informacion_provincia = modelo.Informacion_provincia;
                    fromDbmodelo.Estado_provincia = modelo.Estado_provincia;
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
