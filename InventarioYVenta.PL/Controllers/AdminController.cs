using System.Text.Json;
using InventarioYVenta.Models.Models;
using InventarioYVenta.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioYVenta.PL.Controllers{

    [Authorize(Policy = "Administrador")]
    public class AdminController : Controller 
    {
        private readonly IConfiguration _configuration;
        private readonly JsonSerializerOptions _JsonOptions = new JsonSerializerOptions() {PropertyNameCaseInsensitive = true};

        public AdminController(IConfiguration configuration){
            _configuration = configuration;
        }

        //Dashboard de información del negocio
        [HttpGet]
        public ActionResult Dashboard(){
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Products(string search){
            
            //Traer venta por busqueda de id
            if(!string.IsNullOrEmpty(search)){
                using(HttpClient client = new HttpClient()){
                    var url = $"{_configuration["Urls:Api"]}Inventory/GetProduct?search={search}";
                    var response = await client.GetAsync(url);
                    var responseStr = await response.Content.ReadAsStringAsync();
                    if(response.IsSuccessStatusCode){
                        var productList = JsonSerializer.Deserialize<List<Inventory>>(responseStr, _JsonOptions);
                        return View(productList);
                    } 

                    var message = JsonSerializer.Deserialize<string>(responseStr, _JsonOptions);
                    ViewBag.Message = message!.ToString();
                    return View();
                }
            }

            using(HttpClient client = new HttpClient()){
                var url = $"{_configuration["Urls:Api"]}Inventory/GetProducts";
                var response = await client.GetAsync(url);
                var responseStr = await response.Content.ReadAsStringAsync();
                if(response.IsSuccessStatusCode){
                    var productList = JsonSerializer.Deserialize<List<Inventory>>(responseStr, _JsonOptions);
                        return View(productList);
                }

                var message = JsonSerializer.Deserialize<string>(responseStr, _JsonOptions);
                ViewBag.Message = message!.ToString();
                return View();
            }
        }
    }
}