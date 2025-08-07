using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EMSI_Corporation.Models
{
    public class Rol
    {
        public int IdRol { get; set; }
        public string nomRol { get; set; }
        public string Descripcion { get; set; }
        public virtual ICollection<User_Rol> UserRoles { get; set; }
        public virtual ICollection<Rol_Menu> RolMenus { get; set; }
    }
}
