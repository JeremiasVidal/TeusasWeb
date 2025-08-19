using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using TeusasWeb.Models;
using System.Net;
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

        public IActionResult Create()
        
        {
            //Si tenes el token te manda al "Home" directo
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("token")) || HttpContext.Session.GetString("rol") != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();

        }
        [HttpPost]
        public IActionResult Create(string nombre, string descripcion, decimal precio, IFormFile imagen)
        {
            try
            {
                if (String.IsNullOrEmpty(HttpContext.Session.GetString("token"))
                    || HttpContext.Session.GetString("rol") != "admin")
                {
                    return RedirectToAction("Index", "Home");
                }

                // Cliente HTTP
                HttpClient cliente = new HttpClient();

                /******************* HEADERS *******************/
                Uri uri = new Uri(url + "Producto/Crear");
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Post, uri);

                /******************* CONTENIDO O BODY ********************/
                // Armamos multipart/form-data porque hay imagen
                MultipartFormDataContent contenido = new MultipartFormDataContent();
                // campos planos (coinciden con las props de ProductoDTO)
                contenido.Add(new StringContent(nombre), "nombre");
                contenido.Add(new StringContent(descripcion), "descripcion");
                contenido.Add(new StringContent(precio.ToString(System.Globalization.CultureInfo.InvariantCulture)), "precio");

                // archivo (el nombre debe coincidir con el parámetro del action)
                if (imagen != null && imagen.Length > 0)
                {
                    var img = new StreamContent(imagen.OpenReadStream());
                    img.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(imagen.ContentType);
                    contenido.Add(img, "imagen", imagen.FileName);
                }

                solicitud.Content = contenido;
                /*************** END CONTENIDO O BODY ********************/

                // Envía la solicitud HTTP de forma sincrónica (como tu Registro)
                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();

                Task<string> response = respuesta.Result.Content.ReadAsStringAsync();

                if (respuesta.Result.IsSuccessStatusCode)
                {

                    ViewBag.msg = "Producto creado con éxito";
                    return View();
                }
                else
                {
                    if (respuesta.Result.StatusCode == HttpStatusCode.BadRequest)
                    {
                        ViewBag.msg = response.Result;
                        return View();
                    }
                    ViewBag.msg = "Error al crear producto";
                    return View();
                }
            }
            catch (Exception e)
            {
                ViewBag.msg = "Error: " + e.Message;
                return View();
            }
        }

    }
}
    
  
