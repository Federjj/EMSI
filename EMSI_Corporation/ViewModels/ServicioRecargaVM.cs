using EMSI_Corporation.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMSI_Corporation.ViewModels
{
    public class ServicioRecargaVM
    {
        public byte[] FirmaCliente { get; set; }
        public string Observaciones { get; set; }
        public byte[] ImgEvidencia { get; set; }
        public int Cliente_ID { get; set; }
        public Cliente Cliente { get; set; }

        public int Comprobante_ID { get; set; }
        public ComprobanteServicio ComprobanteServicio { get; set; }

        public int Mantenimiento_ID { get; set; }
        public Mantenimiento Mantenimiento { get; set; }

        public int Recarga_ID { get; set; }
        public Recarga Recarga { get; set; }

        //Comprobante servicio
        public string TipoServicio { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal SubTotal { get; set; }

        //recarga
        public int IdRecarga { get; set; }
        public DateTime FechaRecarga { get; set; }
        public string MaterialUsado { get; set; }
        public decimal CantidadKG { get; set; }
        public decimal PresionPostRecarga { get; set; }

        public int Extintor_ID { get; set; }
        public Extintor Extintor { get; set; }

        public int Empleado_ID { get; set; }
        public Empleado Empleado { get; set; }
        public int ReporteServicio_ID { get; set; }
        public ReporteServicio ReporteServicio { get; set; }
    }
}
