using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMSI_Corporation.Models
{
    public class VisitaTecnica
    {
        [Key]
        public int IdVisita { get; set; }
        public DateTime Fecha { get; set; }
        public string Observaciones { get; set; }

        [ForeignKey("Empleado")]
        public int Empleado_ID { get; set; }
        public Empleado Empleado { get; set; }

        [ForeignKey("Cliente")]
        public int Cliente_ID { get; set; }
        public Cliente Cliente { get; set; }
    }
}
