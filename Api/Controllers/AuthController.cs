using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Shared.AuthController.Request;
using Shared.Routing;

namespace Api.Controllers
{
    [Route(AuthControllerRoutes.Root)]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost(AuthControllerRoutes.Register)]
        public async Task<IActionResult> Register([FromBody] RequestOfRegisterUsuario request)
        {
            var success = await _authService.Register(request);
            return success ? Ok(new { message = "User registered successfully" }) : BadRequest(new { message = "Registration failed" });
        }

        [HttpPost(AuthControllerRoutes.Login)]
        public async Task<IActionResult> Login([FromBody] RequestOfLoginUsuario request)
        {
            var result = await _authService.Login(request);
            return result is not null ? Ok(result) : Unauthorized(new { message = "Invalid credentials" });
        }

        [HttpPost(AuthControllerRoutes.RefreshToken)]
        public async Task<IActionResult> RefreshToken([FromBody] RequestOfRefreshToken request)
        {
            var result = await _authService.RefreshToken(request);
            return result is not null ? Ok(result) : Unauthorized(new { message = "Invalid refresh token" });
        }
    }
}
