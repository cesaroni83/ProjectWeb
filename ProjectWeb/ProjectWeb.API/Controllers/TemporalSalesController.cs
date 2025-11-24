using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.API.Data;
using ProjectWeb.Shared.Modelo.DTO.TemporalSale;
using ProjectWeb.Shared.Modelo.Entidades;

namespace ProjectWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Tbl_TemporalSalesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Tbl_TemporalSalesController(AppDbContext context)
        {
            _context = context;
        }

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

        [HttpGet("count")]
        public async Task<ActionResult> GetCount()
        {
            return Ok(await _context.Tbl_TemporalSales
                .Where(x => x.User!.Email == User.Identity!.Name)
                .SumAsync(x => x.Quantity));
        }

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
