using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMSI_Corporation.Models
{
    public class Venta
    {
        [Key]
        public int IdVenta { get; set; }
        public DateTime FechaVenta { get; set; }
        public string MetodoPago { get; set; }
        public decimal Total { get; set; }

        [ForeignKey("Cliente")]
        public int Cliente_ID { get; set; }
        public Cliente Cliente { get; set; }

        [ForeignKey("Empleado")]
        public int Empleado_ID { get; set; }
        public Empleado Empleado { get; set; }
    }
}
