using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMSI_Corporation.Models
{
    public class Mantenimiento
    {
        [Key]
        public int IdMantenimiento { get; set; }
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

        [ForeignKey("Empleado")]
        public int Empleado_ID { get; set; }
        public Empleado Empleado { get; set; }


        [ForeignKey("Extintor")]
        public int Extintor_ID { get; set; }
        public Extintor Extintor { get; set; }

        [ForeignKey("ReporteServicio")]
        public int ReporteServicio_ID { get; set; }
        public ReporteServicio ReporteServicio { get; set; }
    }
}
