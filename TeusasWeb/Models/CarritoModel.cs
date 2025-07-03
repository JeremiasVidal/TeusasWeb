namespace TeusasWeb.Models
{
    public class CarritoModel
    {
        public int id { get; set; }
        public IEnumerable<DetallePedidoModel> productos { get; set; }
    }
}
