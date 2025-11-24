using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectWeb.API.Servicios;
using ProjectWeb.Shared.Modelo.DTO.Categoria;
using ProjectWeb.Shared.Modelo.Entidades;

namespace ProjectWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategorias _categ;
        private readonly IMapper _mapper;
        public CategoriasController(ICategorias categ, IMapper mapper)
        {
            _categ = categ;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetCategoriaAll()
        {
            var lista = await _categ.GetListaAllCategorias();
            return Ok(lista);
        }

        /******************************************************************************/

        [HttpGet("GetCategoria/{id:int}", Name = "GetCategoria")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetCategoria(int id)
        {
            var lista = await _categ.GetCategoria(id);
            return Ok(lista);
        }

        /*--------------------------------- Insert ---------------------------------*/

        [HttpPost("CreateCategoria")]
        [ProducesResponseType(201, Type = typeof(CategoriaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCategoria([FromBody] CategoriaDTO RegistroDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (RegistroDTO == null)
            {
                return BadRequest(ModelState);
            }
            var createReg = await _categ.CreateCategoria(RegistroDTO);
            return Ok(createReg);
        }

        /*----------------------------------- Delete ------------------------------*/


        [HttpPut("{id:int}", Name = "UpdateCategoria")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoriaDTO))]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateCategoria(int id, [FromBody] CategoriaDTO RegistroDTO)
        {
            if (id != RegistroDTO.Id_Categoria) return BadRequest("Id no coincide");

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
                var Updated = await _categ.UpdateCategoria(RegistroDTO);

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


        [HttpGet("CategoriaActivo/{Estado}", Name = "CategoriaActivo")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> CategoriaDefault(string Estado)
        {
            var lista = await _categ.GetListCategoriaActivo(Estado);
            return Ok(lista);
        }

        [HttpGet("ComboCategoria/{Estado}", Name = "CategoriaCombo")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> CategoriaCombo(string Estado)
        {
            var lista = await _categ.GetCategoriaCombo(Estado);
            return Ok(lista);
        }


        [HttpDelete("{id_categ:int}", Name = "CancelCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelCategoria(int id_categ)
        {
            var Registro = await _categ.DeleteCategoriaLogica(id_categ);
            return Ok(Registro);
        }
    }
}
