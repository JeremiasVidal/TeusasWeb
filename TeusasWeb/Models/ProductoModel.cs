namespace TeusasWeb.Models
{
    public class ProductoModel
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public double precio { get; set; }
        public string foto { get; set; }
        public OfertaModel oferta { get; set; }
        public int cantMaxPorPaquete { get; set; }
        public double peso { get; set; }
    }
}
