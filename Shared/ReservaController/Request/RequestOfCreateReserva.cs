namespace Shared.ReservaController.Request
{
    using Shared.Enums;
    using System.ComponentModel.DataAnnotations;

    public class RequestOfCreateReserva
    {
        [Required(ErrorMessage = "La placa del vehículo es obligatoria.")]
        public string PlacaVehiculo { get; set; }

        [Required(ErrorMessage = "Este valor no es válido")]
        [EnumDataType(typeof(EstadosReserva))]
        public EstadosReserva EstadoReserva { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es requerida.")]
        [DataType(DataType.Date)]
        public DateOnly FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha fin es requerida.")]
        [DataType(DataType.Date)]
        public DateOnly FechaFin { get; set; }
        public float CostoTotal { get; set; }
    }
}
