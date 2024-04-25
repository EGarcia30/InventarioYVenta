using InventarioYVenta.API.Helpers;
using InventarioYVenta.DAL.Context;
using InventarioYVenta.Models.Models;
using InventarioYVenta.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventarioYVenta.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly InventarioYVentaDbContext _context;

        public AccountController(InventarioYVentaDbContext context)
        {
            _context = context;
        }

        //Verificar que exista usuario en BD y retornarlo
        [HttpPost]
        public async Task<ActionResult<User>> LoginAsync(LoginVM LoginVM)
        {
            try
            {
                if(LoginVM == null) return NotFound(new {message = "Datos no enviados."});

                var userBD = await _context.Users.FirstOrDefaultAsync(user => user.Username!.Equals(LoginVM!.Username));

                if(userBD == null) return NotFound(new { message = "Nombre de usuario no encontrado en BD."});

                if (!PasswordSecurity.VerifyPasswordHash(LoginVM!.Password!, userBD.PasswordHash!, userBD.PasswordSalt!)) 
                    return BadRequest(new {message = "Contraseña incorrecta."});

                return Ok(userBD);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
    }
}
