namespace RepositoryContracts
{
    using Entities;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IVehiculoRepository
    {
        Task<VehiculoEntity> CreateVehiculoAsync(VehiculoEntity vehiculo);
        Task<VehiculoEntity?> GetVehiculoAsync(string placa);
        IQueryable<VehiculoEntity> GetVehiculos();
        Task RemoveVehiculoAsync(VehiculoEntity vehiculo);
        Task<VehiculoEntity> UpdateVehiculoAsync(VehiculoEntity vehiculo);
    }
}
