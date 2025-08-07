using System.ComponentModel.DataAnnotations;

namespace EMSI_Corporation.Models
{
    public class ComprobanteServicio
    {
        [Key]
        public int IdComprobante { get; set; }
        public string TipoServicio { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal SubTotal { get; set; }
    }
}
