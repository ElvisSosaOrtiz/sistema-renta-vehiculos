namespace Client.Pages.Authentication
{
    using Client.LocalServices;
    using Client.Routing;
    using Microsoft.AspNetCore.Components;
    using Shared.AuthController.Request;

    public partial class Login
    {
        [Inject] public AuthService AuthService { get; set; } = null!;
        [Inject] public NavigationManager NavManager { get; set; } = null!;

        private RequestOfLoginUsuario _loginRequest = new();
        private string _errorMessage = string.Empty;
        private string _displayErrorClass = "d-none";

        private async Task HandleLogin()
        {
            _displayErrorClass = "d-none";
            var success = await AuthService.Login(_loginRequest);

            if (success) NavManager.NavigateTo(ClientRoutes.VehiculosRoute);
            else
            {
                _errorMessage = "Correo o contraseña incorrectos. Intente de nuevo.";
                _displayErrorClass = string.Empty;
            }
        }
    }
}
