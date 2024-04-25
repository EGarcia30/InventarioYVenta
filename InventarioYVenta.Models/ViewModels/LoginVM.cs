using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioYVenta.Models.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "La contraseña es requerido.")]
        public string? Password { get; set; }
    }
}
