namespace ServiceContracts
{
    using Shared.ReservaController.Request;
    using Shared.ReservaController.Response;
    using System.Threading.Tasks;

    public interface IReservaService
    {
        Task<ResponseOfGetReservas.Reserva?> CreateReservaAsync(RequestOfCreateReserva request);
        Task DeleteReservaAsync(int id);
        Task<ResponseOfGetReservas.Reserva?> GetReservaAsync(int id);
        ResponseOfGetReservas? GetReservas(string cedula);
        Task<ResponseOfGetReservas.Reserva?> UpdateReservaAsync(int id, RequestOfUpdateReserva request);
    }
}
