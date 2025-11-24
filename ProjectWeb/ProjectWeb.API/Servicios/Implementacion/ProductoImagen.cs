using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.API.Data;
using ProjectWeb.API.InterfazGeneral;
using ProjectWeb.Shared.Modelo.DTO.ProductImage;
using ProjectWeb.Shared.Modelo.DTO.Producto;
using ProjectWeb.Shared.Modelo.Entidades;

namespace ProjectWeb.API.Servicios.Implementacion
{
    public class ProductoImagen: IProductoImagen
    {
        public readonly IGenericoModelo<ImagenProd> _modeloRepositorio;
        public readonly IMapper _mapper;
        public readonly AppDbContext _context;
        // private object fromDBmodelo;
        public ProductoImagen(IGenericoModelo<ImagenProd> modeloRepositorio, IMapper mapper, AppDbContext context)
        {
            _modeloRepositorio = modeloRepositorio;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ImagenProdDTO> CreateImagenProd(ImagenProdDTO modelo)
        {
            try
            {
                var dbModelo = _mapper.Map<ImagenProd>(modelo);

                var RspModelo = await _modeloRepositorio.CreateReg(dbModelo);
                if (RspModelo.Id_imagen != 0)
                    return _mapper.Map<ImagenProdDTO>(RspModelo);
                else
                    throw new TaskCanceledException("Nose puede crear");

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteImagenProd(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_imagen == id);
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

        public async Task<List<ImagenProdDTO>> GetListaAllImagenProd()
        {

            try
            {
                var consulta = _modeloRepositorio.GetAll()
                    .Include(x => x.Productos)
                    .Include(c=> c.Productos.Categorias)
                    .OrderBy(m => m.Id_imagen);
                List<ImagenProdDTO> lista = _mapper.Map<List<ImagenProdDTO>>(await consulta.ToListAsync());
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<List<ImagenProdDTO>> GetListImagenProdActivo(string Estado_Activo)
        {
            try
            {
                ///con referencia
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Estado_Imagen == Estado_Activo);

                var fromDBmodelo = await consulta.ToListAsync();
                if (fromDBmodelo != null && fromDBmodelo.Any())
                {
                    return _mapper.Map<List<ImagenProdDTO>>(fromDBmodelo);
                }
                else
                { throw new TaskCanceledException("No nose encontraron considencia"); }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ImagenProdDTO> GetImagenProd(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_imagen == id);
                var fromDBmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDBmodelo != null)
                {
                    return _mapper.Map<ImagenProdDTO>(fromDBmodelo);
                }
                else
                { throw new TaskCanceledException("No nose encontraron considencia"); }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> UpdateImagenProd(ImagenProdDTO modelo)
        {
            try
            {
                // Cargar la imagen actual (SE MATERIALIZA AQUÍ)
                var fromDbmodelo = await _context.Tbl_ProductImages
                    .FirstOrDefaultAsync(p => p.Id_imagen == modelo.Id_imagen);

                if (fromDbmodelo == null)
                    throw new TaskCanceledException("No se encontraron datos");


                // Si la imagen será la principal
                if (modelo.Tipo_Imagen == "True")
                {
                    // Se materializa la lista aquí ↓↓↓
                    var otrasImagenes = await _context.Tbl_ProductImages
                        .Where(p => p.Id_producto == modelo.Id_producto &&
                                    p.Id_imagen != modelo.Id_imagen)
                        .ToListAsync();

                    // Desmarcar todas las demás imágenes
                    foreach (var img in otrasImagenes)
                    {
                        img.Tipo_Imagen = "False";
                    }

                    // Actualizar producto
                    var producto = await _context.Tbl_Producto
                        .FirstOrDefaultAsync(p => p.Id_producto == modelo.Id_producto);

                    if (producto != null)
                    {
                        producto.MainImage = modelo.Foto_Producto;
                    }
                }


                // Actualizar imagen actual
                fromDbmodelo.Id_producto = modelo.Id_producto;
                fromDbmodelo.Foto_Producto = modelo.Foto_Producto;
                fromDbmodelo.Name_imagen = modelo.Name_imagen;
                fromDbmodelo.Descripcion_imagen = modelo.Descripcion_imagen;
                fromDbmodelo.Tipo_Imagen = modelo.Tipo_Imagen;
                fromDbmodelo.Estado_Imagen = modelo.Estado_Imagen;

                // SE GUARDA TODO AL FINAL (UNA SOLA VEZ)
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                throw;
            }
        }


        public async Task<bool> DeleteImagenProdLogica(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_imagen == id);
                var fromDbmodelo = await consulta.FirstOrDefaultAsync();
                if (fromDbmodelo != null)
                {
                    fromDbmodelo.Estado_Imagen = "I";
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

        public async Task<List<ImagenProdDTO>> GetImagenByProducto(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.GetAllWithWhere(p => p.Id_producto == id).OrderBy(m => m.Id_imagen);
                List<ImagenProdDTO> lista = _mapper.Map<List<ImagenProdDTO>>(await consulta.ToListAsync());
                return lista;

            }
            catch (Exception )
            {

                throw;
            }
        }
    }
}
