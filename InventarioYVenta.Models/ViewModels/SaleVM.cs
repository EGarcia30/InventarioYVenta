using System.ComponentModel.DataAnnotations;

namespace InventarioYVenta.Models.ViewModels
{
    public class SaleVM
    {
        public int SaleId { get; set; }

        public int status { get; set; }

        public double total { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
