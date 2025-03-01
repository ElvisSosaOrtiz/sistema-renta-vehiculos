namespace Shared.ReservaController.Response
{
    using Shared.Enums;
    using static Shared.VehiculoController.Response.ResponseOfGetVehiculos;

    public class ResponseOfGetReservas
    {
        public IEnumerable<Reserva> Reservas { get; set; } = [];

        public class Reserva
        {
            public int Id { get; set; }
            public Cliente Cliente { get; set; } = null!;
            public Vehiculo Vehiculo { get; set; } = null!;
            public EstadosReserva Estado { get; set; }
            public float CostoTotal { get; set; }
            public DateOnly FechaInicio { get; set; }
            public DateOnly FechaFin { get; set; }
        }

        public class Cliente
        {
            public string Cedula { get; set; } = null!;
            public string Nombre { get; set; } = null!;
            public string Apellido { get; set; } = null!;
            public string Telefono { get; set; } = null!;
            public string Direccion { get; set; } = null!;
            public string Correo { get; set; } = null!;
        }
    }
}
