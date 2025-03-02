namespace Shared.AuthController.Request
{
    public class RequestOfLoginUsuario
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
