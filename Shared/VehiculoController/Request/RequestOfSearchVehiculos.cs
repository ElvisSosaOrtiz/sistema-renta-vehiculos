namespace Shared.VehiculoController.Request
{
    using Shared.Enums;

    public class RequestOfSearchVehiculos
    {
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public int Year { get; set; }
        public EstadosVehiculo Estado { get; set; }
    }
}
