namespace Shared.VehiculoController.Request
{
    using Shared.Enums;
    using System.ComponentModel.DataAnnotations;

    public class RequestOfUpdateVehiculo
    {
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public int Year { get; set; }

        [EnumDataType(typeof(EstadosVehiculo))]
        public EstadosVehiculo Estado { get; set; }

        [DataType(DataType.Currency)]
        public float PrecioPorDia { get; set; }
    }
}
