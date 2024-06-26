using Microsoft.AspNetCore.Mvc;
using MedicalClaim.Services;
using System.Threading.Tasks;

namespace MedicalClaim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(JwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request.Username == "test" && request.Password == "password")
            {
                var token = await _jwtTokenService.GenerateTokenAsync(request.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }

        [HttpPost("validate")]
        public async Task<IActionResult> Validate([FromBody] TokenRequest request)
        {
            var isValid = await _jwtTokenService.ValidateTokenAsync(request.Token);
            if (isValid)
            {
                return Ok();
            }

            return Unauthorized();
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] TokenRequest request)
        {
            await _jwtTokenService.RevokeTokenAsync(request.Token);
            return NoContent();
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class TokenRequest
    {
        public string Token { get; set; }
    }
}