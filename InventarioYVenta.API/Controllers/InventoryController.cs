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
    public class InventoryController : ControllerBase
    {
        private readonly InventarioYVentaDbContext _context;

        public InventoryController(InventarioYVentaDbContext context)
        {
            _context = context;
        }

        //Obtener lista del inventario
        [HttpGet]
        public async Task<ActionResult<Inventory>> GetProducts()
        {
            try
            {
                if(_context.Inventories == null)
                {
                    return NotFound();
                }

                var productsList = await _context.Inventories.Where(productBd => productBd.Status == 1).ToListAsync();

                return Ok(productsList);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        //Obtener un producto de inventario
        [HttpGet("{id}")]
        public async Task<ActionResult<Inventory>> GetProduct(int? id)
        {
            try
            {
                if(id == null)
                {
                    return NotFound();
                }

                var productBD = await _context.Inventories.FirstOrDefaultAsync(productBd => productBd.InventoryId.Equals(id));

                if(productBD == null)
                {
                    return NotFound();
                }

                return Ok(productBD);

            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        //añadir producto a inventario
        [HttpPost("{id}")]
        public async Task<ActionResult> AddProduct(InventoryVM InventoryVM)
        {
            try
            {
                if(InventoryVM == null)
                {
                    return NotFound();
                }

                var inventoryBD = await _context.Inventories.FirstOrDefaultAsync(inventoryBb => inventoryBb.Name.Equals(InventoryVM.Name));

                if(inventoryBD != null)
                {
                    inventoryBD.Name = InventoryVM.Name;
                    inventoryBD.Amount = InventoryVM.Amount;
                    inventoryBD.UnitPurchasePrice = InventoryVM.UnitPurchasePrice;
                    inventoryBD.UnitSalesPrice = InventoryVM.UnitSalesPrice;
                    inventoryBD.UpdatedAt= DateTime.Now;

                    _context.Update(inventoryBD);
                    await _context.SaveChangesAsync();

                    return Ok(new { message = "Producto en inventario actualizado." });
                }

                Inventory NewInventory = new Inventory()
                {
                    Name = InventoryVM.Name,
                    Amount = InventoryVM.Amount,
                    UnitPurchasePrice = InventoryVM.UnitPurchasePrice,
                    UnitSalesPrice = InventoryVM.UnitSalesPrice,
                    Status = 1,
                    CreatedAt = DateTime.Now,
                };

                _context.Add(NewInventory);
                await _context.SaveChangesAsync();

                return Ok(new {message = "Producto en inventario creado."});

            }
            catch(Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        //actualizar producto en inventario
        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(int? id, InventoryVM InventoryVM)
        {
            try
            {
                if(id != InventoryVM.InventoryId)
                {
                    return BadRequest();
                }

                var inventoryBD = await _context.Inventories.FirstOrDefaultAsync(inventoryBb => inventoryBb.InventoryId.Equals(id));

                if (inventoryBD == null)
                {
                    return NotFound();
                }

                inventoryBD.Name = InventoryVM.Name;
                inventoryBD.Amount = InventoryVM.Amount;
                inventoryBD.UnitPurchasePrice = InventoryVM.UnitPurchasePrice;
                inventoryBD.UnitSalesPrice = InventoryVM.UnitSalesPrice;
                inventoryBD.UpdatedAt = DateTime.Now;

                _context.Update(inventoryBD);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Producto en inventario actualizado." });
            }
            catch(Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        //Mandar al historial el producto del inventario
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int? id)
        {
            try
            {
                var inventoryBD = await _context.Inventories.FirstOrDefaultAsync(inventoryBd => inventoryBd.InventoryId.Equals(id));

                if (inventoryBD == null)
                {
                    return NotFound();
                }

                _context.Inventories.Remove(inventoryBD);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }

    }
}
