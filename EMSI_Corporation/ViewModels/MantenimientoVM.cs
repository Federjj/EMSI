using EMSI_Corporation.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMSI_Corporation.ViewModels
{
    public class MantenimientoVM
    {
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
