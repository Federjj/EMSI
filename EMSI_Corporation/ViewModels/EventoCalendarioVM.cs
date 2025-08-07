using System;
using System.ComponentModel.DataAnnotations;

namespace EMSI_Corporation.ViewModels
{
    public class EventoCalendarioVM
    {
        public int IdEvento { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(150, ErrorMessage = "El título no puede exceder los 150 caracteres.")]
        public string Titulo { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [DataType(DataType.DateTime)]
        public DateTime FechaInicio { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? FechaFin { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un color.")]
        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "Color no válido.")]
        public string Color { get; set; }

        //validaciones para fechas
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var ahora = DateTime.Now;

            // Esto permite mantener la fecha original en ediciones sin exigir que sea > ahora
            if (FechaInicio == default)
            {
                yield return new ValidationResult("La fecha de inicio es obligatoria.", new[] { nameof(FechaInicio) });
            }

            if (FechaInicio != default && FechaInicio < ahora && IdEvento == 0) // Solo al CREAR
            {
                yield return new ValidationResult("La fecha de inicio debe ser igual o posterior a la fecha actual.", new[] { nameof(FechaInicio) });
            }

            if (FechaFin.HasValue && FechaFin < FechaInicio)
            {
                yield return new ValidationResult("La fecha de fin no puede ser anterior a la fecha de inicio.", new[] { nameof(FechaFin) });
            }
        }


    }
}
