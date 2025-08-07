using EMSI_Corporation.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMSI_Corporation.ViewModels
{
    public class ServicioMantenimientoVM
    {
        //Reporte servicio
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
        //mant
        public DateTime FechaMantenimiento { get; set; }
        public bool LugarAdecuado { get; set; }
        public bool Señalización { get; set; }
        public bool Visible { get; set; }
        public bool Usado { get; set; }
        public bool EstadoPrecinto { get; set; }
        public bool EstadoIndicador { get; set; }
        public bool PresionCorrecta { get; set; }
        public bool ExteriorCorrecto { get; set; }
        public bool PesoCorrecto { get; set; }
        public bool MangueraCorrecta { get; set; }
        public bool BoquillaCorrecta { get; set; }
        public bool InstruccionesVisibles { get; set; }
        public bool AperturaCorrecta { get; set; }
        public bool BarometroCorrecto { get; set; }
        public int Empleado_ID { get; set; }
        public Empleado Empleado { get; set; }
        public int Extintor_ID { get; set; }
        public Extintor Extintor { get; set; }
    }
}
