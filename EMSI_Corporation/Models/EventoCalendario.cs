using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMSI_Corporation.Models
{
    public class EventoCalendario
    {
        [Key]
        public int IdEvento { get; set; }

        [Required]
        [StringLength(150)]
        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [StringLength(20)]
        public string Color { get; set; }

        public int EmpleadoId { get; set; }

        [ForeignKey("EmpleadoId")]
        public Empleado? Empleado { get; set; }
    }
}
