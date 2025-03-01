namespace Services
{
    using Entities;
    using Microsoft.Extensions.Logging;
    using RepositoryContracts;
    using ServiceContracts;
    using Shared.Enums;
    using Shared.ReservaController.Request;
    using Shared.ReservaController.Response;
    using static Shared.ReservaController.Response.ResponseOfGetReservas;

    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly ILogger<ReservaService> _logger;

        public ReservaService(
            IReservaRepository reservaRepository,
            ILogger<ReservaService> logger)
        {
            _reservaRepository = reservaRepository;
            _logger = logger;
        }

        public ResponseOfGetReservas? GetReservas(string cedula)
        {
            try
            {
                var reservas = _reservaRepository.GetReservas();

                if (reservas is null || !reservas.Any()) return null;

                return new()
                {
                    Reservas = reservas
                        .Where(reserva => reserva.CedulaUsuario == cedula)
                        .Select(reserva => new ResponseOfGetReservas.Reserva
                    {
                        Id = reserva.Id,
                        Cliente = new()
                        {
                            Cedula = reserva.CedulaUsuario,
                            Nombre = reserva.Cliente.Nombre,
                            Apellido = reserva.Cliente.Apellido,
                            Telefono = reserva.Cliente.Telefono,
                            Direccion = reserva.Cliente.Direccion,
                            Correo = reserva.Cliente.Correo
                        },
                        Vehiculo = new()
                        {
                            Placa = reserva.PlacaVehiculo,
                            Marca = reserva.Vehiculo.Marca,
                            Modelo = reserva.Vehiculo.Modelo,
                            Year = reserva.Vehiculo.Year,
                            Estado = (EstadosVehiculo)reserva.Vehiculo.EstadoVehiculoId,
                            PrecioPorDia = reserva.Vehiculo.PrecioPorDia
                        },
                        Estado = (EstadosReserva)reserva.IdEstadoReserva,
                        CostoTotal = reserva.CostoTotal,
                        FechaInicio = reserva.FechaInicio,
                        FechaFin = reserva.FechaFin
                    })
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<Reserva?> GetReservaAsync(int id)
        {
            try
            {
                var reserva = await _reservaRepository.GetReservaAsync(id);

                if (reserva is null)
                {
                    _logger.LogError("Could not find reserva");
                    return null;
                }

                return new()
                {
                    Id = reserva.Id,
                    Cliente = new()
                    {
                        Cedula = reserva.CedulaUsuario,
                        Nombre = reserva.Cliente.Nombre,
                        Apellido = reserva.Cliente.Apellido,
                        Telefono = reserva.Cliente.Telefono,
                        Direccion = reserva.Cliente.Direccion,
                        Correo = reserva.Cliente.Correo
                    },
                    Vehiculo = new()
                    {
                        Placa = reserva.PlacaVehiculo,
                        Marca = reserva.Vehiculo.Marca,
                        Modelo = reserva.Vehiculo.Modelo,
                        Year = reserva.Vehiculo.Year,
                        Estado = (EstadosVehiculo)reserva.Vehiculo.EstadoVehiculoId,
                        PrecioPorDia = reserva.Vehiculo.PrecioPorDia
                    },
                    Estado = (EstadosReserva)reserva.IdEstadoReserva,
                    CostoTotal = reserva.CostoTotal,
                    FechaInicio = reserva.FechaInicio,
                    FechaFin = reserva.FechaFin
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<Reserva?> CreateReservaAsync(RequestOfCreateReserva request)
        {
            try
            {
                var existingReserva = GetReservas(request.CedulaCliente)?.Reservas
                    .FirstOrDefault(reserva => reserva.Vehiculo.Placa == request.PlacaVehiculo);

                if (existingReserva is not null)
                {
                    _logger.LogError("Reserva of this vehiculo already exists");
                    return null;
                }

                if (Convert.ToDateTime(request.FechaInicio) > DateTime.Today)
                {
                    _logger.LogError("FechaInicio should be greater than today");
                    return null;
                }

                if (Convert.ToDateTime(request.FechaFin) > DateTime.Today && request.FechaFin > request.FechaInicio)
                {
                    _logger.LogError("FechaFin should be greater than today and greater than FechaInicio");
                    return null;
                }

                var reservaEntity = new ReservaEntity
                {
                    CedulaUsuario = request.CedulaCliente,
                    PlacaVehiculo = request.PlacaVehiculo,
                    IdEstadoReserva = (int)request.EstadoReserva,
                    CostoTotal = request.CostoTotal,
                    FechaInicio = request.FechaInicio,
                    FechaFin = request.FechaFin
                };

                var reserva = await _reservaRepository.CreateReservaAsync(reservaEntity);

                if (reserva is null)
                {
                    _logger.LogError("Could not create reserva");
                    return null;
                }

                return new()
                {
                    Id = reserva.Id,
                    Cliente = new() { Cedula = reserva.CedulaUsuario },
                    Vehiculo = new() { Placa = reserva.PlacaVehiculo },
                    Estado = (EstadosReserva)reserva.IdEstadoReserva,
                    CostoTotal = reserva.CostoTotal,
                    FechaInicio = request.FechaInicio,
                    FechaFin = request.FechaFin
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<Reserva?> UpdateReservaAsync(int id, RequestOfUpdateReserva request)
        {
            try
            {
                var reservaEntity = await _reservaRepository.GetReservaAsync(id);

                if (reservaEntity is null)
                {
                    _logger.LogError("Could not find reserva to update");
                    return null;
                }

                if (!string.IsNullOrEmpty(request.CedulaCliente)) reservaEntity.CedulaUsuario = request.CedulaCliente;
                if (!string.IsNullOrEmpty(request.PlacaVehiculo)) reservaEntity.PlacaVehiculo = request.PlacaVehiculo;
                if (request.EstadoReserva > 0) reservaEntity.IdEstadoReserva = (int)request.EstadoReserva;
                if (Convert.ToDateTime(request.FechaInicio) > DateTime.Today) reservaEntity.FechaInicio = request.FechaInicio;
                if (Convert.ToDateTime(request.FechaFin) > DateTime.Today && request.FechaFin > request.FechaInicio) reservaEntity.FechaFin = request.FechaFin;

                var reserva = await _reservaRepository.UpdateReservaAsync(reservaEntity);

                if (reserva is null)
                {
                    _logger.LogError("Could not update reserva");
                    return null;
                }

                return new()
                {
                    Id = reserva.Id,
                    Cliente = new() { Cedula = reserva.CedulaUsuario },
                    Vehiculo = new() { Placa = reserva.PlacaVehiculo },
                    Estado = (EstadosReserva)reserva.IdEstadoReserva,
                    FechaInicio = reserva.FechaInicio,
                    FechaFin = reserva.FechaFin
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task DeleteReservaAsync(int id)
        {
            try
            {
                var reserva = await _reservaRepository.GetReservaAsync(id);

                if (reserva is null)
                {
                    _logger.LogError("Could not find reserva to delete");
                    return;
                }

                await _reservaRepository.RemoveReservaAsync(reserva);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
