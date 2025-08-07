using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EMSI_Corporation.Models
{
    public class Rol_Menu
    {
        public int IdRol { get; set; }
        public int IdMenu { get; set; }
        public virtual Rol Rol { get; set; }
        public virtual Menu Menu { get; set; }
    }
}
