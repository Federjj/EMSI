using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMSI_Corporation.Models
{
    public class ReporteServicio
    {
        [Key]
        public int IdReporte { get; set; }
        public byte[] FirmaCliente { get; set; }
        public string Observaciones { get; set; }
        public byte[] ImgEvidencia { get; set; }

        [ForeignKey("Cliente")]
        public int Cliente_ID { get; set; }
        public Cliente Cliente { get; set; }

        [ForeignKey("ComprobanteServicio")]
        public int Comprobante_ID { get; set; }
        public ComprobanteServicio ComprobanteServicio { get; set; }

        public ICollection<Mantenimiento>? Mantenimientos { get; set; }

        public ICollection<Recarga>? Recargas { get; set; }
    }
}
