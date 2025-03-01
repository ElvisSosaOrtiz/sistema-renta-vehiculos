namespace Shared.ReservaController.Request
{
    using Shared.Enums;

    public class RequestOfUpdateReserva
    {
        public string? CedulaCliente { get; set; }
        public string? PlacaVehiculo { get; set; }
        public EstadosReserva EstadoReserva { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
    }
}
