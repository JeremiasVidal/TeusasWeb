namespace TeusasWeb.Models
{
    public class PedidoModel
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public double costoTotal { get; set; }
        public string estado { get; set; }
        public IEnumerable<DetallePedidoModel> productos { get; set; }
        public int? idMetodoPago { get; set; }
        public MetodoPagoModel metodoPago { get; set; }
    }
}
