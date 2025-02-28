namespace Shared.VehiculoController.Request
{
    using Shared.Enums;

    public class RequestOfCreateVehiculo
    {
        public required string Placa { get; set; }
        public required string Marca { get; set; }
        public required string Modelo { get; set; }
        public required int Year { get; set; }
        public required EstadosVehiculo Estado { get; set; }
        public required float PrecioPorDia { get; set; }
    }
}
