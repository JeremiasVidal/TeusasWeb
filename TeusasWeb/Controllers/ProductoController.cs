using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using TeusasWeb.Models;
namespace TeusasWeb.Controllers
{
    public class ProductoController : Controller
    {
        private string url = "https://localhost:7160/";

        public IActionResult Index()
        {
            try
            {
               
                HttpClient cliente = new HttpClient();
                /** HEADERS **/
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                /** END HEADERS **/

                Uri uri = new Uri(url + "api/Producto/ListaProductos");
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();

                if (respuesta.Result.IsSuccessStatusCode)
                {
                    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                    IEnumerable<ProductoModel> ret = JsonConvert.DeserializeObject<IEnumerable<ProductoModel>>(response.Result);
                    return View(ret);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.msg = "Error: " + ex.Message;
                return View();
            }
        }
    }
}
