using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EMSI_Corporation.Models
{
    public class Recepcion
    {
        [Key]
        public int IdRecepcion { get; set; }

        public DateTime Fecha { get; set; }

        // Usuario que recibe
        public int? UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        // Proveedor que envía
        public int ProovedorId { get; set; }
        public Proovedor Proovedor { get; set; }

    }
}
