using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMSI_Corporation.Models
{
    public class Comprobante
    {
        [Key]
        public int IdComprobante { get; set; }
        public string TipoComprobante { get; set; }
        public string NroComprobante { get; set; }
        public decimal MontoTotal { get; set; }

        [ForeignKey("Venta")]
        public int Venta_ID { get; set; }
        public Venta Venta { get; set; }
    }
}
