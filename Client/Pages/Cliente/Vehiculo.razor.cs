namespace Client.Pages.Cliente
{
    using Client.Routing;
    using Microsoft.AspNetCore.Components;
    using Shared.Enums;
    using Shared.ReservaController.Request;
    using Shared.Routing;
    using Shared.VehiculoController.Response;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    public partial class Vehiculo
    {
        [Inject] public HttpClient HttpClient { get; set; } = null!;
        [Inject] public NavigationManager NavManager { get; set; } = null!;

        [Parameter] public string Placa { get; set; } = null!;

        private ResponseOfGetVehiculos.Vehiculo _vehiculo = new();
        private RequestOfCreateReserva _reservaRequest = new();
        private string _errorMessage = string.Empty;
        private string _displayErrorClass = "d-none";
        private int _dateDifference;

        protected override async Task OnInitializedAsync()
        {
            _vehiculo = await HttpClient.GetFromJsonAsync<ResponseOfGetVehiculos.Vehiculo>(VehiculoControllerRoutes.GetVehiculoByPlaca(Placa)) ?? new();
            _reservaRequest = new()
            {
                PlacaVehiculo = Placa,
                EstadoReserva = EstadosReserva.Confirmada,
                FechaInicio = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
                FechaFin = DateOnly.FromDateTime(DateTime.Today).AddDays(8)
            };
            await HandlePriceCalculationAsync();
        }

        private async Task CreateNewReservaAsync()
        {
            _displayErrorClass = "d-none";
            var response = await HttpClient.PostAsJsonAsync(ReservaControllerRoutes.CreateReservas, _reservaRequest);

            if (!response.IsSuccessStatusCode)
            {
                _errorMessage = "Hubo un problema al comparar las fechas, o ya existe una reservación de este vehículo.";
                _displayErrorClass = string.Empty;
            }

            NavManager.NavigateTo(ClientRoutes.VehiculoListRoute);
        }

        private static string FormatPriceNumberToMoneyString(float value) => string.Format("{0:C}", value);

        private async Task HandlePriceCalculationAsync()
        {
            await Task.Delay(1);
            _dateDifference = _reservaRequest.FechaFin.DayNumber - _reservaRequest.FechaInicio.DayNumber;
            _reservaRequest.CostoTotal = _vehiculo.PrecioPorDia * _dateDifference;
            StateHasChanged();
        }
    }
}
