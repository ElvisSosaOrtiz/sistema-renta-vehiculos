namespace Client.Pages.Administrador
{
    using Client.Routing;
    using Microsoft.AspNetCore.Components;
    using Shared.Routing;
    using Shared.VehiculoController.Request;
    using Shared.VehiculoController.Response;
    using System.Net.Http.Json;

    public partial class EditVehiculo
    {
        [Inject] public HttpClient HttpClient { get; set; } = null!;
        [Inject] public NavigationManager NavManager { get; set; } = null!;

        [Parameter] public string Placa { get; set; } = null!;

        private RequestOfUpdateVehiculo _request = new();
        private string _errorMessage = string.Empty;
        private string _displayErrorClass = "d-none";

        protected override async Task OnInitializedAsync()
        {
            var vehiculo = await HttpClient.GetFromJsonAsync<ResponseOfGetVehiculos.Vehiculo>(VehiculoControllerRoutes.GetVehiculoByPlaca(Placa)) ?? new();
            _request = new()
            {
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                Year = vehiculo.Year,
                Estado = vehiculo.Estado,
                PrecioPorDia = vehiculo.PrecioPorDia
            };
        }

        private async Task HandleUpdateVehiculoAsync()
        {
            var response = await HttpClient.PutAsJsonAsync(VehiculoControllerRoutes.EditVehiculo(Placa), _request);

            if (!response.IsSuccessStatusCode)
            {
                _errorMessage = "No se pudo modificar el vehículo. Intente de nuevo más tarde";
                _displayErrorClass = string.Empty;
            }

            NavManager.NavigateTo(ClientRoutes.VehiculoListRoute);
        }
    }
}
