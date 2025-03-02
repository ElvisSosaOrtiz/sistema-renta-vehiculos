namespace Shared.AuthController.Request
{
    public class RequestOfRefreshToken
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
