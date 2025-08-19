namespace TeusasWeb.Models
{
    public class UsuarioModel
    {

        public int id { get; set; }
        public string nombre { get; set; }
        public string contrasenia { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public string? rol { get; set; }



        public IEnumerable<PedidoModel>? pedidos { get; set; }
        public CarritoModel? carrito { get; set; }

    }
}
