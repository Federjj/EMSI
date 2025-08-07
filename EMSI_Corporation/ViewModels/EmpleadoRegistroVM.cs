namespace EMSI_Corporation.ViewModels
{
    public class EmpleadoRegistroVM
    {
        // Datos del Empleado
        public string NomEmpleado { get; set; }
        public string ApeEmpleado { get; set; }
        public string DNI { get; set; }
        public string Gmail { get; set; }
        public string NumCelular { get; set; }

        // Datos del Usuario
        public string Usuario { get; set; }
        public string Password { get; set; }
        public int IdRol { get; set; }

    }
}
