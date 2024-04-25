using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventarioYVenta.PL.Controllers
{
    public class AccountController : Controller
    {
        //Login o Inicio de sesion
        public ActionResult Index()
        {
            return View();
        }

        
    }
}
