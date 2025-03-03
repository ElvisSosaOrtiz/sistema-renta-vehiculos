namespace Api.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ServiceContracts;
    using Shared.Enums;
    using Shared.ReservaController.Request;
    using Shared.Routing;
    using System.Security.Claims;

    [Authorize(AuthenticationSchemes = "Bearer")]
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
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReserva(int id)
        {
            var result = await _reservaService.GetReservaAsync(id);

            if (result is null) return NotFound("La reserva no fue encontrada");

            return Ok(result);
        }

        [Authorize(Roles = nameof(UserRoles.Cliente))]
        [HttpPost]
        public async Task<IActionResult> CreateReserva([FromBody] RequestOfCreateReserva request)
        {
            string cedula = User.FindFirstValue("nameid")!;
            string userId = User.FindFirstValue("sub")!;
            var result = await _reservaService.CreateReservaAsync(request, cedula, userId);

            if (result is null) return BadRequest("No se pudo crear la reserva");

            return Ok(result);
        }

        [Authorize(Roles = nameof(UserRoles.Cliente))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReserva(int id, [FromBody] RequestOfUpdateReserva request)
        {
            var result = await _reservaService.UpdateReservaAsync(id, request);

            if (result is null) return BadRequest("No se pudo modificar la reserva o no fue encontrada");

            return Ok(result);
        }

        [Authorize(Roles = nameof(UserRoles.Administrador))]
        [HttpPut(ReservaControllerRoutes.EstadoReserva + "/{id}")]
        public async Task<IActionResult> ChangeEstadoReserva(int id, [FromBody] EstadosReserva estado)
        {
            var result = await _reservaService.ChangeEstadoReservaAsync(id, estado);

            if (result is null) return BadRequest("No se pudo modificar el estado de la reserva o no fue encontrada");

            return Ok(result);
        }

        [Authorize(Roles = nameof(UserRoles.Cliente))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            await _reservaService.DeleteReservaAsync(id);
            return Ok();
        }
    }
}
