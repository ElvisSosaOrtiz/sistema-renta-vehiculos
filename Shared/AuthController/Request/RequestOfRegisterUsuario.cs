namespace Shared.AuthController.Request
{
    public class RequestOfRegisterUsuario
    {
        public required string Cedula { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string UserName { get; set; }
        public required string Telefono { get; set; }
        public required string Direccion { get; set; }
        public required string Correo { get; set; }
        public required string Password { get; set; }
    }
}
