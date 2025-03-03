namespace Client.Pages.Administrador
{
    using Microsoft.AspNetCore.Components;
    using Shared.ReservaController.Response;
    using Shared.Routing;
    using System.Net.Http.Json;

    public partial class ConsultarReservasCliente
    {
        [Inject] public HttpClient HttpClient { get; set; } = null!;
        [Inject] public NavigationManager NavManager { get; set; } = null!;

        private ResponseOfGetReservas _reservaResponse = new();
        private string Cedula = string.Empty;

        private async Task GetReservasByCedulaAsync()
        {
            _reservaResponse = await HttpClient.GetFromJsonAsync<ResponseOfGetReservas>(ReservaControllerRoutes.GetReservas(Cedula)) ?? new();
        }

        private static string FormatPriceNumberToMoneyString(float value) => string.Format("{0:C}", value);
    }
}
