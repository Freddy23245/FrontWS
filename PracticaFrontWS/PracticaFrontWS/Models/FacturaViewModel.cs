namespace PracticaFrontWS.Models
{
    public class FacturaViewModel
    {
        public string clientes { get; set; }
        public string direccion { get; set; }
        public string CUIT { get; set; }
        public string condicionVenta { get; set; }
        public string remito { get; set; }
        public string codigo { get; set; }
        public int cantidad { get; set; }
        public string detalle { get; set; }
        public decimal precioUnitario { get; set; }
        public decimal total { get; set; }
        public decimal impuesto { get; set; }
        public decimal Iva { get;set; }
        public decimal IvaNO { get;set; }
    }
}
