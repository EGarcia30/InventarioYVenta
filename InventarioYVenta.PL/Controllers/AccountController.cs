using InventarioYVenta.Models.Models;
using InventarioYVenta.Models.ViewModels;
using InventarioYVenta.PL.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace InventarioYVenta.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly JsonSerializerOptions _JsonOptions = new JsonSerializerOptions() {PropertyNameCaseInsensitive = true};

        public AccountController()
        {

        }
        //Vista Login o Inicio de sesion
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        //funcion de login o inicio de sesion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(LoginVM LoginVM)
        {
            try{
                if(ModelState.IsValid)
                {
                    if(LoginVM == null){
                        ViewBag.Message = "Usuario no encontrado.";
                        return View();
                    }

                    using(HttpClient client = new HttpClient()){
                        var url = "https://localhost:7205/api/Account/Login";
                        var response = await client.PostAsJsonAsync(url, LoginVM);

                        switch(response.StatusCode)
                        {
                            case HttpStatusCode.OK:
                                var responseSuccess = await response.Content.ReadAsStringAsync();
                                var userBD = JsonSerializer.Deserialize<User>(responseSuccess, _JsonOptions);

                                var urlRol = $"https://localhost:7205/api/Roles/GetRole/{userBD!.RolId}";
                                var responseRol = await client.GetAsync(urlRol);

                                switch(responseRol.StatusCode){
                                    case HttpStatusCode.OK:
                                        var rolSuccess = await responseRol.Content.ReadAsStringAsync();
                                        var rolBD = JsonSerializer.Deserialize<Role>(rolSuccess, _JsonOptions);

                                        await Auth.CreateCookie(HttpContext, userBD, rolBD!.Name!);

                                        return RedirectToAction("Dashboard", "Admin");
                                    case HttpStatusCode.NotFound:
                                        var rolError = await responseRol.Content.ReadAsStringAsync();
                                        var msgRol = JsonSerializer.Deserialize<MessageError>(rolError, _JsonOptions);
                                        ViewBag.Message = msgRol?.Message;
                                        return View();
                                    default:
                                        return View();
                                }
                            case HttpStatusCode.NotFound:
                            case HttpStatusCode.BadRequest:
                                var responseError = await response.Content.ReadAsStringAsync();
                                var msgError = JsonSerializer.Deserialize<MessageError>(responseError, _JsonOptions);
                                ViewBag.Message = msgError?.Message;
                                return View();
                            default:
                                return View();
                        }
                    }
                }
                return View();
            }
            catch(Exception ex){
                throw new Exception($"Error: {ex.Message}");
            }
        }
    
        // *! funcion de log out o cerrar sesion(eliminacion de cookie)
        public async Task<IActionResult> LogOut(){
            await Auth.DeleteCookie(HttpContext);
            return RedirectToAction("Index", "Account");
        }
    }
}
