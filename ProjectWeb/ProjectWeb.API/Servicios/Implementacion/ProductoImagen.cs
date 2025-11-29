using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.API.Data;
using ProjectWeb.API.Helper;
using ProjectWeb.API.InterfazGeneral;
using ProjectWeb.Shared.Modelo.DTO.ProductImage;
using ProjectWeb.Shared.Modelo.Entidades;

namespace ProjectWeb.API.Servicios.Implementacion
{
    public class ProductoImagen : IProductoImagen
    {
        public readonly IGenericoModelo<ImagenProd> _modeloRepositorio;
        public readonly IMapper _mapper;
        public readonly AppDbContext _context;
        public readonly IFileStorage _fileStorage;
        // private object fromDBmodelo;
        public ProductoImagen(IGenericoModelo<ImagenProd> modeloRepositorio, IMapper mapper, AppDbContext context, IFileStorage fileStorage)
        {
            _modeloRepositorio = modeloRepositorio;
            _mapper = mapper;
            _context = context;
            _fileStorage = fileStorage;
        }


        public async Task<ImagenProdDTO> CreateImagenProd(ImagenProdDTO modelo)
        {
            try
            {
                // 1️⃣ Guardar la imagen usando FileStorage
                if (!string.IsNullOrEmpty(modelo.Foto_Producto))
                {
                    // Guardamos la imagen en la carpeta "Productos/Imagenes"
                    modelo.Foto_Producto = await _fileStorage.SaveImageAsync(modelo.Foto_Producto, "Productos/Imagenes");
                }

                // 2️⃣ Mapear a la entidad
                var dbModelo = _mapper.Map<ImagenProd>(modelo);

                // 3️⃣ Obtener todas las imágenes existentes del producto
                var imagenesExistentes = await _context.Tbl_ProductImages
                    .Where(p => p.Id_producto == dbModelo.Id_producto)
                    .ToListAsync();

                // 4️⃣ Determinar si es la primera imagen o principal
                if (!imagenesExistentes.Any())
                {
                    dbModelo.Tipo_Imagen = "True"; // Primera imagen, principal

                    // Actualizar producto
                    var producto = await _context.Tbl_Producto
                        .FirstOrDefaultAsync(p => p.Id_producto == dbModelo.Id_producto);
                    if (producto != null)
                    {
                        producto.MainImage = dbModelo.Foto_Producto;
                    }
                }
                else
                {
                    if (dbModelo.Tipo_Imagen == "True")
                    {
                        // Si se marca como principal, desmarcar otras imágenes
                        foreach (var img in imagenesExistentes)
                        {
                            img.Tipo_Imagen = "False";
                        }

                        // Actualizar producto
                        var producto = await _context.Tbl_Producto
                            .FirstOrDefaultAsync(p => p.Id_producto == dbModelo.Id_producto);
                        if (producto != null)
                        {
                            producto.MainImage = dbModelo.Foto_Producto;
                        }
                    }
                    else
                    {
                        dbModelo.Tipo_Imagen ??= "False"; // Por si no se pasó
                    }
                }

                // 5️⃣ Guardar la nueva imagen en la base de datos
                var RspModelo = await _modeloRepositorio.CreateReg(dbModelo);

                if (RspModelo.Id_imagen != 0)
                    return _mapper.Map<ImagenProdDTO>(RspModelo);
                else
                    throw new TaskCanceledException("No se pudo crear la imagen");

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
                    // 1️⃣ Verificar si esta imagen es la principal del producto
                    var producto = await _context.Tbl_Producto
                        .FirstOrDefaultAsync(p => p.Id_producto == fromDbmodelo.Id_producto);

                    if (producto != null && producto.MainImage == fromDbmodelo.Foto_Producto)
                    {
                        producto.MainImage = null; // Quitar la referencia de la imagen principal
                    }

                    // 2️⃣ Eliminar la imagen física del servidor
                    if (!string.IsNullOrEmpty(fromDbmodelo.Foto_Producto))
                    {
                        await _fileStorage.DeleteImageAsync(fromDbmodelo.Foto_Producto, "Productos/Imagenes");
                    }

                    // 3️⃣ Eliminar registro de la base de datos
                    var respuesta = await _modeloRepositorio.Delete(fromDbmodelo);
                    if (!respuesta)
                        throw new TaskCanceledException("No se pudo eliminar");

                    // 4️⃣ Guardar cambios en el producto si fue modificado
                    if (producto != null)
                        await _context.SaveChangesAsync();

                    return respuesta;
                }
                else
                {
                    throw new TaskCanceledException("No se encontraron datos");
                }
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
                    .Include(c => c.Productos.Categorias)
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
                // 1️⃣ Cargar la imagen actual
                var fromDbmodelo = await _context.Tbl_ProductImages
                    .FirstOrDefaultAsync(p => p.Id_imagen == modelo.Id_imagen);

                if (fromDbmodelo == null)
                    throw new TaskCanceledException("No se encontraron datos");

                // 2️⃣ Guardar nueva imagen solo si ha cambiado
                if (!string.IsNullOrEmpty(modelo.Foto_Producto) && modelo.Foto_Producto != fromDbmodelo.Foto_Producto)
                {
                    // Eliminar la imagen anterior solo si existe
                    if (!string.IsNullOrEmpty(fromDbmodelo.Foto_Producto))
                    {
                        await _fileStorage.DeleteImageAsync(fromDbmodelo.Foto_Producto, "Productos/Imagenes");
                    }

                    // Guardar la nueva imagen
                    fromDbmodelo.Foto_Producto = await _fileStorage.SaveImageAsync(modelo.Foto_Producto, "Productos/Imagenes");
                }

                // 3️⃣ Si la imagen será la principal, actualizar otras imágenes y producto
                if (modelo.Tipo_Imagen == "True")
                {
                    var otrasImagenes = await _context.Tbl_ProductImages
                        .Where(p => p.Id_producto == modelo.Id_producto &&
                                    p.Id_imagen != modelo.Id_imagen)
                        .ToListAsync();

                    foreach (var img in otrasImagenes)
                    {
                        img.Tipo_Imagen = "False";
                    }

                    var producto = await _context.Tbl_Producto
                        .FirstOrDefaultAsync(p => p.Id_producto == modelo.Id_producto);

                    if (producto != null)
                    {
                        producto.MainImage = fromDbmodelo.Foto_Producto;
                    }
                }

                // 4️⃣ Actualizar otros campos
                fromDbmodelo.Id_producto = modelo.Id_producto;
                fromDbmodelo.Name_imagen = modelo.Name_imagen;
                fromDbmodelo.Descripcion_imagen = modelo.Descripcion_imagen;
                fromDbmodelo.Tipo_Imagen = modelo.Tipo_Imagen;
                fromDbmodelo.Estado_Imagen = modelo.Estado_Imagen;

                // 5️⃣ Guardar cambios
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
            catch (Exception)
            {

                throw;
            }
        }
    }
}
