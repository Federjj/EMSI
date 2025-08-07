using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMSI_Corporation.Models
{
    public class Extintor
    {
        [Key]
        public int IdExtintor { get; set; }
        public string TipoAgente { get; set; }
        public string ClaseFuego { get; set; }
        public decimal CapacidadKG { get; set; }
        public int CantidadDisponible { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Estado { get; set; }

        [ForeignKey("Venta")]
        public int? Venta_ID { get; set; }
        public Venta Venta { get; set; }

        [ForeignKey("Recepcion")]
        public int? RecepcionId { get; set; }
        public Recepcion Recepcion { get; set; }

    }
}
