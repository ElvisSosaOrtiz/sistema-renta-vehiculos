namespace RepositoryContracts
{
    using Entities;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IReservaRepository
    {
        Task<ReservaEntity> CreateReservaAsync(ReservaEntity reserva);
        Task<ReservaEntity?> GetReservaAsync(int id);
        IQueryable<ReservaEntity> GetReservas();
        Task RemoveReservaAsync(ReservaEntity reserva);
        Task<ReservaEntity> UpdateReservaAsync(ReservaEntity reserva);
    }
}
