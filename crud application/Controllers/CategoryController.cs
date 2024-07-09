using crud_application.Models;
using crud_application.service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crud_application.Controllers
{
    [Route("category")]
    public class CategoryController:ControllerBase
    {
        private readonly CategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(CategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetCategories()
        {
            
            var res = await _categoryService.GetCategories();
            return Ok(res);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(string id)
        {
            var res = await _categoryService.GetCategory(id);
            return Ok(res);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryRequest category)
        {
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }
            _logger.LogInformation("category service: {category}", category);
            var res=await _categoryService.CreateCategory(category);
            return Ok(res);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCategory(string id, [FromForm] CategoryRequest category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _logger.LogInformation("category service id: {category}", id);
            await _categoryService.UpdateCategory(id,category);
            return Ok(id);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _categoryService.DeleteCategory(id);
            return NoContent();
        }
    }
}
