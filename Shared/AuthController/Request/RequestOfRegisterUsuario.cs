namespace Shared.AuthController.Request
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class RequestOfRegisterUsuario
    {
        [Required(ErrorMessage = "La cédula es obligatoria.")]
        public string Cedula { get; set; } = null!;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; } = null!;

        [Required(ErrorMessage = "El username es obligatorio.")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [Phone]
        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public string Direccion { get; set; } = null!;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [PasswordPropertyText(true)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
