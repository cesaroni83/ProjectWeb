using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectWeb.API.Servicios;
using ProjectWeb.Shared.Modelo.DTO.Provincia;

namespace ProjectWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinciasController : ControllerBase
    {
        private readonly IProvincias _provincia;
        private readonly IMapper _mapper;
        public ProvinciasController(IProvincias provincia, IMapper mapper)
        {
            _provincia = provincia;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetProvinciaAll()
        {
            var lista = await _provincia.GetListaAllProvincia();
            return Ok(lista);
        }

        /**********************************************************************/

        /*--------------------------------- Insert ---------------------------------*/

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProvinciaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProvincia([FromBody] ProvinciaDTO RegistroDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (RegistroDTO == null)
            {
                return BadRequest(ModelState);
            }
            var createReg = await _provincia.CreateProvincia(RegistroDTO);
            return Ok(createReg);
        }

        /*----------------------------------- Delete ------------------------------*/


        [HttpPut("{id_provincia:int}", Name = "UpdateProvincia")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProvinciaDTO))]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdatePais(int id_provincia, [FromBody] ProvinciaDTO RegistroDTO)
        {
            if (id_provincia != RegistroDTO.Id_provincia) return BadRequest("Id no coincide");

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
                var Updated = await _provincia.UpdateProvincia(RegistroDTO);

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


        [HttpGet("ProvinciasActiva/{Default_name}", Name = "ProvinciaActiva")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> PaisDefault(string Default_name)
        {
            var lista = await _provincia.GetListProvinciaActivo(Default_name);
            return Ok(lista);
        }

        [HttpGet("ComboProvincia/{id_pais:int}/{Estado}", Name = "ProvinciaCombo")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ProvinciaCombo(int id_pais, string Estado)
        {
            var lista = await _provincia.GetProvinciaCombo(id_pais, Estado);
            return Ok(lista);
        }

        [HttpDelete("{id_provincia:int}", Name = "CancelProvincia")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelProvincia(int id_provincia)
        {
            var Registro = await _provincia.DeleteProvinciaLogica(id_provincia);
            return Ok(Registro);
        }
    }
}
