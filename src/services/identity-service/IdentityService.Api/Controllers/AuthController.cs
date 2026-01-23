using IdentityService.Api.Common;
using IdentityService.Application.Features.Auth.Commands.Login;
using IdentityService.Application.Features.Auth.Commands.Logout;
using IdentityService.Application.Features.Auth.Commands.Register;
using IdentityService.Application.Features.Auth.Commands.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers;

[Route("api/[controller]")]
public class AuthController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand cmd)
    {
        var result = await _mediator.Send(cmd);
        return HandleResult(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand cmd)
    {
        var result = await _mediator.Send(cmd);
        return HandleResult(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenCommand cmd)
    {
        var result = await _mediator.Send(cmd);
        return HandleResult(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutCommand cmd)
    {
        var result = await _mediator.Send(cmd);
        return HandleResult(result);
    }
}
