using System.ComponentModel.DataAnnotations;

namespace EMSI_Corporation.Models
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }
        public string NomCliente { get; set; }
        public string TipoCliente { get; set; }
        public string TipoDocumento { get; set; }
        public string NumDocumento { get; set; }
        public string NumCelular { get; set; }
        public string Correo { get; set; }
    }
}
