using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.DTOs;
using ECommerceAPI.Services;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(AuthService authService) : ControllerBase
    {
        private readonly AuthService _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            bool success = await _authService.Register(dto);

            if (!success)
                return BadRequest("Email already exists.");

            return Ok("Registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _authService.Login(dto);

            if (token == null)
                return Unauthorized("Invalid email or password.");

            return Ok(new { token });
        }
    }
}
