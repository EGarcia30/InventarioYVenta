using System;
using System.Collections.Generic;

namespace InventarioYVenta.Models.Models
{
    public partial class SaleDetail
    {
        public int SaleDetailId { get; set; }
        public int? SaleId { get; set; }
        public int? InventoryId { get; set; }
        public string? Name { get; set; }
        public int? Amount { get; set; }
        public decimal? UnitPurchasePrice { get; set; }
        public decimal? UnitSalesPrice { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Inventory? Inventory { get; set; }
        public virtual Sale? Sale { get; set; }
    }
}
