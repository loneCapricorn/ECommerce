using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ECommerceAPI.Services;

namespace ECommerceAPI.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController(UserService service) : ControllerBase
    {
        private readonly UserService _userService = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _userService.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}
