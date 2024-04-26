using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioYVenta.PL.Controllers{

    [Authorize(Policy = "Administrador")]
    public class AdminController : Controller 
    {
        //Dashboard de información del negocio
        [HttpGet]
        public ActionResult Dashboard(){
            return View();
        }
    }
}