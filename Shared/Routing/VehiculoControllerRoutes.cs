namespace Shared.Routing
{
    using Shared.VehiculoController.Request;
    using System;

    public class VehiculoControllerRoutes
    {
        public const string Root = "api/vehiculo";

        public static string GetVehiculoByPlaca(string placa) => $"{Root}/{placa}";
        public static string AddVehiculo => $"{Root}";
        public static string? EditVehiculo(string placa) => $"{Root}/{placa}";
        public static string? RemoveVehiculo(string placa) => $"{Root}/{placa}";

        public static string GetVehiculos(RequestOfSearchVehiculos request)
        {
            if (request == new RequestOfSearchVehiculos()) return $"{Root}";

            return $"{Root}?{(string.IsNullOrEmpty(request.Marca) ? string.Empty : $"marca={request.Marca}")}" +
                $"{(string.IsNullOrEmpty(request.Modelo) ? string.Empty : $"&modelo={request.Modelo}")}" +
                $"{(request.Year <= 1970 ? string.Empty : $"&year={request.Year}")}" +
                $"{(request.Estado <= 0 || (int)request.Estado > 3 ? string.Empty : $"&estado={request.Estado}")}";
        }
    }
}
