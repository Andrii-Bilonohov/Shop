using Application.Abstractions.Services;
using Application.DTOs.Items.Requests;
using Application.DTOs.Reviews.Requests;
using Application.Filters;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;

namespace Presentation.Controllers
{
    
    [ApiController]
    [Route("api/items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
        }
        
        [HttpGet]
        [GatewayAuthorize(Roles = new[] { "Admin", "Seller", "Buyer"})]
        public async Task<IActionResult> GetItemsAsync([FromQuery] int limit = 100, [FromQuery] int offset = 0, [FromQuery] ItemFilter filter = null, CancellationToken ct = default)
        {
            var response =  await _itemService.GetAllAsync(limit, offset, filter, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id:Guid}")]
        [GatewayAuthorize(Roles = new[] { "Admin", "Seller", "Buyer"})]
        public async Task<IActionResult> GetItemAsync([FromRoute] Guid id, CancellationToken ct)
        {
            var response = await _itemService.GetByIdAsync(id, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        [GatewayAuthorize(Roles = new[] {"Seller"})]
        public async Task<IActionResult> CreateItemAsync([FromBody] CreateItem item, CancellationToken ct)
        {
            var response = await _itemService.CreateAsync(item, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id:Guid}")]
        [GatewayAuthorize(Roles = new[] { "Seller"})]
        public async Task<IActionResult> UpdateItemAsync([FromRoute] Guid id, [FromBody] UpdateItem item, CancellationToken ct)
        {
            var response = await _itemService.UpdateAsync(id, item, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id:Guid}")]
        [GatewayAuthorize(Roles = new[] { "Admin", "Seller"})]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken ct)
        {
            var response = await _itemService.DeleteAsync(id, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPatch("rate")]
        [GatewayAuthorize(Roles = new[] { "Buyer" })]
        public async Task<IActionResult> RateAsync([FromBody] ReviewItemRequest request, CancellationToken ct)
        {
            var response = await _itemService.RateAsync(request, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
