using EMSI_Corporation.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMSI_Corporation.ViewModels
{
    public class VentaVM
    {
        public DateTime FechaVenta { get; set; }
        public string MetodoPago { get; set; }
        public decimal Total { get; set; }

        public int Cliente_ID { get; set; }
        public Cliente Cliente { get; set; }

        public int Empleado_ID { get; set; }
        public Empleado Empleado { get; set; }

        public int Extintor_ID { get; set; }
        public Extintor Extintor { get; set; }
    }
}
