namespace Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ServiceContracts;
    using Shared.Routing;
    using Shared.VehiculoController.Request;

    [Route(VehiculoControllerRoutes.Root)]
    [ApiController]
    public class VehiculoController : ControllerBase
    {
        private readonly IVehiculoService _vehiculoService;

        public VehiculoController(IVehiculoService vehiculoService)
        {
            _vehiculoService = vehiculoService;
        }

        [HttpGet]
        public IActionResult GetVehiculos([FromBody] RequestOfSearchVehiculos? request = null)
        {
            var result = _vehiculoService.GetVehiculos(request);

            if (result is null) return NoContent();

            return Ok(result);
        }

        [HttpGet("{placa}")]
        public async Task<IActionResult> GetVehiculo(string placa)
        {
            var result = await _vehiculoService.GetVehiculoAsync(placa);

            if (result is null) return NotFound("El vehículo no fue encontrado");

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehiculo([FromBody] RequestOfCreateVehiculo request)
        {
            var result = await _vehiculoService.CreateVehiculoAsync(request);

            if (result is null) return BadRequest("No se pudo crear el vehículo");

            return Ok(result);
        }

        [HttpPut("{placa}")]
        public async Task<IActionResult> UpdateVehiculo(string placa, [FromBody] RequestOfUpdateVehiculo request)
        {
            var result = await _vehiculoService.UpdateVehiculoAsync(placa, request);

            if (result is null) return BadRequest("No se pudo modificar el vehículo o no fue encontrado");

            return Ok(result);
        }

        [HttpDelete("{placa}")]
        public async Task<IActionResult> DeleteVehiculo(string placa)
        {
            await _vehiculoService.RemoveVehiculoAsync(placa);
            return Ok();
        }
    }
}
