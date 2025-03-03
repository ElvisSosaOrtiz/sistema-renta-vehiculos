namespace Client.Pages.Administrador
{
    using Client.Routing;
    using Microsoft.AspNetCore.Components;
    using Shared.Routing;
    using Shared.VehiculoController.Request;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    public partial class AddVehiculo
    {
        [Inject] public HttpClient HttpClient { get; set; } = null!;
        [Inject] public NavigationManager NavManager { get; set; } = null!;

        private RequestOfCreateVehiculo _request = new();
        private string _errorMessage = string.Empty;
        private string _displayErrorClass = "d-none";

        private async Task HandleCreateVehiculoAsync()
        {
            var response = await HttpClient.PostAsJsonAsync(VehiculoControllerRoutes.AddVehiculo, _request);

            if (!response.IsSuccessStatusCode)
            {
                _errorMessage = "No se pudo agregar el vehículo. Intente de nuevo más tarde";
                _displayErrorClass = string.Empty;
            }

            NavManager.NavigateTo(ClientRoutes.VehiculoListRoute);
        }
    }
}
