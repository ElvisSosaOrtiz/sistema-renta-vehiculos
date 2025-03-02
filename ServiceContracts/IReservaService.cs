namespace ServiceContracts
{
    using Shared.Enums;
    using Shared.ReservaController.Request;
    using Shared.ReservaController.Response;
    using System.Threading.Tasks;

    public interface IReservaService
    {
        Task<ResponseOfGetReservas.Reserva?> ChangeEstadoReservaAsync(int id, EstadosReserva estado);
        Task<ResponseOfGetReservas.Reserva?> CreateReservaAsync(RequestOfCreateReserva request, string cedula, string userId);
        Task DeleteReservaAsync(int id);
        Task<ResponseOfGetReservas.Reserva?> GetReservaAsync(int id);
        ResponseOfGetReservas? GetReservas(string cedula);
        Task<ResponseOfGetReservas.Reserva?> UpdateReservaAsync(int id, RequestOfUpdateReserva request);
    }
}
