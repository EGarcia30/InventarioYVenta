using System;
using System.Collections.Generic;

namespace InventarioYVenta.Models.Models
{
    public partial class Inventory
    {
        public Inventory()
        {
            Invoces = new HashSet<Invoce>();
            Sales = new HashSet<Sale>();
        }

        public int InventoryId { get; set; }
        public string? Name { get; set; }
        public int? Amount { get; set; }
        public decimal? UnitPurchasePrice { get; set; }
        public decimal? UnitSalesPrice { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Invoce> Invoces { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
