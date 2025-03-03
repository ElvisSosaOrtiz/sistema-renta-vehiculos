namespace ServiceContracts
{
    using Shared.AuthController.Request;
    using Shared.AuthController.Response;

    public interface IAuthService
    {
        Task<ResponseOfGetToken?> Login(RequestOfLoginUsuario request);
        Task<bool> Register(RequestOfRegisterUsuario request);
        Task<ResponseOfGetToken?> RefreshToken(RequestOfRefreshToken request);
    }
}
