namespace Client.Pages.Administrador
{
    using Client.Routing;
    using Microsoft.AspNetCore.Components;
    using Shared.Enums;
    using Shared.Routing;
    using System.Net.Http.Json;

    public partial class EditReserva
    {
        [Inject] public HttpClient HttpClient { get; set; } = null!;
        [Inject] public NavigationManager NavManager { get; set; } = null!;

        [Parameter] public int Id { get; set; }

        private EstadosReserva Estado;

        private async Task ChangeEstadoReservaAsync()
        {
            var response = await HttpClient.PutAsJsonAsync(ReservaControllerRoutes.ChangeEstadoReserva(Id), Estado);

            if (response.IsSuccessStatusCode) NavManager.NavigateTo(ClientRoutes.AdminReservasRoute);
        }
    }
}
