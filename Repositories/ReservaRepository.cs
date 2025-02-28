namespace Repositories
{
    using DbContext;
    using Entities;
    using Microsoft.EntityFrameworkCore;
    using RepositoryContracts;

    public class ReservaRepository : IReservaRepository
    {
        private readonly AppDbContext _dbContext;

        public ReservaRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<ReservaEntity> GetReservas() => _dbContext.Reservas;

        public async Task<ReservaEntity?> GetReservaAsync(int id)
        {
            return await _dbContext.Reservas
                .Include(prop => prop.Vehiculo)
                .Include(prop => prop.Cliente)
                .Include(prop => prop.EstadoReseva)
                .FirstOrDefaultAsync(reserva => reserva.Id == id);
        }

        public async Task<ReservaEntity> CreateReservaAsync(ReservaEntity reserva)
        {
            await _dbContext.Reservas.AddAsync(reserva);
            await _dbContext.SaveChangesAsync();

            return reserva;
        }

        public async Task<ReservaEntity> UpdateReservaAsync(ReservaEntity reserva)
        {
            _dbContext.Reservas.Update(reserva);
            await _dbContext.SaveChangesAsync();

            return reserva;
        }

        public async Task RemoveReservaAsync(ReservaEntity reserva)
        {
            _dbContext.Reservas.Remove(reserva);
            await _dbContext.SaveChangesAsync();
        }
    }
}
