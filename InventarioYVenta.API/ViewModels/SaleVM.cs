using InventarioYVenta.API.Models;
using System.ComponentModel.DataAnnotations;

namespace InventarioYVenta.API.ViewModels
{
    public class SaleVM
    {
        public int SaleId { get; set; }

        [Required(ErrorMessage = "El id del producto en el inventario es requerido.")]
        public int? InventoryId { get; set; }

        [Required(ErrorMessage = "El nombre completo es requerido.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "La cantidad es requerido.")]
        public int? Amount { get; set; }

        [Required(ErrorMessage = "El precio de compra unitario es requerido.")]
        public decimal? UnitPurchasePrice { get; set; }

        [Required(ErrorMessage = "El precio de venta unitario es requerido.")]
        public decimal? UnitSalesPrice { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
