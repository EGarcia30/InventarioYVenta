using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioYVenta.Models.ViewModels
{
    public class ResponseVM
    {
        [Key]
        public int Validator { get; set; }

        public string? Message { get; set; }
    }
}
