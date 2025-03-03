namespace Client.Pages.Authentication
{
    using Client.LocalServices;
    using Client.Routing;
    using Microsoft.AspNetCore.Components;
    using Shared.AuthController.Request;
    using Shared.AuthController.Response;
    using System.Net.Http.Json;

    public partial class Register
    {
        [Inject] public AuthService AuthService { get; set; } = null!;
        [Inject] public NavigationManager NavManager { get; set; } = null!;

        private RequestOfRegisterUsuario _registerRequest = new();
        private string _errorMessage = string.Empty;
        private string _displayErrorClass = "d-none";

        private async Task HandleRegister()
        {
            _displayErrorClass = "d-none";
            var response = await AuthService.Register(_registerRequest);

            if (response.IsSuccessStatusCode) NavManager.NavigateTo(ClientRoutes.LoginRoute);
            else
            {
                var error = await response.Content.ReadFromJsonAsync<List<ResponseOfRegisterError>>();
                _errorMessage = error!.First().Description;
                _displayErrorClass = string.Empty;
            }
        }
    }
}
