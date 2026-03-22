using Application.Abstractions.Services;
using Application.DTOs.Reviews.Requests;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
        }
        
        [HttpGet]
        [GatewayAuthorize(Roles = new[] { "Admin", "Seller", "Buyer" })]
        public async Task<IActionResult> GetReviewsAsync(int limit, int offset, CancellationToken ct)
        {
            var response = await _reviewService.GetReviewsAsync(limit, offset, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpPost]
        [GatewayAuthorize(Roles = new[] { "Buyer" })]
        public async Task<IActionResult> CreateReviewAsync([FromBody] ReviewItemRequest request, CancellationToken ct)
        {
            var response = await _reviewService.RateItemAsync(request, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpPut]
        [GatewayAuthorize(Roles = new[] { "Buyer" })]
        public async Task<IActionResult> UpdateReviewAsync([FromBody] ReviewItemRequest request, CancellationToken ct)
        {
            var response = await _reviewService.UpdateRateItemAsync(request, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpDelete("{id:Guid}")]
        [GatewayAuthorize(Roles = new[] { "Admin", "Buyer" })]
        public async Task<IActionResult> DeleteReviewAsync([FromRoute] Guid id, CancellationToken ct)
        {
            var response = await _reviewService.DeleteItemAsync(id, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}