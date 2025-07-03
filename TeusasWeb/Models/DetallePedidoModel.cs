namespace TeusasWeb.Models
{
    public class DetallePedidoModel
    {
        public int id { get; set; }
        public int cantidad { get; set; }
        public double precioUnitario { get; set; }
        public int productoId { get; set; }
        public ProductoModel producto { get; set; }
        public int cantPaquetes { get; set; }
        public string comprobPago { get; set; }

    }
}
