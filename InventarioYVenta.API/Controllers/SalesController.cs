using InventarioYVenta.DAL.Context;
using InventarioYVenta.Models.Models;
using InventarioYVenta.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventarioYVenta.API.Controllers
{
    [Route("api/[controller]/[action]")]
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
                    return NotFound(new { message = "Datos no enviados." });
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

                await AmountProductAsync(SaleVM.InventoryId, SaleVM.Amount);
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
                if(id != SaleVM.SaleId || SaleVM == null) return NotFound(new { message = "el id no coincide o datos no enviados, intente otra vez." });

                var saleBD = await _context.Sales.FirstOrDefaultAsync(saleBd => saleBd.SaleId.Equals(id));

                if(saleBD == null) return NotFound(new { message = "Venta no encontrada en base de datos." });

                if (saleBD.Amount != SaleVM.Amount && SaleVM.Amount > saleBD.Amount) await AmountProductAsync(SaleVM.InventoryId, (SaleVM.Amount - saleBD.Amount));
                if(saleBD.Amount != SaleVM.Amount && SaleVM.Amount < saleBD.Amount) await IncreaseAmountProductAsync(SaleVM.InventoryId, (saleBD.Amount - SaleVM.Amount));

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
        [HttpPut("{id}")]
        public async Task<ActionResult> PutStatus(int? id, SaleVM SaleVM)
        {
            try
            {
                if (id != SaleVM.SaleId || SaleVM == null) return NotFound(new { message = "el id no coincide o datos no enviados, intente otra vez." });

                var saleBD = await _context.Sales.FirstOrDefaultAsync(saleBd => saleBd.SaleId.Equals(id));

                if (saleBD == null) return NotFound(new { message = "Venta no encontrada en base de datos." });

                saleBD.Status = 0;
                saleBD.DeletedAt = DateTime.Now;

                _context.Sales.Update(saleBD);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Venta enviada al historial." });

            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        //Eliminar venta
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSale(int? id)
        {
            try
            {
                if(id == null)
                {
                    return NotFound(new { message = "Id no encontrado." });
                }

                var saleBD = await _context.Sales.FirstOrDefaultAsync(saleBd => saleBd.SaleId.Equals(id));

                if(saleBD == null)
                {
                    return NotFound(new { message = "Venta no encontrada." });
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

        //Restar cantidad a producto en inventario
        private async Task AmountProductAsync(int? id, int? amount)
        {
            try
            {
                if(id == null) return;
                if (amount == null) return;

                var productBD = await _context.Inventories.FirstOrDefaultAsync(prodBD => prodBD.InventoryId.Equals(id));

                if (productBD == null) return;
                
                productBD.Amount -= amount;
                productBD.UpdatedAt = DateTime.Now;

                _context.Inventories.Update(productBD);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        //incrementar cantidad a producto en inventario
        private async Task IncreaseAmountProductAsync(int? id, int? amount)
        {
            try
            {
                if (id == null) return;
                if (amount == null) return;

                var productBD = await _context.Inventories.FirstOrDefaultAsync(prodBD => prodBD.InventoryId.Equals(id));

                if (productBD == null) return;

                productBD.Amount += amount;
                productBD.UpdatedAt = DateTime.Now;

                _context.Inventories.Update(productBD);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

    }
}
