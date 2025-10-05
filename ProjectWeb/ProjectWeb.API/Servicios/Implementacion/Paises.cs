using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.API.InterfazGeneral;
using ProjectWeb.Shared.Modelo.DTO.Pais;
using ProjectWeb.Shared.Modelo.Entidades;

namespace ProjectWeb.API.Servicios.Implementacion
{
    public class Paises : IPaises
    {
        public readonly IGenericoModelo<Pais> _modeloRepositorio;
        public readonly IMapper _mapper;
        // private object fromDBmodelo;
        public Paises(IGenericoModelo<Pais> modeloRepositorio, IMapper mapper)
        {
            _modeloRepositorio = modeloRepositorio;
            _mapper = mapper;
        }


        public async Task<PaisDTO> CreatePais(PaisDTO modelo)
        {
            try
            {
                var dbModelo = _mapper.Map<Pais>(modelo);

                var RspModelo = await _modeloRepositorio.CreateReg(dbModelo);
                if (RspModelo.Id_pais != 0)
                    return _mapper.Map<PaisDTO>(RspModelo);
                else
                    throw new TaskCanceledException("Nose puede crear");

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeletePais(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_pais == id);
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

        public async Task<bool> DeletePaisLogica(int id_pais)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_pais == id_pais);
                var fromDbmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDbmodelo != null)
                {
                    fromDbmodelo.Estado_pais = "I";
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

        public async Task<List<PaisDTO>> GetListaAllPaises()
        {

            try
            {
                var consulta = _modeloRepositorio.GetAll()
                    .Include(x => x.Provincias)
                    .OrderBy(m => m.Id_pais);
                List<PaisDTO> lista = _mapper.Map<List<PaisDTO>>(await consulta.ToListAsync());
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<List<PaisDTO>> GetListPaisActivo(string Estado_Activo)
        {
            try
            {
                ///con referencia
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Estado_pais == Estado_Activo);

                var fromDBmodelo = await consulta.ToListAsync();
                if (fromDBmodelo != null && fromDBmodelo.Any())
                {
                    return _mapper.Map<List<PaisDTO>>(fromDBmodelo);
                }
                else
                { throw new TaskCanceledException("No nose encontraron considencia"); }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<PaisDTO> GetPais(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_pais == id);
                var fromDBmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDBmodelo != null)
                {
                    return _mapper.Map<PaisDTO>(fromDBmodelo);
                }
                else
                { throw new TaskCanceledException("No nose encontraron considencia"); }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<PaisDropDTO>> GetPaisCombo(string Estado_Activo)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(x => x.Estado_pais == Estado_Activo).OrderBy(m => m.Id_pais);
                List<PaisDropDTO> lista = _mapper.Map<List<PaisDropDTO>>(await consulta.ToListAsync());
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<string> GetPaisName(int id_pais)
        {
            try
            {
                var consulta = await _modeloRepositorio.GetAllWithWhere(p => p.Id_pais == id_pais).FirstOrDefaultAsync();
                if (consulta != null)
                {
                    return consulta.Nombre_pais ?? "";

                }
                else
                { throw new TaskCanceledException("No nose encontraron datos"); }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdatePais(PaisDTO modelo)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_pais == modelo.Id_pais);
                var fromDbmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDbmodelo != null)
                {
                    fromDbmodelo.Nombre_pais = modelo.Nombre_pais;
                    fromDbmodelo.Informacion = modelo.Informacion;
                    fromDbmodelo.Estado_pais = modelo.Estado_pais;
                    fromDbmodelo.Foto_pais = modelo.Foto_pais;
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
