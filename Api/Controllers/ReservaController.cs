namespace Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ServiceContracts;
    using Shared.ReservaController.Request;
    using Shared.Routing;

    [Route(ReservaControllerRoutes.Root)]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpGet]
        public IActionResult GetReservas([FromQuery] string cedula)
        {
            var result = _reservaService.GetReservas(cedula);

            if (result is null) return NoContent();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReserva(int id)
        {
            var result = await _reservaService.GetReservaAsync(id);

            if (result is null) return NotFound("La reserva no fue encontrada");

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReserva([FromBody] RequestOfCreateReserva request)
        {
            var result = await _reservaService.CreateReservaAsync(request);

            if (result is null) return BadRequest("No se pudo crear la reserva");

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReserva(int id, [FromBody] RequestOfUpdateReserva request)
        {
            var result = await _reservaService.UpdateReservaAsync(id, request);

            if (result is null) return BadRequest("No se pudo modificar la reserva o no fue encontrada");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            await _reservaService.DeleteReservaAsync(id);
            return Ok();
        }
    }
}
