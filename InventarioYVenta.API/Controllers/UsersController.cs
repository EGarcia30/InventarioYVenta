using InventarioYVenta.DAL.Context;
using InventarioYVenta.API.Helpers;
using InventarioYVenta.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventarioYVenta.Models.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventarioYVenta.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly InventarioYVentaDbContext _context;
        public UsersController(InventarioYVentaDbContext context)
        {
            _context = context;
        }

        //Obtener lista de usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                var userList = await _context.Users.Where(userBd => userBd.Status == 1).ToListAsync();

                if(userList == null || !userList.Any())
                {
                    return NotFound();
                }

                return Ok(userList);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }

        //Obtener usuario
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int? id)
        {
            try
            {
                if(id == null)
                {
                    return NotFound();
                }

                var userBD = await _context.Users.FirstOrDefaultAsync(userBd => userBd.UserId.Equals(id));

                if(userBD == null)
                {
                    return NotFound();
                }

                return userBD;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
        
        //Ingresar un nuevo usuario
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserVM userVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var boolUser = await _context.Users.AnyAsync(userBd => userBd.Username!.Equals(userVM.Username));

                    if (boolUser)
                    {
                        return Ok(new { message = "Nombre de usuario en existencia." });
                    }

                    PasswordSecurity.CreatePasswordHash(userVM.Password!, out byte[] PasswordHashS, out byte[] PasswordSaltS);

                    User NewUser = new User()
                    {
                        Name = userVM.Name,
                        Gender = userVM.Gender,
                        Birthdate = userVM.Birthdate,
                        Address = userVM.Address,
                        Dui = userVM.Dui,
                        PhoneNumber = userVM.PhoneNumber,
                        Username = userVM.Username,
                        Email = userVM.Email,
                        PasswordHash = PasswordHashS,
                        PasswordSalt = PasswordSaltS,
                        RolId = userVM.RolId,
                        Status = 1,
                        CreatedAt= DateTime.Now
                    };

                    _context.Add(NewUser);
                    await _context.SaveChangesAsync();

                    return Ok(new { message = "Usuario creado." });

                }
                throw new Exception("Completa todos los campos.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }

        //Actualizar un usuario
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int? id, UpdateUserVM updateUserVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(id != updateUserVM.UserId)
                    {
                        return NotFound(new { message = "El Id no coincide con el id del modelo." });
                    }

                    var userBD = await _context.Users.FirstOrDefaultAsync(userBd => userBd.UserId.Equals(id));

                    if (userBD == null)
                    {
                        return NotFound(new { message = "No se encontro usuario en base de datos, intente de nuevo." });
                    }

                    userBD.Name = updateUserVM.Name;
                    userBD.Gender = updateUserVM.Gender;
                    userBD.Birthdate = updateUserVM.Birthdate;
                    userBD.Address = updateUserVM.Address;
                    userBD.Dui = updateUserVM.Dui;
                    userBD.PhoneNumber = updateUserVM.PhoneNumber;
                    userBD.Username = updateUserVM.Username;
                    userBD.Email = updateUserVM.Email;
                    userBD.RolId = updateUserVM.RolId;
                    userBD.UpdatedAt = DateTime.Now;

                    _context.Update(userBD);
                    await _context.SaveChangesAsync();

                    return Ok(new { message = "Usuario actualizado." });

                }
                throw new Exception("Completa todos los campos.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }

        //Historial un usuario
        [HttpPut("{id}")]
        public async Task<ActionResult> PutStatus(int? id, UpdateUserVM updateUserVM)
        {
            try
            {
                if (id != updateUserVM.UserId)
                {
                    return NotFound(new { message = "El Id no coincide con el id del modelo." });
                }

                var userBD = await _context.Users.FirstOrDefaultAsync(userBd => userBd.UserId.Equals(id));

                if (userBD == null)
                {
                    return NotFound(new { message = "No se encontro usuario en base de datos, intente de nuevo." });
                }

                userBD.Status = 0;
                userBD.DeletedAt = DateTime.Now;

                _context.Update(userBD);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Usuario enviado al historial." });
            }
            catch(Exception ex)
            {
                throw new Exception($"Error {ex.Message}");
            }
        }

        //Eliminar
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            try
            {
                if(id == null)
                {
                    return BadRequest();
                }

                var userBD = await _context.Users.FirstOrDefaultAsync(userBd => userBd.UserId.Equals(id));

                if (userBD == null)
                {
                    return NotFound();
                }

                _context.Users.Remove(userBD);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}
