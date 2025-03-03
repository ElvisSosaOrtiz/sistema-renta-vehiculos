namespace Client.Routing
{
    public class ClientRoutes
    {
        public const string LoginRoute = "/login";
        public const string VehiculoListRoute = "/vehiculos";
        public const string AdminReservasRoute = "/admin-reservas";

        public static string VehiculoRoute(string placa) => $"{VehiculoListRoute}/{placa}";
        public static string AdminEditVehiculoRoute(string placa) => $"/admin-edit-vehiculo/{placa}";
        public static string AdminDeleteVehiculoRoute(string placa) => $"/admin-delete-vehiculo/{placa}";
        public static string AdminEditEstadoReservaRoute(int id) => $"/admin-edit-estado-reserva/{id}";
    }
}
