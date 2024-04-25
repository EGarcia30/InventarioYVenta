using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventarioYVenta.DAL.Context;
using InventarioYVenta.Models.Models;
using InventarioYVenta.Models.ViewModels;

namespace InventarioYVenta.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly InventarioYVentaDbContext _context;

        public RolesController(InventarioYVentaDbContext context)
        {
            _context = context;
        }

        //Obtener lista de roles GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            try
            {
                if (_context.Roles == null)
                {
                    return NotFound();
                }
                return await _context.Roles.Where(rolBd => rolBd.Status == 1).ToListAsync();
            }
            catch(Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }

        //Obtener rol GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            try
            {
                if (_context.Roles == null)
                {
                    return NotFound();
                }
                var role = await _context.Roles.FirstOrDefaultAsync(rol => rol.RolId.Equals(id));

                if (role == null)
                {
                    return NotFound();
                }

                return role;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }

        //Añadir Rol POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> AddRole(RoleVM roleVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(roleVM == null)
                    {
                        return NotFound();
                    }

                    Role NewRole = new Role()
                    {
                        Name = roleVM.Name,
                        Status = 1,
                        CreatedAt = DateTime.Now,
                    };

                    _context.Add(NewRole);
                    await _context.SaveChangesAsync();

                    return Ok(new { message = "Rol creado." });
                }
                throw new Exception($"Completa los campos.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }


        }

        //Actualizar Rol PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, RoleVM roleVM)
        {
            try
            {
                if (id != roleVM.RolId)
                {
                    return BadRequest();
                }

                var roleBD = await _context.Roles.FirstOrDefaultAsync(userBd => userBd.RolId.Equals(id));

                if(roleBD == null)
                {
                    return NotFound();
                }

                roleBD.Name = roleVM.Name;
                roleBD.UpdatedAt = DateTime.Now;

                _context.Update(roleBD);
                await _context.SaveChangesAsync();

                return Ok(new {message = "Rol actualizado."});

            }
            catch(Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        //Mandar al historial
        public async Task<IActionResult> PutStatus(int id, RoleVM roleVM)
        {
            try
            {
                if (id != roleVM.RolId)
                {
                    return BadRequest(new { message =  "El id no coincide con el modelo." });
                }

                var roleBD = await _context.Roles.FirstOrDefaultAsync(userBd => userBd.RolId.Equals(id));

                if (roleBD == null)
                {
                    return NotFound(new { message = "El rol no es encontrado." });
                }

                roleBD.Status = 0;
                roleBD.DeletedAt = DateTime.Now;

                _context.Update(roleBD);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Rol al historial." });

            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        //Eliminar Rol DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            if (_context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
