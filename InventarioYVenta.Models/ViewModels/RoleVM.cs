using InventarioYVenta.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace InventarioYVenta.Models.ViewModels
{
    public class RoleVM
    {
        public int RolId { get; set; }

        [Required(ErrorMessage = "El nombre completo es requerido.")]
        public string? Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
