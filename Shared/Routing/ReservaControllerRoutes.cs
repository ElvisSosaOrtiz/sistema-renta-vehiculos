namespace Shared.Routing
{
    public class ReservaControllerRoutes
    {
        public const string Root = "api/reserva";
        public const string EstadoReserva = "estado-reserva";

        public static string GetReservas(string cedula) => $"{Root}?cedula={cedula}";
        public static string CreateReservas => $"{Root}";
        public static string ChangeEstadoReserva(int id) => $"{Root}/{EstadoReserva}/{id}";
    }
}
