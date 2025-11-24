using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectWeb.API.Servicios;
using ProjectWeb.Shared.Modelo.DTO.ProductImage;

namespace ProjectWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagenProductController : ControllerBase
    {
        private readonly IProductoImagen _img;
        private readonly IMapper _mapper;
        public ImagenProductController(IProductoImagen img, IMapper mapper)
        {
            _img = img;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetImagenProdAll()
        {
            var lista = await _img.GetListaAllImagenProd();
            return Ok(lista);
        }

        [HttpGet("ImagenByProducto/{id:int}", Name = "ImagenByProducto")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImagenByProducto(int id)
        {
            var lista = await _img.GetImagenByProducto(id);
            return Ok(lista);
        }

        /******************************************************************************/

        [HttpGet("GetImagenProd/{id:int}", Name = "GetImagenProd")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetImagenProd(int id)
        {
            var lista = await _img.GetImagenProd(id);
            return Ok(lista);
        }

        /*--------------------------------- Insert ---------------------------------*/

        [HttpPost("CreateImagenProd")]
        [ProducesResponseType(201, Type = typeof(ImagenProdDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateImagenProd([FromBody] ImagenProdDTO RegistroDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (RegistroDTO == null)
            {
                return BadRequest(ModelState);
            }
            var createReg = await _img.CreateImagenProd(RegistroDTO);
            return Ok(createReg);
        }

        /*----------------------------------- Delete ------------------------------*/


        [HttpPut("{idpais:int}", Name = "UpdateImagenProd")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImagenProdDTO))]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateImagenProd(int idpais, [FromBody] ImagenProdDTO RegistroDTO)
        {
            if (idpais != RegistroDTO.Id_imagen) return BadRequest("Id no coincide");

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
                var Updated = await _img.UpdateImagenProd(RegistroDTO);

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


        [HttpGet("ImagenProdActivo/{Estado}", Name = "ImagenProdActivo")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImagenProdDefault(string Estado)
        {
            var lista = await _img.GetListImagenProdActivo(Estado);
            return Ok(lista);
        }

        [HttpDelete("{id_img:int}", Name = "CancelImagenProd")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelImagenProd(int id_img)
        {
            var Registro = await _img.DeleteImagenProdLogica(id_img);
            return Ok(Registro);
        }

    }
}
