using System.ComponentModel.DataAnnotations;

namespace EMSI_Corporation.Models
{
    public class Menu
    {
        public int IdMenu { get; set; }
        public string nomMenu { get; set; }
        public string Url { get; set; }
        public virtual ICollection<Rol_Menu> RolMenus { get; set; }
    }
}
