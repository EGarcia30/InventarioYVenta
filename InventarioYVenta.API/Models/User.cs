using System;
using System.Collections.Generic;

namespace InventarioYVenta.API.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? Address { get; set; }
        public string? Dui { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public byte[]? PasswordHash { get; set; } = new byte[32];
        public byte[]? PasswordSalt { get; set; } = new byte[32];
        public int? RolId { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Role? Rol { get; set; }
    }
}
