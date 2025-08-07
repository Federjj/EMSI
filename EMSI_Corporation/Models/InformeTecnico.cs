using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMSI_Corporation.Models
{
    public class InformeTecnico
    {
        [Key]
        public int IdInforme { get; set; }
        public DateTime FechaInforme { get; set; }
        public string Informe_PDF { get; set; }

        [ForeignKey("VisitaTecnica")]
        public int Visita_ID { get; set; }
        public VisitaTecnica VisitaTecnica { get; set; }
    }
}
