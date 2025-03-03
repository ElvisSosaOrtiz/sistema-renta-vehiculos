namespace Shared.AuthController.Response
{
    public class ResponseOfGetToken
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
