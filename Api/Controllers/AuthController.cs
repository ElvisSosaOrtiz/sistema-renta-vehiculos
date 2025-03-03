using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared.AuthController.Request;
using Shared.AuthController.Response;
using Shared.Enums;
using Shared.Routing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Api.Controllers
{
    [Route(AuthControllerRoutes.Root)]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<UsuarioEntity> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<UsuarioEntity> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost(AuthControllerRoutes.Register)]
        public async Task<IActionResult> Register([FromBody] RequestOfRegisterUsuario request)
        {
            var existingUser = _userManager.Users
                .Where(user => user.Email == request.Correo || user.Cedula == request.Cedula)
                .FirstOrDefault();
            if (existingUser is not null) return BadRequest("Algunos datos introducidos como el correo o la cédula ya existen");

            var usuario = new UsuarioEntity
            {
                Cedula = request.Cedula,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                UserName = request.UserName,
                Direccion = request.Direccion,
                PhoneNumber = request.Telefono,
                Email = request.Correo,
                PasswordHash = request.Password
            };
            var result = await _userManager.CreateAsync(usuario, request.Password);

            if (!result.Succeeded) return BadRequest(result.Errors.Select(error => new ResponseOfRegisterError
            {
                Code = error.Code,
                Description = error.Description
            }).ToList());

            await _userManager.AddToRoleAsync(usuario, nameof(UserRoles.Cliente));

            return Ok(new { message = "User registered successfully!" });
        }

        [HttpPost(AuthControllerRoutes.Login)]
        public async Task<IActionResult> Login([FromBody] RequestOfLoginUsuario request)
        {
            var usuario = await _userManager.FindByEmailAsync(request.Email);
            
            if (usuario is null || !await _userManager.CheckPasswordAsync(usuario, request.Password))
                return Unauthorized(new { message = "Invalid credentials" });

            var accessToken = await GenerateJwtTokenAsync(usuario);
            var refreshToken = GenerateRefreshToken();

            usuario.RefreshToken = refreshToken;
            usuario.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(usuario);

            return Ok(new ResponseOfGetToken {
                Token = accessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost(AuthControllerRoutes.RefreshToken)]
        public async Task<IActionResult> RefreshToken([FromBody] RequestOfRefreshToken request)
        {
            if (request is null) return BadRequest("Invalid request");

            var principal = GetPrincipalFromExpiredToken(request.Token);
            if (principal is null) return BadRequest("Invalid token");

            var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(emailClaim!);
            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return Unauthorized("Invalid refresh token");

            var newAccessToken = await GenerateJwtTokenAsync(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return Ok(new ResponseOfGetToken
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            return (securityToken is JwtSecurityToken jwtSecurityToken &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)) ?
                principal : null;
        }

        private async Task<string> GenerateJwtTokenAsync(UsuarioEntity usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id),
                new Claim(JwtRegisteredClaimNames.NameId, usuario.Cedula),
                new Claim(JwtRegisteredClaimNames.Name, $"{usuario.Nombre} {usuario.Apellido}"),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(usuario);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
