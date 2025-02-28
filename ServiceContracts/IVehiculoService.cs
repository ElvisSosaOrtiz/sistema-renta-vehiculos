namespace ServiceContracts
{
    using Shared.VehiculoController.Request;
    using Shared.VehiculoController.Response;
    using System.Threading.Tasks;

    public interface IVehiculoService
    {
        Task<ResponseOfGetVehiculos.Vehiculo?> CreateVehiculoAsync(RequestOfCreateVehiculo request);
        Task<ResponseOfGetVehiculos.Vehiculo?> GetVehiculoAsync(string placa);
        ResponseOfGetVehiculos? GetVehiculos(RequestOfSearchVehiculos? request = null);
        Task RemoveVehiculoAsync(string placa);
        Task<ResponseOfGetVehiculos.Vehiculo?> UpdateVehiculoAsync(string placa, RequestOfUpdateVehiculo request);
    }
}
