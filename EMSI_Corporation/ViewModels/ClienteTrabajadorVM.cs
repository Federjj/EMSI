using EMSI_Corporation.Models;

namespace EMSI_Corporation.ViewModels
{
    public class ClienteTrabajadorVM
    {
        public int clienteID { get; set; }
        public Cliente cliente { get; set; }
        public int empleadoID { get; set; }
        public Empleado empleado { get; set; }
        public string TipoServicio { get; set; }
    }
}
