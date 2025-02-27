namespace Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Vehiculo")]
    public class VehiculoEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Marca { get; set; } = null!;

        [Required]
        public string Modelo { get; set; } = null!;

        [Required]
        public int Year { get; set; }

        [Required]
        public string Placa { get; set; } = null!;

        [Required]
        public int EstadoVehiculoId { get; set; }
        [ForeignKey(nameof(EstadoVehiculoId))]
        public EstadoVehiculoEntity EstadoVehiculo { get; set; } = null!;

        [Required]
        [DataType(DataType.Currency)]
        public float PrecioPorDia { get; set; }
    }
}
