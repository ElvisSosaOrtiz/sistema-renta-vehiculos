namespace Client.Pages.CommonPages
{
    using Client.LocalServices;
    using Client.Routing;
    using Microsoft.AspNetCore.Components;
    using Shared.Enums;
    using Shared.Routing;
    using Shared.VehiculoController.Request;
    using Shared.VehiculoController.Response;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    public partial class VehiculosList
    {
        [Inject] public HttpClient HttpClient { get; set; } = null!;
        [Inject] public NavigationManager NavManager { get; set; } = null!;
        [Inject] public CustomAuthStateProvider AuthStateProvider { get; set; } = null!;

        private ResponseOfGetVehiculos _vehiculosResponse = new();
        private List<ResponseOfGetVehiculos.Vehiculo> _vehiculos = [];
        private RequestOfSearchVehiculos _requestSearch = new();
        private bool IsAdmin;

        protected override async Task OnInitializedAsync()
        {
            await GetVehiculosWithFiltersAsync();
            IsAdmin = (await AuthStateProvider.GetAuthenticationStateAsync()).User.IsInRole(nameof(UserRoles.Administrador));
        }

        private async Task GetVehiculosWithFiltersAsync()
        {
            var response = await HttpClient.GetAsync(VehiculoControllerRoutes.GetVehiculos(_requestSearch));

            if (response.IsSuccessStatusCode)
            {
                _vehiculosResponse = await response.Content.ReadFromJsonAsync<ResponseOfGetVehiculos>() ?? new();
                _vehiculos = _vehiculosResponse.Vehiculos.ToList();
            }

            StateHasChanged();
        }

        private static string FormatPriceNumberToMoneyString(float value) => string.Format("{0:C}", value);

        private void NavigateToVehiculoDetails(ResponseOfGetVehiculos.Vehiculo vehiculo)
        {
            if (IsAdmin) NavManager.NavigateTo(ClientRoutes.AdminEditVehiculoRoute(vehiculo.Placa));
            else NavManager.NavigateTo(ClientRoutes.VehiculoRoute(vehiculo.Placa));
        }
    }
}
