using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectWeb.API.Servicios;
using ProjectWeb.Shared.Modelo.DTO.Ciudad;

namespace ProjectWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CiudadesController : ControllerBase
    {
        private readonly ICiudades _ciudad;
        private readonly IMapper _mapper;
        public CiudadesController(ICiudades ciudad, IMapper mapper)
        {
            _ciudad = ciudad;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetCiudadAll()
        {
            var lista = await _ciudad.GetListaAllCiudades();
            return Ok(lista);
        }


        [HttpGet("GetCiudad/{id_ciudad:int}", Name = "GetCiudad")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetCiudad(int id_ciudad)
        {
            var lista = await _ciudad.GetCiudad(id_ciudad);
            return Ok(lista);
        }


        [HttpGet("CiudadesByProvincia/{id_provincia:int}", Name = "CiudadesByProvincia")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> CiudadesByProvincia(int id_provincia)
        {
            var lista = await _ciudad.GetCiudadByProvincia(id_provincia);
            return Ok(lista);
        }



        /**********************************************************************/

        //[HttpGet("AllRegisterMix", Name = "AllRegisterMixCiudad")]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult> GetCiudadProvinciaPaisAll()
        //{
        //    //var lista = await _ciudad.GetListCiudadProvinciaPais();
        //    //return Ok(lista);
        //}
        /*--------------------------------- Insert ---------------------------------*/

        [HttpPost("CreateCiudad")]
        [ProducesResponseType(201, Type = typeof(CiudadDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCiudad([FromBody] CiudadDTO RegistroDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (RegistroDTO == null)
            {
                return BadRequest(ModelState);
            }
            var createReg = await _ciudad.CreateCiudad(RegistroDTO);
            return Ok(createReg);
        }

        /*----------------------------------- Delete ------------------------------*/


        [HttpPut("{id_ciudad:int}", Name = "UpdateCiudad")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CiudadDTO))]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateCiudad(int id_ciudad, [FromBody] CiudadDTO RegistroDTO)
        {
            if (id_ciudad != RegistroDTO.Id_ciudad) return BadRequest("Id no coincide");

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
                var Updated = await _ciudad.UpdateCiudad(RegistroDTO);

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


        [HttpGet("CiudadesActive/{Default_name}", Name = "CiudadActive")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> CiudadDefault(string Default_name)
        {
            var lista = await _ciudad.GetListCiudadActivo(Default_name);
            return Ok(lista);
        }

       [HttpGet("ComboCiudades/{id_provincia:int}/{Estado}", Name = "CiudadCombo")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> CiudadCombo(int id_provincia, string Estado)
        {
            var lista = await _ciudad.GetCiudadCombo(id_provincia,Estado);
            return Ok(lista);
        }

        [HttpDelete("{id_ciudad:int}", Name = "CancelCiudad")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelCiudad(int id_ciudad)
        {
            var Registro = await _ciudad.DeleteCiudadLogica(id_ciudad);
            return Ok(Registro);
        }
    }
}
