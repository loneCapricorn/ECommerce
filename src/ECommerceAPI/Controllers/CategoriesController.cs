using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ECommerceAPI.Services;
using ECommerceAPI.Models;

namespace ECommerceAPI.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(CategoryService service) : ControllerBase
    {
        private readonly CategoryService _categoryService = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _categoryService.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            var created = await _categoryService.Create(category);
            return Ok(created);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool removed = await _categoryService.Delete(id);
            if (!removed) return NotFound();
            return NoContent();
        }
    }
}
