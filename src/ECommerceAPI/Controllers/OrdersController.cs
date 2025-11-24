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
            int userId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
            var order = await _orderService.CreateOrder(userId, dto);
            return Ok(order);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyOrders()
        {
            int userId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
            var orders = await _orderService.GetOrdersByUser(userId);
            return Ok(orders);
        }
    }
}
