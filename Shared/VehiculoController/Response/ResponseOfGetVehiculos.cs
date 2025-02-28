namespace Shared.VehiculoController.Response
{
    using Shared.Enums;

    public class ResponseOfGetVehiculos
    {
        public IEnumerable<Vehiculo> Vehiculos { get; set; } = [];

        public class Vehiculo
        {
            public string Placa { get; set; } = null!;
            public string Marca { get; set; } = null!;
            public string Modelo { get; set; } = null!;
            public int Year { get; set; }
            public EstadosVehiculo Estado { get; set; }
            public float PrecioPorDia { get; set; }
        }
    }
}
