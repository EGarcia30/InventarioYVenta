using InventarioYVenta.DAL.Context;
using InventarioYVenta.Models.Models;
using InventarioYVenta.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventarioYVenta.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly InventarioYVentaDbContext _context;
        private int validatorVM = 0;
        private string messageVM = string.Empty;
        private int saleIdVM= 0;

        public SalesController(InventarioYVentaDbContext context)
        {
            _context = context;
        }

        //Obtener lista de ventas
        [HttpGet]
        public async Task<ActionResult<List<SaleListVM>>> GetSales()
        {
            try
            {
                if(_context.Sales == null || _context.SaleDetails == null) return NotFound(new { message = "Realizar una venta."});

                var salesList = await _context.SaleListVMs.FromSqlRaw("sp_GetSales").ToListAsync();

                if (salesList == null) return NotFound(new { message = "No se ha realizado ninguna venta." });

                return Ok(salesList);

            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        //Obtener venta
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleListVM>> GetSale(int? id)
        {
            try
            {
                if(id == null) return NotFound(new {message = "Id de la venta no encontrado."});

                SqlParameter SaleId = new SqlParameter("@id", id);
                var saleBD = await _context.SaleListVMs.FromSqlRaw($"sp_GetSaleId @id", SaleId).ToListAsync();


                if(saleBD == null) return NotFound(new { message = "Venta no encontrada."});

                return Ok(saleBD);
            }
            catch(Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        //añadir venta
        [HttpPost]
        public async Task<ActionResult> AddSale(List<InventoryVM> products, decimal total)
        {
            try
            {
                SqlParameter TotalParameter = new SqlParameter("@total", total);
                var res = await _context.ResponseVM.FromSqlRaw("sp_AddSale @total", TotalParameter).ToListAsync();

                res.ForEach(r =>
                {
                    validatorVM = r.Validator;
                    messageVM = r.Message!;
                });

                if (validatorVM == 0) return BadRequest(new { message = messageVM.ToString() });

                string query = "sp_AddDetailSale @sale_id, @inventory_id, @name, @amount, @unit_purchase_price, @unit_sales_price";
                foreach (var product in products)
                {
                    SqlParameter[] productModel =
                    {
                        new SqlParameter("@sale_id", Convert.ToInt32(messageVM.ToString())),
                        new SqlParameter("@inventory_id", product.InventoryId),
                        new SqlParameter("@name", product.Name),
                        new SqlParameter("@amount", product.Amount),
                        new SqlParameter("@unit_purchase_price", product.UnitPurchasePrice),
                        new SqlParameter("@unit_sales_price", product.UnitSalesPrice),
                    };
                    await _context.Database.ExecuteSqlRawAsync(query, productModel);
                    await AmountProductAsync(product.InventoryId, product.Amount);
                }
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

                //if (saleBD.Amount != SaleVM.Amount && SaleVM.Amount > saleBD.Amount) await AmountProductAsync(SaleVM.InventoryId, (SaleVM.Amount - saleBD.Amount));
                //if(saleBD.Amount != SaleVM.Amount && SaleVM.Amount < saleBD.Amount) await IncreaseAmountProductAsync(SaleVM.InventoryId, (saleBD.Amount - SaleVM.Amount));

                //saleBD.InventoryId= SaleVM.InventoryId;
                //saleBD.Name = SaleVM.Name;
                //saleBD.Amount = SaleVM.Amount;
                //saleBD.UnitPurchasePrice = SaleVM.UnitPurchasePrice;
                //saleBD.UnitSalesPrice= SaleVM.UnitSalesPrice;
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
        public async Task<ActionResult> PutStatus(int? id)
        {
            try
            {
                //if (id != SaleVM.SaleId || SaleVM == null) return NotFound(new { message = "el id no coincide o datos no enviados, intente otra vez." });

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
