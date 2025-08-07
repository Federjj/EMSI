using System.ComponentModel.DataAnnotations;

namespace EMSI_Corporation.Models
{
    public class Proovedor
    {
        [Key]
        public int IdProovedor { get; set; }
        public string RazonSocial { get; set; }
        public string RUC {  get; set; }
        public string Direccion { get; set; }
        public string NumCelular { get; set; }
        public string Correo {  get; set; }
        public Boolean estado { get; set; }

        public ICollection<Recepcion> Recepciones { get; set; }
    }
}
