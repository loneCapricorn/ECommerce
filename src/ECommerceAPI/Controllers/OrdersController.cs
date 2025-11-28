using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Services;
using ECommerceAPI.DTOs;

namespace ECommerceAPI.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController(OrderService service) : ControllerBase
    {
        private readonly OrderService _orderService = service;

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
        {
            var userIdString =
                User.FindFirstValue(JwtRegisteredClaimNames.Sub) ??
                User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                User.FindFirstValue("id");

            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized("User ID claim is missing or invalid.");

            var order = await _orderService.CreateOrder(userId, dto);
            return Ok(order);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userIdString =
                User.FindFirstValue(JwtRegisteredClaimNames.Sub) ??
                User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                User.FindFirstValue("id");

            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized("User ID claim is missing or invalid.");

            var orders = await _orderService.GetOrdersByUser(userId);
            return Ok(orders);
        }
    }
}
