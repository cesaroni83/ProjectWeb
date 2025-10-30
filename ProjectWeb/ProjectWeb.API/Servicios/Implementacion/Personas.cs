using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.API.InterfazGeneral;
using ProjectWeb.Shared.Modelo.DTO.Persona;
using ProjectWeb.Shared.Modelo.Entidades;

namespace ProjectWeb.API.Servicios.Implementacion
{
    public class Personas : IPersonas
    {
        public readonly IGenericoModelo<Persona> _modeloRepositorio;
        public readonly IMapper _mapper;
        // private object fromDBmodelo;
        public Personas(IGenericoModelo<Persona> modeloRepositorio, IMapper mapper)
        {
            _modeloRepositorio = modeloRepositorio;
            _mapper = mapper;
        }


        public async Task<PersonaDTO> CreatePersona(PersonaDTO modelo)
        {
            try
            {
                var dbModelo = _mapper.Map<Persona>(modelo);

                var RspModelo = await _modeloRepositorio.CreateReg(dbModelo);
                if (RspModelo.Id_persona != 0)
                    return _mapper.Map<PersonaDTO>(RspModelo);
                else
                    throw new TaskCanceledException("Nose puede crear");

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeletePersona(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_persona == id);
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

        public async Task<bool> DeletePersonaLogica(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_persona == id);
                var fromDbmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDbmodelo != null)
                {
                    fromDbmodelo.Estado_persona = "I";
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

        public async Task<List<PersonaDTO>> GetListaAllPersonas()
        {

            try
            {
                var consulta = _modeloRepositorio.GetAll()
                    .Include(x => x.Ciudades)
                    .OrderBy(m => m.Id_persona);
                List<PersonaDTO> lista = _mapper.Map<List<PersonaDTO>>(await consulta.ToListAsync());
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public async Task<List<PersonaDTO>> GetListPersonaActivo(string Estado_Activo)
        {
            try
            {
                ///con referencia
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Estado_persona == Estado_Activo);

                var fromDBmodelo = await consulta.ToListAsync();
                if (fromDBmodelo != null && fromDBmodelo.Any())
                {
                    return _mapper.Map<List<PersonaDTO>>(fromDBmodelo);
                }
                else
                { throw new TaskCanceledException("No nose encontraron considencia"); }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<PersonaDTO> GetPersona(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_persona == id);
                var fromDBmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDBmodelo != null)
                {
                    return _mapper.Map<PersonaDTO>(fromDBmodelo);
                }
                else
                { throw new TaskCanceledException("No nose encontraron considencia"); }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<PersonaDTO> GetPersonaByUser(string email)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Email == email);
                var fromDBmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDBmodelo != null)
                {
                    return _mapper.Map<PersonaDTO>(fromDBmodelo);
                }
                else
                { throw new TaskCanceledException("No nose encontraron considencia"); }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<PersonaDropDTO>> GetPersonaCombo(string Estado_Activo)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(x => x.Estado_persona == Estado_Activo).OrderBy(m => m.Id_persona);
                List<PersonaDropDTO> lista = _mapper.Map<List<PersonaDropDTO>>(await consulta.ToListAsync());
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<string> GetPersonaName(int id)
        {
            try
            {
                var consulta = await _modeloRepositorio.GetAllWithWhere(p => p.Id_persona == id).FirstOrDefaultAsync();
                if (consulta != null)
                {
                    return (consulta.Nombre + consulta.Apellido?? "");

                }
                else
                { throw new TaskCanceledException("No nose encontraron datos"); }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<PersonaDropDTO>> GetPersonaType(string Type,string Estado)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(x => x.Estado_persona == Estado && x.Tipo_persona==Type ).OrderBy(m => m.Id_persona);
                List<PersonaDropDTO> lista = _mapper.Map<List<PersonaDropDTO>>(await consulta.ToListAsync());
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> UpdatePersona(PersonaDTO modelo)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_persona == modelo.Id_persona);
                var fromDbmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDbmodelo != null)
                {
                    fromDbmodelo.Id_user= modelo.Id_user;
                    fromDbmodelo.Nombre = modelo.Nombre;
                    fromDbmodelo.Apellido = modelo.Apellido;
                    fromDbmodelo.Tipo_documento = modelo.Tipo_documento;
                    fromDbmodelo.Numero_documento = modelo.Numero_documento;
                    fromDbmodelo.Date_nacimiento = modelo.Date_nacimiento;
                    fromDbmodelo.Genero = modelo.Genero;
                    fromDbmodelo.Estado_civil = modelo.Estado_civil;
                    fromDbmodelo.Nacionalidad = modelo.Nacionalidad;
                    fromDbmodelo.Ocupacion = modelo.Ocupacion;
                    fromDbmodelo.Nivel_estudio = modelo.Nivel_estudio;
                    fromDbmodelo.Id_ciudad = modelo.Id_ciudad;
                    fromDbmodelo.Cap_persona = modelo.Cap_persona;
                    fromDbmodelo.Direccion_persona = modelo.Direccion_persona;
                    fromDbmodelo.Telefono = modelo.Telefono;
                    fromDbmodelo.Email = modelo.Email;
                    fromDbmodelo.Foto = modelo.Foto;
                    fromDbmodelo.Informacion = modelo.Informacion;
                    fromDbmodelo.Estado_persona = modelo.Estado_persona;
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
