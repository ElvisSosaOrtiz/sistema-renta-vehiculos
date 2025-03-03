namespace Client.LocalServices
{
    using Microsoft.AspNetCore.Components.Authorization;
    using Shared.AuthController.Request;
    using Shared.AuthController.Response;
    using Shared.Routing;
    using System.Net.Http.Json;

    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly CustomAuthStateProvider _authStateProvider;

        public AuthService(
            HttpClient httpClient,
            AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider as CustomAuthStateProvider ??
                throw new InvalidOperationException("CustomAuthStateProvider not registered correctly.");
        }

        public async Task<bool> Login(RequestOfLoginUsuario request)
        {
            var response = await _httpClient.PostAsJsonAsync(AuthControllerRoutes.LoginOnClient, request);

            if (!response.IsSuccessStatusCode) return false;

            var tokenResponse = await response.Content.ReadFromJsonAsync<ResponseOfGetToken>();
            await _authStateProvider.MarkUserAsAuthenticated(tokenResponse!.Token, tokenResponse.RefreshToken);

            return true;
        }

        public async Task<HttpResponseMessage> Register(RequestOfRegisterUsuario request)
        {
            var response = await _httpClient.PostAsJsonAsync(AuthControllerRoutes.RegisterOnClient, request);
            return response;
        }

        public async Task Logout() => await _authStateProvider.MarkUserAsLoggedOut();
    }
}
