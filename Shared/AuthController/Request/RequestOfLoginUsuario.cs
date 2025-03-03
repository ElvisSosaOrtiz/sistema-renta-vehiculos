namespace Shared.AuthController.Request
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class RequestOfLoginUsuario
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [PasswordPropertyText(true)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
