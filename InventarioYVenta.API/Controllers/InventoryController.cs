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
                    return NotFound(new { message = "El id es nulo." });
                }

                var productBD = await _context.Inventories.FirstOrDefaultAsync(productBd => productBd.InventoryId.Equals(id));

                if(productBD == null)
                {
                    return NotFound(new { message = "El producto en el inventario no encontrado." });
                }

                return Ok(productBD);

            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        //añadir producto a inventario
        [HttpPost]
        public async Task<ActionResult> AddProduct(InventoryVM InventoryVM)
        {
            try
            {
                if(InventoryVM == null)
                {
                    return NotFound(new { message = "Datos no enviados." });
                }

                var inventoryBD = await _context.Inventories.FirstOrDefaultAsync(inventoryBb => inventoryBb.Name.Equals(InventoryVM.Name));

                if(inventoryBD != null)
                {
                    inventoryBD.Name = InventoryVM.Name;
                    inventoryBD.Amount += InventoryVM.Amount;
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
                    return BadRequest(new { message = "El id no coincide con el modelo." });
                }

                var inventoryBD = await _context.Inventories.FirstOrDefaultAsync(inventoryBb => inventoryBb.InventoryId.Equals(id));

                if (inventoryBD == null)
                {
                    return NotFound(new { message = "Producto no encontrado en el inventario." });
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
        [HttpPut("{id}")]
        public async Task<ActionResult> PutStatus(int? id, InventoryVM InventoryVM)
        {
            try
            {
                if (id != InventoryVM.InventoryId)
                {
                    return BadRequest(new { message = "El id no coincide con el modelo." });
                }

                var inventoryBD = await _context.Inventories.FirstOrDefaultAsync(inventoryBb => inventoryBb.InventoryId.Equals(id));

                if (inventoryBD == null)
                {
                    return NotFound(new { message = "Producto no encontrado en el inventario." });
                }

                inventoryBD.Status = 0;
                inventoryBD.DeletedAt = DateTime.Now;

                _context.Update(inventoryBD);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Producto en inventario enviado al historial." });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        //eliminar producto del inventario
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
