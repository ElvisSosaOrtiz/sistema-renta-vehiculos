namespace Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Reserva")]
    public class ReservaEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CedulaCliente { get; set; } = null!;
        [ForeignKey(nameof(CedulaCliente))]
        public ClienteEntity Cliente { get; set; } = null!;

        [Required]
        public int IdVehiculo { get; set; }
        [ForeignKey(nameof(IdVehiculo))]
        public VehiculoEntity Vehiculo { get; set; } = null!;

        [Required]
        public int IdEstadoReserva { get; set; }
        [ForeignKey(nameof(IdEstadoReserva))]
        public EstadoReservaEntity EstadoReseva { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        public DateOnly FechaInicio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly FechaFin { get; set; }
    }
}
