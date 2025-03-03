namespace Client.Pages.Administrador
{
    using Client.Routing;
    using Microsoft.AspNetCore.Components;
    using Shared.Routing;
    using Shared.VehiculoController.Response;
    using System.Net.Http.Json;

    public partial class DeleteVehiculo
    {
        [Inject] public HttpClient HttpClient { get; set; } = null!;
        [Inject] public NavigationManager NavManager { get; set; } = null!;

        [Parameter] public string Placa { get; set; } = null!;

        private ResponseOfGetVehiculos.Vehiculo _vehiculo = new();
        private string NombreVehiculo = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            _vehiculo = await HttpClient.GetFromJsonAsync<ResponseOfGetVehiculos.Vehiculo>(VehiculoControllerRoutes.GetVehiculoByPlaca(Placa)) ?? new();
            NombreVehiculo = $"{_vehiculo.Marca} {_vehiculo.Modelo}";
        }

        private async Task RemoveVehiculoAsync()
        {
            var response = await HttpClient.DeleteAsync(VehiculoControllerRoutes.RemoveVehiculo(Placa));

            if (response.IsSuccessStatusCode) NavManager.NavigateTo(ClientRoutes.VehiculoListRoute);
        }
    }
}
