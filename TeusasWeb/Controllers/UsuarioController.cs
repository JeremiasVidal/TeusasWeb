using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TeusasWeb.Models;

namespace TeusasWeb.Controllers
{
    public class UsuarioController : Controller
    { // URL base de la API a la que se hacen las peticiones.
        private string url = "https://localhost:7160/";

        // Acción para mostrar la vista de inicio de sesión.
        public IActionResult Login()
        {
            //Si tenes el token te manda al "Home" directo
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("token")))
            {
                return RedirectToAction("Index", "Home");
            }
        
            return View();
        }

        // Acción para procesar los datos enviados desde el formulario de login.
        [HttpPost]
        public IActionResult Login(UsuarioModel usuario)
        {
            try
            {
                // Crea un cliente HTTP para realizar la solicitud.
                HttpClient cliente = new HttpClient();

                /******************* HEADERS *******************/
                // Configuración de la URI de la solicitud de login.
                Uri uri = new Uri(url + "Login");
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Post, uri);

                /******************* CONTENIDO O BODY ********************/
                // Serializa el objeto usuario al formato JSON y lo agrega al cuerpo de la solicitud.
                string json = JsonConvert.SerializeObject(usuario);
                HttpContent contenido =
                new StringContent(json, Encoding.UTF8, "application/json");
                solicitud.Content = contenido;
                /*************** END CONTENIDO O BODY ********************/

                // Envía la solicitud HTTP de forma asincrónica y espera la respuesta.
                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();

                // Lee el contenido de la respuesta.
                Task<string> response = respuesta.Result.Content.ReadAsStringAsync();

                // Si el login es exitoso (código de estado 200), guarda los datos del usuario en la sesión.
                if (respuesta.Result.IsSuccessStatusCode)
                {
                    UsuarioModel usuarioModel = JsonConvert.DeserializeObject<UsuarioModel>(response.Result);
                    HttpContext.Session.SetString("token", usuarioModel.Token);
                    HttpContext.Session.SetString("nombreUsuario", usuario.nombre);

                 

                    // Redirige al usuario a la página principal (Index) del controlador Home.
                    return RedirectToAction("Index", "Home");
                }
                // Si el login falla debido a un error de autenticación (código 400), muestra un mensaje.
                else if (respuesta.Result.StatusCode == HttpStatusCode.BadRequest)
                {
                    ViewBag.msg = "Usuario o contraseña incorrecta";
                    return View();
                }
                // Si ocurre otro tipo de error, muestra un mensaje genérico.
                else
                {
                    ViewBag.msg = "Error en la base de datos";
                    return View();
                }
            }
            catch (Exception e)
            {
                // Captura cualquier excepción y muestra un mensaje de error.
                ViewBag.msg = "Error: " + e.Message;
                return View();
            }
        }
        public IActionResult Registro()
        {
            //Si tenes el token te manda al "Home" directo
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("token")))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Registro(UsuarioModel usuario)
        {
            try
            {
                // Crea un cliente HTTP para realizar la solicitud.
                HttpClient cliente = new HttpClient();

                /******************* HEADERS *******************/
                // Configuración de la URI de la solicitud de login.
                Uri uri = new Uri(url + "Registro");
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Post, uri);

                /******************* CONTENIDO O BODY ********************/
                // Serializa el objeto usuario al formato JSON y lo agrega al cuerpo de la solicitud.
                string json = JsonConvert.SerializeObject(usuario);
                HttpContent contenido =
                new StringContent(json, Encoding.UTF8, "application/json");
                solicitud.Content = contenido;
                /*************** END CONTENIDO O BODY ********************/

                // Envía la solicitud HTTP de forma asincrónica y espera la respuesta.
                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();

                // Lee el contenido de la respuesta.
                Task<string> response = respuesta.Result.Content.ReadAsStringAsync();

                // Si el regstro es exitoso (código de estado 200), guarda los datos del usuario en la sesión.
                if (respuesta.Result.IsSuccessStatusCode)
                {
                    UsuarioModel usuarioModel = JsonConvert.DeserializeObject<UsuarioModel>(response.Result);
                    HttpContext.Session.SetString("token", usuarioModel.Token);
                    HttpContext.Session.SetString("nombreUsuario", usuario.nombre);



                    // Redirige al usuario a la página principal (Index) del controlador Home.
                    return RedirectToAction("Index", "Home");
                }
               
                // Si ocurre otro tipo de error, muestra un mensaje genérico.
                else
                {
                    if(respuesta.Result.StatusCode == HttpStatusCode.BadRequest)
                    {
                        
                        ViewBag.msg = respuesta.Result;
                        return View();
                    }
                    ViewBag.msg = "Error en la base de datos";
                    return View();
                }
            }
            catch (Exception e)
            {
                // Captura cualquier excepción y muestra un mensaje de error.
                ViewBag.msg = "Error: " + e.Message;
                return View();
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Usuario");
        }
    }
}
