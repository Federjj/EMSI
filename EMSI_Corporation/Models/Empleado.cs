namespace EMSI_Corporation.Models
{
    public class Empleado
    {
        public int IdEmpleado { get; set; }
        public string nomEmpleado { get; set; }
        public string apeEmpleado { get; set; }
        public string dni { get; set; }
        public string gmail { get; set; }
        public string numCelular { get; set; }

        /* Relación con Usuario 1:1 */
        public Usuario usuario { get; set; }
    }
}
