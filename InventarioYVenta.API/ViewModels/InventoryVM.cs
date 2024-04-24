using InventarioYVenta.API.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace InventarioYVenta.API.ViewModels
{
    public class InventoryVM
    {

        public int InventoryId { get; set; }

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
