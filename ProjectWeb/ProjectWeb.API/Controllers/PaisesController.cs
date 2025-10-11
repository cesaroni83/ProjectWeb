using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectWeb.API.Servicios;
using ProjectWeb.Shared.Modelo.DTO.Pais;

namespace ProjectWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaisesController : ControllerBase
    {
        private readonly IPaises _pais;
        private readonly IMapper _mapper;
        public PaisesController(IPaises pais, IMapper mapper)
        {
            _pais = pais;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPaisAll()
        {
            var lista = await _pais.GetListaAllPaises();
            return Ok(lista);
        }

        /******************************************************************************/

        [HttpGet("GetPais/{id:int}", Name = "GetPais")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPais(int id)
        {
            var lista = await _pais.GetPais(id);
            return Ok(lista);
        }

        /*--------------------------------- Insert ---------------------------------*/

        [HttpPost("CreatePais")]
        [ProducesResponseType(201, Type = typeof(PaisDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePais([FromBody] PaisDTO RegistroDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (RegistroDTO == null)
            {
                return BadRequest(ModelState);
            }
            var createReg = await _pais.CreatePais(RegistroDTO);
            return Ok(createReg);
        }

        /*----------------------------------- Delete ------------------------------*/


        [HttpPut("{idpais:int}", Name = "UpdatePais")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaisDTO))]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdatePais(int idpais, [FromBody] PaisDTO RegistroDTO)
        {
            if (idpais != RegistroDTO.Id_pais) return BadRequest("Id no coincide");

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
                var Updated = await _pais.UpdatePais(RegistroDTO);

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


        [HttpGet("PaisesActivo/{Estado}", Name = "PaisActivo")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> PaisDefault(string Estado)
        {
            var lista = await _pais.GetListPaisActivo(Estado);
            return Ok(lista);
        }

        [HttpGet("ComboPais/{Estado}", Name = "PaisCombo")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> PaisCombo(string Estado)
        {
            var lista = await _pais.GetPaisCombo(Estado);
            return Ok(lista);
        }

        [HttpDelete("{id_pais:int}", Name = "CancelPais")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelPais(int id_pais)
        {
            var Registro = await _pais.DeletePaisLogica(id_pais);
            return Ok(Registro);
        }
    }
}
