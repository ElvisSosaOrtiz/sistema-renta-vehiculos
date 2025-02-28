namespace Services
{
    using Entities;
    using Microsoft.Extensions.Logging;
    using RepositoryContracts;
    using ServiceContracts;
    using Shared.Enums;
    using Shared.VehiculoController.Request;
    using Shared.VehiculoController.Response;
    using System.Linq;

    public class VehiculoService : IVehiculoService
    {
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly ILogger<VehiculoService> _logger;

        public VehiculoService(
            IVehiculoRepository vehiculoRepository,
            ILogger<VehiculoService> logger)
        {
            _vehiculoRepository = vehiculoRepository;
            _logger = logger;
        }

        public ResponseOfGetVehiculos? GetVehiculos(RequestOfSearchVehiculos? request = null)
        {
            try
            {
                var vehiculos = _vehiculoRepository.GetVehiculos();

                if (vehiculos is null || !vehiculos.Any()) return null;

                if (request is not null)
                {
                    if (!string.IsNullOrEmpty(request.Marca))
                        vehiculos = vehiculos.Where(vehiculo => vehiculo.Marca.Contains(request.Marca));

                    if (!string.IsNullOrEmpty(request.Modelo))
                        vehiculos = vehiculos.Where(vehiculo => vehiculo.Modelo.Contains(request.Modelo));

                    if (request.Year > 0)
                        vehiculos = vehiculos.Where(vehiculo => vehiculo.Year == request.Year);

                    if (request.Estado > 0)
                        vehiculos = vehiculos.Where(vehiculo => vehiculo.EstadoVehiculo.Nombre.Contains(Enum.GetName(request.Estado)!));

                    if (!vehiculos.Any()) return null;
                }

                return new()
                {
                    Vehiculos = vehiculos.Select(vehiculo => new ResponseOfGetVehiculos.Vehiculo
                    {
                        Placa = vehiculo.Placa,
                        Marca = vehiculo.Marca,
                        Modelo = vehiculo.Modelo,
                        Year = vehiculo.Year,
                        Estado = Enum.Parse<EstadosVehiculo>(vehiculo.EstadoVehiculo.Nombre),
                        PrecioPorDia = vehiculo.PrecioPorDia
                    })
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<ResponseOfGetVehiculos.Vehiculo?> GetVehiculoAsync(string placa)
        {
            try
            {
                var vehiculo = await _vehiculoRepository.GetVehiculoAsync(placa);

                if (vehiculo is null)
                {
                    _logger.LogError("Could not find vehiculo");
                    return null;
                }

                return new()
                {
                    Placa = vehiculo.Placa,
                    Marca = vehiculo.Marca,
                    Modelo = vehiculo.Modelo,
                    Year = vehiculo.Year,
                    Estado = Enum.Parse<EstadosVehiculo>(vehiculo.EstadoVehiculo.Nombre),
                    PrecioPorDia = vehiculo.PrecioPorDia
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<ResponseOfGetVehiculos.Vehiculo?> CreateVehiculoAsync(RequestOfCreateVehiculo request)
        {
            try
            {
                var vehiculoEntity = new VehiculoEntity
                {
                    Placa = request.Placa,
                    Marca = request.Marca,
                    Modelo = request.Modelo,
                    Year = request.Year,
                    EstadoVehiculoId = (int)request.Estado,
                    PrecioPorDia = request.PrecioPorDia
                };

                var vehiculo = await _vehiculoRepository.CreateVehiculoAsync(vehiculoEntity);

                if (vehiculo is null)
                {
                    _logger.LogError("Could not create vehiculo");
                    return null;
                }

                return new()
                {
                    Placa = vehiculo.Placa,
                    Marca = vehiculo.Marca,
                    Modelo = vehiculo.Modelo,
                    Year = vehiculo.Year,
                    Estado = (EstadosVehiculo)vehiculo.EstadoVehiculoId,
                    PrecioPorDia = vehiculo.PrecioPorDia
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<ResponseOfGetVehiculos.Vehiculo?> UpdateVehiculoAsync(string placa, RequestOfUpdateVehiculo request)
        {
            try
            {
                var vehiculoEntity = await _vehiculoRepository.GetVehiculoAsync(placa);

                if (vehiculoEntity is null)
                {
                    _logger.LogError("Could not find vehiculo to update");
                    return null;
                }

                if (!string.IsNullOrEmpty(request.Marca)) vehiculoEntity.Marca = request.Marca;
                if (!string.IsNullOrEmpty(request.Modelo)) vehiculoEntity.Modelo = request.Modelo;
                if (request.Year > 0) vehiculoEntity.Year = request.Year;
                if (request.Estado > 0) vehiculoEntity.EstadoVehiculoId = (int)request.Estado;
                if (request.PrecioPorDia > 0) vehiculoEntity.PrecioPorDia = request.PrecioPorDia;

                var vehiculo = await _vehiculoRepository.UpdateVehiculoAsync(vehiculoEntity);

                if (vehiculo is null)
                {
                    _logger.LogError("Could not update vehiculo");
                    return null;
                }

                return new()
                {
                    Placa = placa,
                    Marca = vehiculo.Marca,
                    Modelo = vehiculo.Modelo,
                    Year = vehiculo.Year,
                    Estado = (EstadosVehiculo)vehiculo.EstadoVehiculoId,
                    PrecioPorDia = vehiculo.PrecioPorDia
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task RemoveVehiculoAsync(string placa)
        {
            try
            {
                var vehiculoEntity = await _vehiculoRepository.GetVehiculoAsync(placa);

                if (vehiculoEntity is null)
                {
                    _logger.LogError("Could not find vehiculo to delete");
                    return;
                }

                await _vehiculoRepository.RemoveVehiculoAsync(vehiculoEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
