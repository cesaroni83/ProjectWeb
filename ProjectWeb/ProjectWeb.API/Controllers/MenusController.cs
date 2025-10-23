using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectWeb.API.Servicios;
using ProjectWeb.Shared.Modelo.DTO.Menu;
using ProjectWeb.Shared.Modelo.DTO.Pais;
using ProjectWeb.Shared.Modelo.Entidades;
using System.ComponentModel;

namespace ProjectWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly IMenus _menu;
        private readonly IMapper _mapper;
        public MenusController(IMenus menu, IMapper mapper)
        {
            _menu = menu;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetMenuAll()
        {
            var lista = await _menu.GetMenuAll();
            return Ok(lista);
        }

        [HttpGet("ComboMenu/{Estado}", Name = "MenuCombo")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ComboMenu(string Estado)
        {
            var lista = await _menu.GetParendMenu(Estado);
            return Ok(lista);
        }


        [HttpGet("MenuName/{id_menu:int}", Name = "MenuName")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> MenuName(int id_menu)
        {
            try
            {
                var nombre = await _menu.Name_Menu(id_menu);
                return Ok(nombre);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id_menu:int}", Name = "GetMenu")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetPais(int id_menu)
        {
            var lista = await _menu.GetMenu(id_menu);
            if (lista == null)
            {
                return NotFound();
            }
            return Ok(lista);
        }

        [HttpPost("CreateMenu")]
        [ProducesResponseType(201, Type = typeof(MenuDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMenu([FromBody] MenuDTO RegistroDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (RegistroDTO == null)
            {
                return BadRequest(ModelState);
            }
            var createReg = await _menu.CreateMenu(RegistroDTO);
            return Ok(createReg);
        }


        [HttpPut("{idmenu:int}", Name = "UpdateMenu")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaisDTO))]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateMenu(int idmenu, [FromBody] MenuDTO RegistroDTO)
        {
            if (idmenu != RegistroDTO.Id_menu) return BadRequest("Id no coincide");

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
                var Updated = await _menu.UpdateMenu(RegistroDTO);

                if (Updated == null)
                    return NotFound("No se encontró el registro a actualizar");

                return Ok(RegistroDTO);
            }
            catch (Exception ex)
            {
                // Puedes loggear el error aquí
                return StatusCode(500, $"Ocurrió un error al actualizar: {ex.Message}");
            }
        }


        //[HttpGet("default/{Default_name}", Name = "MenuDefault")]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult> MenuDefault(string Default_name)
        //{
        //    var lista = await _menu.ListaDefault(Default_name);
        //    return Ok(lista);
        //}

        [HttpDelete("{id_menu:int}", Name = "CancelMenu")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelMenu(int id_menu)
        {
            var Registro = await _menu.DeleteMenuLogica(id_menu);
            return Ok(Registro);
        }

       

    }
}
