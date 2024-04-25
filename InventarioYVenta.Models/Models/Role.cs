using System;
using System.Collections.Generic;

namespace InventarioYVenta.Models.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int RolId { get; set; }
        public string? Name { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
