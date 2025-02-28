namespace Shared.VehiculoController.Request
{
    using Shared.Enums;

    public class RequestOfUpdateVehiculo
    {
        public string? Placa { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public int Year { get; set; }
        public EstadosVehiculo Estado { get; set; }
        public float PrecioPorDia { get; set; }
    }
}
