using InventarioYVenta.API.Models;
using System.ComponentModel.DataAnnotations;

namespace InventarioYVenta.API.ViewModels
{
    public class UpdateUserVM
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "El nombre completo es requerido.")]
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public DateTime? Birthdate { get; set; }

        [Required(ErrorMessage = "La dirección es requerido.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "El dui es requerido.")]
        public string? Dui { get; set; }
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "El email es requerido."), EmailAddress]
        public string? Email { get; set; }
        public int? RolId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

    }
}
