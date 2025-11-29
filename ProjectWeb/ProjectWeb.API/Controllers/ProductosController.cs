using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.API.Data;
using ProjectWeb.API.Helper;
using ProjectWeb.API.Servicios;
using ProjectWeb.Shared.Modelo.DTO.Producto;
using ProjectWeb.Shared.Paginacion;

namespace ProjectWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductos _prod;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        public ProductosController(IProductos prod, IMapper mapper, AppDbContext context)
        {
            _prod = prod;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("AllProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetProductoAll()
        {
            var lista = await _prod.GetListaAllProductos();
            return Ok(lista);
        }

        /******************************************************************************/

        [HttpGet("GetProducto/{id:int}", Name = "GetProducto")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetProducto(int id)
        {
            var lista = await _prod.GetProducto(id);
            return Ok(lista);
        }


        [HttpGet("GetProductoWithImagen/{id:int}", Name = "GetProductoWithImagen")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetProductoWithImagen(int id)
        {
            var lista = await _prod.GetProductoWithImg(id);
            return Ok(lista);
        }
        /*--------------------------------- Insert ---------------------------------*/

        [HttpPost("CreateProducto")]
        [ProducesResponseType(201, Type = typeof(ProductoDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProducto([FromBody] ProductoDTO RegistroDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (RegistroDTO == null)
            {
                return BadRequest(ModelState);
            }
            var createReg = await _prod.CreateProducto(RegistroDTO);
            return Ok(createReg);
        }

        /*----------------------------------- Delete ------------------------------*/


        [HttpPut("{id:int}", Name = "UpdateProducto")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductoDTO))]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateProducto(int id, [FromBody] ProductoDTO RegistroDTO)
        {
            if (id != RegistroDTO.Id_producto) return BadRequest("Id no coincide");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (RegistroDTO == null)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var Updated = await _prod.UpdateProducto(RegistroDTO);

                if (!Updated)
                    return NotFound("No se encontró el registro a actualizar");

                return Ok(RegistroDTO);
            }
            catch (Exception ex)
            {
                // Puedes loggear el error aquí
                return StatusCode(500, $"Ocurrió un error al actualizar: {ex.Message}");
            }
        }


        [HttpGet("ProductoActivo/{Estado}", Name = "ProductoActivo")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ProductoDefault(string Estado)
        {
            var lista = await _prod.GetListProductoActivo(Estado);
            return Ok(lista);
        }

        [HttpDelete("{id_prod:int}", Name = "CancelProducto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelProducto(int id_prod)
        {
            var Registro = await _prod.DeleteProductoLogica(id_prod);
            return Ok(Registro);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Tbl_Producto
                .Include(x => x.Categorias)
                .Include(x => x.ProductImages)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return Ok(await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync());
        }

        [HttpGet("totalPages")]
        [AllowAnonymous]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Tbl_Producto
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }

        [HttpGet("ProductoByCategoria/{id_cat:int}", Name = "ProductoByCategoria")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ProvinciaByPais(int id_Cat)
        {
            var lista = await _prod.GetProductoByCategoria(id_Cat);
            return Ok(lista);
        }

        [HttpGet("ComboProducto/{Estado}", Name = "ComboProducto")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ProductoCombo( string Estado)
        {
            var lista = await _prod.GetProductoCombo(Estado);
            return Ok(lista);
        }


    }
}
