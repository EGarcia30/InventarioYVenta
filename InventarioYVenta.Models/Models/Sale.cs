using System;
using System.Collections.Generic;

namespace InventarioYVenta.Models.Models
{
    public partial class Sale
    {
        public Sale()
        {
            Invoces = new HashSet<Invoce>();
            SaleDetails = new HashSet<SaleDetail>();
        }

        public int SaleId { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public decimal? Total { get; set; }

        public virtual ICollection<Invoce> Invoces { get; set; }
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
    }
}
