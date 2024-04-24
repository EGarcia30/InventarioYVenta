using InventarioYVenta.API.Context;
using InventarioYVenta.API.Models;
using InventarioYVenta.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventarioYVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly InventarioYVentaDbContext _context;

        public SalesController(InventarioYVentaDbContext context)
        {
            _context = context;
        }

        //Obtener lista de ventas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSales()
        {
            try
            {
                if(_context.Sales == null)
                {
                    return NotFound();
                }

                var salesList = await _context.Sales.Where(saleBD => saleBD.Status == 1).ToListAsync();

                return Ok(salesList);

            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        //Obtener venta
        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> GeSale(int? id)
        {
            try
            {
                if(id == null)
                {
                    return NotFound();
                }

                var saleBD = await _context.Sales.FirstOrDefaultAsync(saleBd => saleBd.SaleId.Equals(id));

                if(saleBD == null)
                { 
                    return NotFound();
                }

                return Ok(saleBD);
            }
            catch(Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        //añadir venta
        [HttpPost]
        public async Task<ActionResult> AddSale(SaleVM SaleVM)
        {
            try
            {
                if(SaleVM == null)
                {
                    return NotFound();
                }

                Sale NewSale = new Sale()
                {
                    InventoryId= SaleVM.InventoryId,
                    Name= SaleVM.Name,
                    Amount=SaleVM.Amount,
                    UnitPurchasePrice=SaleVM.UnitPurchasePrice,
                    UnitSalesPrice=SaleVM.UnitSalesPrice,
                    Status = 1,
                    CreatedAt = DateTime.Now
                };

                _context.Sales.Add(NewSale);
                await _context.SaveChangesAsync();

                return Ok(new {message = "Venta creada"});

            }
            catch(Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        //Actualizar detalles de venta
        [HttpPut("{id}")]
        public async Task<ActionResult> PutSale(int? id, SaleVM SaleVM)
        {
            try
            {
                if(id != SaleVM.SaleId || SaleVM == null)
                {
                    return NotFound();
                }

                var saleBD = await _context.Sales.FirstOrDefaultAsync(saleBd => saleBd.SaleId.Equals(id));

                if(saleBD == null)
                {
                    return NotFound();
                }

                saleBD.InventoryId= SaleVM.InventoryId;
                saleBD.Name = SaleVM.Name;
                saleBD.Amount = SaleVM.Amount;
                saleBD.UnitPurchasePrice = SaleVM.UnitPurchasePrice;
                saleBD.UnitSalesPrice= SaleVM.UnitSalesPrice;
                saleBD.UpdatedAt = DateTime.Now;

                _context.Sales.Update(saleBD);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Venta actualizada." });

            }
            catch(Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        //Mandar al historial la venta
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSale(int? id)
        {
            try
            {
                if(id == null)
                {
                    return NotFound();
                }

                var saleBD = await _context.Sales.FirstOrDefaultAsync(saleBd => saleBd.SaleId.Equals(id));

                if(saleBD == null)
                {
                    return NotFound();
                }

                _context.Sales.Remove(saleBD);
                await _context.SaveChangesAsync();

                return NoContent();

            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

    }
}
