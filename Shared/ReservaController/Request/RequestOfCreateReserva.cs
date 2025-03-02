namespace Shared.ReservaController.Request
{
    using Shared.Enums;

    public class RequestOfCreateReserva
    {
        public required string PlacaVehiculo { get; set; }
        public required EstadosReserva EstadoReserva { get; set; }
        public required float CostoTotal { get; set; }
        public required DateOnly FechaInicio { get; set; }
        public required DateOnly FechaFin { get; set; }
    }
}
