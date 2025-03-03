namespace Client.Pages.Cliente
{
    using Client.LocalServices;
    using Microsoft.AspNetCore.Components;
    using Shared.ReservaController.Response;
    using Shared.Routing;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    public partial class ReservaList
    {
        [Inject] public HttpClient HttpClient { get; set; } = null!;
        [Inject] public CustomAuthStateProvider AuthStateProvider { get; set; } = null!;

        private ResponseOfGetReservas _reservaResponse = new();

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var cedula = authState.User.Claims.First(claim => claim.Type == "nameid").Value;
            Console.WriteLine($"Cedula: {cedula}");
            var response = await HttpClient.GetAsync(ReservaControllerRoutes.GetReservas(cedula));

            if (response.IsSuccessStatusCode)
            {
                _reservaResponse = await response.Content.ReadFromJsonAsync<ResponseOfGetReservas>() ?? new();
            }
        }

        private static string FormatPriceNumberToMoneyString(float value) => string.Format("{0:C}", value);
    }
}
