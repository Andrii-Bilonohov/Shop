using Application.Abstractions.Services.Payments;
using Application.DTOs.Payments.Requests;
using Application.DTOs.Payments.Responses;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        }
        
        [HttpPost("{orderId}/pay")]
        [GatewayAuthorize(Roles = new[] { "Buyer"})]
        public async Task<IActionResult> PayOrderAsync([FromRoute] Guid orderId, [FromBody] BuyOrder request)
        {
            var response = await _paymentService.PayOrderAsync(orderId, request, HttpContext.RequestAborted);

            if (!response.Success)
                return BadRequest(new { response.Error });
            
            return Ok(new 
            { 
                response.Data!.Data, 
                response.Data.Signature 
            });
        }
        
        [HttpGet("{id}/status")]
        [GatewayAuthorize(Roles = new[] { "Admin", "Seller", "Buyer"})]
        public async Task<ActionResult<PaymentStatusResponse>> GetPaymentStatus([FromRoute] Guid id, CancellationToken ct)
        {
            var response = await _paymentService.GetPaymentStatusAsync(id, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        [HttpPatch("{id}/status")]
        [GatewayAuthorize(Roles = new[] { "Admin", "Seller", "Buyer"})]
        public async Task<ActionResult> UpdateStatusAsync([FromRoute] Guid id, [FromBody] UpdatePaymentStatus request,
            CancellationToken ct)
        {
            var response = await _paymentService.UpdateStatusAsync(id, request, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        
        // [HttpGet("liqpay/result")]
        // public IActionResult Result()
        // {
        //     return Content("""
        //                        <html>
        //                        <body>
        //                            <h2>Payment is being processed</h2>
        //                            <p>You can close this page.</p>
        //                        </body>
        //                        </html>
        //                    """, "text/html");
        // }
    }
}
