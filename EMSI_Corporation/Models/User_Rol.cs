using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EMSI_Corporation.Models
{
    public class User_Rol
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Rol Rol { get; set; }
    }
}
