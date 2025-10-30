using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.API.Data;
using ProjectWeb.API.InterfazGeneral;
using ProjectWeb.Shared.Modelo.DTO.Menu;
using ProjectWeb.Shared.Modelo.DTO.Pais;
using ProjectWeb.Shared.Modelo.Entidades;

namespace ProjectWeb.API.Servicios.Implementacion
{
    public class Menus : IMenus
    {
        public readonly IGenericoModelo<Menu> _modeloRepositorio;
        public readonly IMapper _mapper;
        public readonly AppDbContext _db;

        public Menus(IGenericoModelo<Menu> modeloRepositorio, IMapper mapper, AppDbContext db)
        {
            _modeloRepositorio = modeloRepositorio;
            _mapper = mapper;
            _db = db;
        }

        public async Task<MenuDTO> CreateMenu(MenuDTO modelo)
        {
            try
            {
                var dbModelo = _mapper.Map<Menu>(modelo);

                var RspModelo = await _modeloRepositorio.CreateReg(dbModelo);
                if (RspModelo.Id_menu != 0)
                    return _mapper.Map<MenuDTO>(RspModelo);
                else
                    throw new TaskCanceledException("Nose puede crear");

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteMenu(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_menu == id);
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

        public async Task<List<MenuDTO>> GetMenuAll()
        {
            try
            {
                var consulta = _modeloRepositorio.GetAll().OrderBy(m => m.Id_menu);
                List<MenuDTO> lista = _mapper.Map<List<MenuDTO>>(await consulta.ToListAsync());
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        //public async Task<List<MenuDTO>> ListaDefault(string Default_menu)
        //{
        //    try
        //    {
        //        ///con referencia
        //        //var consulta = _modeloRepositorio.GetAllTable(p => p.Default_menu == Default_menu);
        //        var consulta = _modeloRepositorio.GetAll().OrderBy(m => m.Id_menu);
        //        var fromDBmodelo = await consulta.ToListAsync();
        //        if (fromDBmodelo != null && fromDBmodelo.Any())
        //        {
        //            return _mapper.Map<List<MenuDTO>>(fromDBmodelo);
        //        }
        //        else
        //        { throw new TaskCanceledException("No nose encontraron considencia"); }

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        public async Task<MenuDTO> GetMenu(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_menu == id);
                var fromDBmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDBmodelo != null)
                {
                    return _mapper.Map<MenuDTO>(fromDBmodelo);
                }
                else
                { throw new TaskCanceledException("No nose encontraron considencia"); }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> UpdateMenu(MenuDTO modelo)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_menu == modelo.Id_menu);
                var fromDbmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDbmodelo != null)
                {
                    fromDbmodelo.Descripcion = modelo.Descripcion;
                    fromDbmodelo.Referencia = modelo.Referencia!;
                    fromDbmodelo.Informacion_menu = modelo.Informacion_menu!;
                    fromDbmodelo.Icono_name = modelo.Icono_name;
                    fromDbmodelo.Icono_color = modelo.Icono_color;
                    fromDbmodelo.Id_parend = modelo.Id_parend;
                    fromDbmodelo.Estado_menu = modelo.Estado_menu;
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

        public async Task<List<MenuDropDTO>> GetParendMenu(string Estado)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(x => x.Estado_menu == Estado).OrderBy(m => m.Id_menu);
                List<MenuDropDTO> lista = _mapper.Map<List<MenuDropDTO>>(await consulta.ToListAsync());
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> DeleteMenuLogica(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_menu == id);
                var fromDbmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDbmodelo != null)
                {
                    fromDbmodelo.Estado_menu = "I";
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

        public async Task<string> Name_Menu(int id_menu)
        {
            
            try
            {
                var consulta = await _modeloRepositorio.GetAllWithWhere(p => p.Id_menu == id_menu).FirstOrDefaultAsync();
                if (consulta != null)
                {
                    return consulta.Descripcion ?? "";

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
