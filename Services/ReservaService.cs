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
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly ILogger<ReservaService> _logger;

        public ReservaService(
            IReservaRepository reservaRepository,
            IVehiculoRepository vehiculoRepository,
            ILogger<ReservaService> logger)
        {
            _reservaRepository = reservaRepository;
            _vehiculoRepository = vehiculoRepository;
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
                        .Where(reserva => reserva.Cliente.Cedula == cedula)
                        .Select(reserva => new Reserva
                        {
                        Id = reserva.Id,
                        Cliente = new()
                        {
                            Cedula = reserva.Cliente.Cedula,
                            Nombre = reserva.Cliente.Nombre,
                            Apellido = reserva.Cliente.Apellido,
                            Telefono = reserva.Cliente.PhoneNumber!,
                            Direccion = reserva.Cliente.Direccion,
                            Correo = reserva.Cliente.Email!
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
                        Cedula = reserva.Cliente.Cedula,
                        Nombre = reserva.Cliente.Nombre,
                        Apellido = reserva.Cliente.Apellido,
                        Telefono = reserva.Cliente.PhoneNumber!,
                        Direccion = reserva.Cliente.Direccion,
                        Correo = reserva.Cliente.Email!
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

        public async Task<Reserva?> CreateReservaAsync(RequestOfCreateReserva request, string cedula, string userId)
        {
            try
            {
                var existingReserva = GetReservas(cedula)?.Reservas
                    .FirstOrDefault(reserva => reserva.Vehiculo.Placa == request.PlacaVehiculo);

                var vehiculo = await _vehiculoRepository.GetVehiculoAsync(request.PlacaVehiculo);
                if (vehiculo is null)
                {
                    _logger.LogError("This vehiculo does not exist");
                    return null;
                }

                if (existingReserva is not null)
                {
                    _logger.LogError("Reserva of this vehiculo already exists");
                    return null;
                }

                if (request.FechaInicio.ToDateTime(TimeOnly.FromDateTime(DateTime.Now)) <= DateTime.Now)
                {
                    _logger.LogError("FechaInicio should be greater than today");
                    return null;
                }

                if (request.FechaFin <= request.FechaInicio)
                {
                    _logger.LogError("FechaFin should be greater than FechaInicio");
                    return null;
                }

                var reservaEntity = new ReservaEntity
                {
                    UserId = userId,
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
                    Cliente = new() { Cedula = cedula },
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

                if (request.FechaInicio.ToDateTime(TimeOnly.FromDateTime(DateTime.Now)) <= DateTime.Today || request.FechaInicio >= request.FechaFin)
                {
                    _logger.LogError("FechaInicio cannot be less or equal than Today or greater or equal than FechaFin");
                    return null;
                } 

                if (request.FechaFin <= request.FechaInicio)
                {
                    _logger.LogError("FechaFin cannot be less or equal than FechaInicio");
                    return null;
                }

                reservaEntity.FechaInicio = request.FechaInicio;
                reservaEntity.FechaFin = request.FechaFin;

                var reserva = await _reservaRepository.UpdateReservaAsync(reservaEntity);

                if (reserva is null)
                {
                    _logger.LogError("Could not update reserva");
                    return null;
                }

                return new()
                {
                    Id = reserva.Id,
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

        public async Task<Reserva?> ChangeEstadoReservaAsync(int id, EstadosReserva estado)
        {
            try
            {
                var reservaEntity = await _reservaRepository.GetReservaAsync(id);

                if (reservaEntity is null)
                {
                    _logger.LogError("Could not find reserva to change estado");
                    return null;
                }

                if (estado > 0) reservaEntity.IdEstadoReserva = (int)estado;

                var reserva = await _reservaRepository.UpdateReservaAsync(reservaEntity);

                if (reserva is null)
                {
                    _logger.LogError("Could not change estado reserva");
                    return null;
                }

                return new()
                {
                    Id = reserva.Id,
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
