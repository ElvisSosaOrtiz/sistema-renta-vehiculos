namespace Shared.VehiculoController.Request
{
    using Shared.Enums;
    using System.ComponentModel.DataAnnotations;

    public class RequestOfCreateVehiculo
    {
        [Required(ErrorMessage = "La placa es obligatoria.")]
        public string Placa { get; set; } = null!;

        [Required(ErrorMessage = "La marca es obligatoria.")]
        public string Marca { get; set; } = null!;

        [Required(ErrorMessage = "El modelo es obligatorio.")]
        public string Modelo { get; set; } = null!;

        [Required(ErrorMessage = "El año es obligatorio.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [EnumDataType(typeof(EstadosVehiculo))]
        public EstadosVehiculo Estado { get; set; }

        [Required(ErrorMessage = "El precio por día es obligatorio.")]
        [DataType(DataType.Currency)]
        public float PrecioPorDia { get; set; }
    }
}
