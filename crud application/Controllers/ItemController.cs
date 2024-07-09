using crud_application.Models;
using crud_application.service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crud_application.Controllers
{
    [Route("item")]
    public class ItemController: ControllerBase
    {

        private readonly ItemService _itemService;
        private readonly ILogger<ItemController> _logger;

        public ItemController(ItemService itemService, ILogger<ItemController> logger)
        {
            _itemService = itemService;
            _logger = logger;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetItems()
        {

            var res = await _itemService.GetItems();
            return Ok(res);
        }
        [HttpGet("categorywiseitem/{id}")]
        public async Task<IActionResult> GetCategoryWiseItems(string id)
        {

            var res = await _itemService.GetCategoryWiseItems(id);
            return Ok(res);
        }
        [HttpGet("getNewRelease")]
        public async Task<IActionResult> GetNewReleaseItems()
        {

            var res = await _itemService.GetNewReleaseItems();
            return Ok(res);
        }
        [HttpGet("getBestSelling")]
        public async Task<IActionResult> GetBestSellingItems()
        {

            var res = await _itemService.GetBestSellingItems();
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(string id)
        {
            var res = await _itemService.GetItem(id);
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateItem([FromForm] ItemRequest item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _logger.LogInformation("category service: {item}", item);
            await _itemService.CreateItem(item);
            return Ok(item);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateItem(string id, [FromForm] ItemRequest item)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _logger.LogInformation("category service id: {item}", item);
            await _itemService.UpdateItem(id, item);
            return Ok(item);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteItem(string id)
        {
            await _itemService.DeleteItem(id);
            return NoContent();
        }
    }
}
