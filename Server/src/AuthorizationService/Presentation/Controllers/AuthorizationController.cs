using Application.Abstractions.Services.Authorization;
using Application.DTOs.Requests.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthorizationController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }


        [HttpPost("login")]
        public async Task<IActionResult> AuthLocalAsync([FromBody] LoginLocal login, CancellationToken ct)
        {
            var response = await _authService.AuthLocalAsync(login, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterLocalAsync([FromBody] RegisterLocal register, CancellationToken ct)
        {
            var response = await _authService.RegisterLocalAsync(register, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
