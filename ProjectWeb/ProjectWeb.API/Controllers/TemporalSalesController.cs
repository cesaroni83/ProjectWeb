using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.API.Data;
using ProjectWeb.Shared.Modelo.DTO.TemporalSale;
using ProjectWeb.Shared.Modelo.Entidades;

namespace ProjectWeb.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    
    public class TemporalSalesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TemporalSalesController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        
        public async Task<ActionResult> Post(TemporalSaleDTO temporalSaleDTO)
        {
            var product = await _context.Tbl_Producto.FirstOrDefaultAsync(x => x.Id_producto == temporalSaleDTO.ProductId);
            if (product == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);
            if (user == null)
            {
                return NotFound();
            }

            var temporalSale = new TemporalSale
            {
                Product = product,
                Quantity = temporalSaleDTO.Quantity,
                Remarks = temporalSaleDTO.Remarks,
                User = user
            };

            try
            {
                _context.Add(temporalSale);
                await _context.SaveChangesAsync();
                return Ok(temporalSaleDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.Tbl_TemporalSales
                .Include(ts => ts.User!)
                .Include(ts => ts.Product!)
                .ThenInclude(pc => pc.Categorias)
                .Include(ts => ts.Product!)
                .ThenInclude(p => p.ProductImages)
                .Where(x => x.User!.Email == User.Identity!.Name)
                .ToListAsync());
        }

        [AllowAnonymous]
        [HttpGet("count")]
        public async Task<ActionResult> GetCount()
        {
            //return Ok(await _context.Tbl_TemporalSales
            //    .Where(x => x.User!.Email == User.Identity!.Name)
            //    .SumAsync(x => x.Quantity));
            // Obtener el UserId basado en el email del usuario autenticado
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Ahora filtrar las ventas temporales por UserId
            var totalQuantity = await _context.Tbl_TemporalSales
                .Where(x => x.UserId == user.Id)  // Aquí usamos UserId en lugar de User.Email
                .SumAsync(x => x.Quantity);

            return Ok(totalQuantity);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            return Ok(await _context.Tbl_TemporalSales
                .Include(ts => ts.User!)
                .Include(ts => ts.Product!)
                .ThenInclude(pc => pc.Categorias)
                .Include(ts => ts.Product!)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(x => x.Id == id));
        }

        [AllowAnonymous]
        [HttpPut]
        
        public async Task<ActionResult> Put(TemporalSaleDTO temporalSaleDTO)
        {
            var currentTemporalSale = await _context.Tbl_TemporalSales.FirstOrDefaultAsync(x => x.Id == temporalSaleDTO.Id);
            if (currentTemporalSale == null)
            {
                return NotFound();
            }

            currentTemporalSale!.Remarks = temporalSaleDTO.Remarks;
            currentTemporalSale.Quantity = temporalSaleDTO.Quantity;

            _context.Update(currentTemporalSale);
            await _context.SaveChangesAsync();
            return Ok(temporalSaleDTO);
        }

        [AllowAnonymous]
        [HttpDelete("{id:int}")]
       
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var temporalSale = await _context.Tbl_TemporalSales.FirstOrDefaultAsync(x => x.Id == id);
            if (temporalSale == null)
            {
                return NotFound();
            }

            _context.Remove(temporalSale);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
