namespace Repositories
{
    using DbContext;
    using Entities;
    using Microsoft.EntityFrameworkCore;
    using RepositoryContracts;

    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly AppDbContext _dbContext;

        public VehiculoRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<VehiculoEntity> GetVehiculos() => _dbContext.Vehiculos;

        public async Task<VehiculoEntity?> GetVehiculoAsync(string placa)
        {
            return await _dbContext.Vehiculos
                .Include(prop => prop.EstadoVehiculo)
                .FirstOrDefaultAsync(vehiculo => vehiculo.Placa == placa);
        }

        public async Task<VehiculoEntity> CreateVehiculoAsync(VehiculoEntity vehiculo)
        {
            await _dbContext.Vehiculos.AddAsync(vehiculo);
            await _dbContext.SaveChangesAsync();

            return vehiculo;
        }

        public async Task<VehiculoEntity> UpdateVehiculoAsync(VehiculoEntity vehiculo)
        {
            _dbContext.Vehiculos.Update(vehiculo);
            await _dbContext.SaveChangesAsync();

            return vehiculo;
        }

        public async Task RemoveVehiculoAsync(VehiculoEntity vehiculo)
        {
            _dbContext.Vehiculos.Remove(vehiculo);
            await _dbContext.SaveChangesAsync();
        }
    }
}
