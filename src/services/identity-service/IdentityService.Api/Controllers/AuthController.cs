using IdentityService.Application.Features.Auth.Commands.Login;
using IdentityService.Application.Features.Auth.Commands.Logout;
using IdentityService.Application.Features.Auth.Commands.Register;
using IdentityService.Application.Features.Auth.Commands.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand cmd)
        => Ok(await _mediator.Send(cmd));
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand cmd)
        => Ok(await _mediator.Send(cmd));

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenCommand cmd)
        => Ok(await _mediator.Send(cmd));

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutCommand cmd)
        {
            await _mediator.Send(cmd);
            return NoContent();
        }
    }
}
