using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.API.InterfazGeneral;
using ProjectWeb.Shared.Modelo.DTO.Producto;
using ProjectWeb.Shared.Modelo.Entidades;

namespace ProjectWeb.API.Servicios.Implementacion
{
    public class Productos : IProductos
    {
        public readonly IGenericoModelo<Producto> _modeloRepositorio;
        public readonly IMapper _mapper;
        // private object fromDBmodelo;
        public Productos(IGenericoModelo<Producto> modeloRepositorio, IMapper mapper)
        {
            _modeloRepositorio = modeloRepositorio;
            _mapper = mapper;
        }
        public async Task<ProductoDTO> CreateProducto(ProductoDTO modelo)
        {
            try
            {
                var dbModelo = _mapper.Map<Producto>(modelo);

                var RspModelo = await _modeloRepositorio.CreateReg(dbModelo);
                if (RspModelo.Id_producto != 0)
                    return _mapper.Map<ProductoDTO>(RspModelo);
                else
                    throw new TaskCanceledException("Nose puede crear");

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteProducto(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_producto == id);
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

        public async Task<bool> DeleteProductoLogica(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_producto == id);
                var fromDbmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDbmodelo != null)
                {
                    fromDbmodelo.Estado_Producto = "I";
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

        public async Task<List<ProductoDTO>> GetListaAllProductos()
        {

            try
            {
                var consulta = _modeloRepositorio.GetAll()
                    .Include(x => x.Categorias)
                    .OrderBy(m => m.Id_producto);
                List<ProductoDTO> lista = _mapper.Map<List<ProductoDTO>>(await consulta.ToListAsync());
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<List<ProductoDTO>> GetListProductoActivo(string Estado_Activo)
        {
            try
            {
                ///con referencia
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Estado_Producto == Estado_Activo);

                var fromDBmodelo = await consulta.ToListAsync();
                if (fromDBmodelo != null && fromDBmodelo.Any())
                {
                    return _mapper.Map<List<ProductoDTO>>(fromDBmodelo);
                }
                else
                { throw new TaskCanceledException("No nose encontraron considencia"); }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ProductoDTO> GetProducto(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_producto == id);
                var fromDBmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDBmodelo != null)
                {
                    return _mapper.Map<ProductoDTO>(fromDBmodelo);
                }
                else
                { throw new TaskCanceledException("No nose encontraron considencia"); }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<ProductoDropDTO>> GetProductoCombo(string Estado_Activo)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(x => x.Estado_Producto == Estado_Activo).OrderBy(m => m.Name);
                List<ProductoDropDTO> lista = _mapper.Map<List<ProductoDropDTO>>(await consulta.ToListAsync());
                return lista;

            }
            catch (Exception )
            {

                throw ;
            }
        }

        //public async Task<string> GetProductoName(int id_pais)
        //{
        //    try
        //    {
        //        var consulta = await _modeloRepositorio.GetAllWithWhere(p => p.Id_pais == id_pais).FirstOrDefaultAsync();
        //        if (consulta != null)
        //        {
        //            return consulta.Nombre_pais ?? "";

        //        }
        //        else
        //        { throw new TaskCanceledException("No nose encontraron datos"); }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public async Task<bool> UpdateProducto(ProductoDTO modelo)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_producto == modelo.Id_producto);
                var fromDbmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDbmodelo != null)
                {
                    fromDbmodelo.Id_producto = modelo.Id_producto;
                    fromDbmodelo.Id_Categoria = modelo.Id_Categoria;
                    fromDbmodelo.Estado_Producto = modelo.Estado_producto;
                    fromDbmodelo.Description = modelo.Description;
                    fromDbmodelo.Name = modelo.Name;
                    fromDbmodelo.Price = modelo.Price;
                    fromDbmodelo.Stock = modelo.Stock;
                    //fromDbmodelo.MainImage = modelo.MainImage;
                    fromDbmodelo.Stock = modelo.Stock;
                    //agrega mas campos para aggiornar 
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

        public async Task<List<ProductoDTO>> GetProductoByCategoria(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_Categoria == id).OrderBy(m => m.Name);
                List<ProductoDTO> lista = _mapper.Map<List<ProductoDTO>>(await consulta.ToListAsync());
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ProductoDTO> GetProductoWithImg(int id)
        {
            try
            {
                var productoQuery = _modeloRepositorio.GetAllWithWhere(p => p.Id_producto == id)
                    .Include(p => p.Categorias)
                    .Include(p => p.ProductImages); // Incluimos las imágenes del producto
                    
                var producto = await productoQuery.FirstOrDefaultAsync();

                if (producto == null)
                    throw new KeyNotFoundException($"No se encontró producto con Id {id}");

                return _mapper.Map<ProductoDTO>(producto);
            }
            catch (Exception)
            {
                throw; // Mantener el stack trace original
            }

        }
    }
}
