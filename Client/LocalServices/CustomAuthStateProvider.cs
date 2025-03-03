namespace Client.LocalServices
{
    using Blazored.LocalStorage;
    using Microsoft.AspNetCore.Components.Authorization;
    using Shared.AuthController.Request;
    using Shared.AuthController.Response;
    using Shared.Routing;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public CustomAuthStateProvider(
            HttpClient httpClient,
            ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsStringAsync("accessToken");
            var refreshToken = await _localStorage.GetItemAsStringAsync("refreshToken");

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(refreshToken))
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                var refreshResult = await RefreshTokenAsync(token, refreshToken);
                if (!refreshResult)
                {
                    await MarkUserAsLoggedOut();
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        private async Task<bool> RefreshTokenAsync(string expiredToken, string refreshToken)
        {
            var request = new RequestOfRefreshToken { Token = expiredToken, RefreshToken = refreshToken };
            var response = await _httpClient.PostAsJsonAsync(AuthControllerRoutes.RefreshTokenOnClient, request);

            if (!response.IsSuccessStatusCode) return false;

            var newTokens = (await response.Content.ReadFromJsonAsync<ResponseOfGetToken>())!;
            await _localStorage.SetItemAsStringAsync("accessToken", newTokens.Token);
            await _localStorage.SetItemAsStringAsync("refreshToken", newTokens.RefreshToken);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return true;
        }

        public async Task MarkUserAsAuthenticated(string accessToken, string refreshToken)
        {
            await _localStorage.SetItemAsStringAsync("accessToken", accessToken);
            await _localStorage.SetItemAsStringAsync("refreshToken", refreshToken);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _localStorage.RemoveItemAsync("accessToken");
            await _localStorage.RemoveItemAsync("refreshToken");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string token)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwt.Claims;
        }
    }
}
