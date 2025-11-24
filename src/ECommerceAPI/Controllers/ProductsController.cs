using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using ECommerceAPI.Models;
using ECommerceAPI.Services;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(ProductService service) : ControllerBase
    {
        private readonly ProductService _productService = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _productService.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var created = await _productService.Create(product);
            return Ok(created);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool removed = await _productService.Delete(id);
            if (!removed) return NotFound();
            return NoContent();
        }
    }
}
