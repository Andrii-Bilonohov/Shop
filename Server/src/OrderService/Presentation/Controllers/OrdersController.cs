using Application.Abstractions.Services;
using Application.DTOs.Requests;
using Application.Filters;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }
        
        [HttpGet]
        [GatewayAuthorize(Roles = new[] { "Admin", "Seller", "Buyer"})]
        public async Task<IActionResult> GetOrdersAsync([FromQuery] int limit = 100, [FromQuery] int offset = 0, [FromQuery] OrderFilter filter = null, CancellationToken ct = default)
        {
            var response = await _orderService.GetAllAsync(limit, offset, filter, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpGet("{id:Guid}")]
        [GatewayAuthorize(Roles = new[] { "Admin", "Seller", "Buyer"})]
        public async Task<IActionResult> GetOrderAsync([FromRoute] Guid id, CancellationToken ct)
        {
            var response = await _orderService.GetByIdAsync(id, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpPost]
        [GatewayAuthorize(Roles = new[] { "Buyer"})]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrder order, CancellationToken ct)
        {
            var response = await _orderService.CreateAsync(order, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpPatch("{id:Guid}/status")]
        [GatewayAuthorize(Roles = new[] { "Seller" })]
        public async Task<IActionResult> UpdateOrderStatusAsync([FromRoute] Guid id, [FromBody] UpdateOrderStatus status, CancellationToken ct)
        {
            var response = await _orderService.UpdateOrderStatusAsync(id, status, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpDelete("{id:Guid}")]
        [GatewayAuthorize(Roles = new[] { "Admin"})]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken ct)
        {
            var response = await _orderService.DeleteAsync(id, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }

    }
}
