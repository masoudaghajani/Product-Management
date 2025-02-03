using Domain.Models;
using Infrastructure.Api;
using Microsoft.AspNetCore.Mvc;
namespace Presentation.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Domain.Models.LoginRequest request)
        {
            if (request.Username == "admin" && request.Password == "password") // Replace with actual user validation
            {
                var token = _jwtService.GenerateToken(request.Username);
                return Ok(new { Token = token });
            }
            return Unauthorized(new { Error = "Invalid username or password." });
        }
    }

   

}