using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectWeb.API.Servicios;
using ProjectWeb.Shared.Modelo.DTO.Empresa;

namespace ProjectWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresasController : ControllerBase
    {
        private readonly IEmpresas _empresa;
        private readonly IMapper _mapper;

        public EmpresasController(IEmpresas empresa, IMapper mapper)
        {
            _empresa = empresa;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetEmpresaAll()
        {
            var lista = await _empresa.GetListaAllEmpresas();
            return Ok(lista);
        }

        /******************************************************************************/

        [HttpGet("GetEmpresa/{id:int}", Name = "GetEmpresa")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetEmpresa(int id)
        {
            var lista = await _empresa.GetEmpresa(id);
            return Ok(lista);
        }

        [HttpGet("GetEmpresaAllDate/{id:int}", Name = "GetEmpresaAllDate")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetEmpresaAllDate(int id)
        {
            var lista = await _empresa.GetEmpresaAllDate(id);
            return Ok(lista);
        }

        /*--------------------------------- Insert ---------------------------------*/

        [HttpPost("CreateEmpresa")]
        [ProducesResponseType(201, Type = typeof(EmpresaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEmpresa([FromBody] EmpresaDTO RegistroDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (RegistroDTO == null)
            {
                return BadRequest(ModelState);
            }
            var createReg = await _empresa.CreateEmpresa(RegistroDTO);
            return Ok(createReg);
        }

        /*----------------------------------- Delete ------------------------------*/


        [HttpPut("{id:int}", Name = "UpdateEmpresa")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmpresaDTO))]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateEmpresa(int id, [FromBody] EmpresaDTO RegistroDTO)
        {
            if (id != RegistroDTO.Id_empresa) return BadRequest("Id no coincide");

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
                var Updated = await _empresa.UpdateEmpresa(RegistroDTO);

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


        [HttpGet("EmpresasActivo/{Estado}", Name = "EmpresasActivo")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> EmpresaDefault(string Estado)
        {
            var lista = await _empresa.GetListEmpresaActivo(Estado);
            return Ok(lista);
        }

        [HttpGet("ComboEmpresa/{Estado}", Name = "EmpresaCombo")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> EmpresaCombo(string Estado)
        {
            var lista = await _empresa.GetEmpresaCombo(Estado);
            return Ok(lista);
        }


        [HttpDelete("{id_empresa:int}", Name = "CancelEmpresa")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelEmpresa(int id_empresa)
        {
            var Registro = await _empresa.DeleteEmpresaLogica(id_empresa);
            return Ok(Registro);
        }
    }
}
