using System.ComponentModel.DataAnnotations;

namespace EMSI_Corporation.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [RegularExpression(@"^[a-zA-Z0-9@._]+$", ErrorMessage = "El nombre de usuario contiene caracteres inválidos")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria"), StringLength(50, ErrorMessage = "La contraseña no puede tener más de 50 caracteres")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
