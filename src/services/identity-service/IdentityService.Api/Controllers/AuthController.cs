using IdentityService.Api.Common;
using IdentityService.Application.Features.Auth.Commands.Login;
using IdentityService.Application.Features.Auth.Commands.Logout;
using IdentityService.Application.Features.Auth.Commands.RefreshToken;
using IdentityService.Application.Features.Auth.Commands.Register;
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

        if (result.IsFailure)
            return BadRequest(result.Errors);

        var data = result.Value;

        Response.Cookies.Append("refreshToken", data.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = data.RefreshExpiresAt
        });

        return Ok(new
        {
            accessToken = data.AccessToken,
            expiresAt = data.AccessExpiresAt
        });
    }


    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized("Missing refresh token");

        var cmd = new RefreshTokenCommand(refreshToken);
        var result = await _mediator.Send(cmd);

        if (result.IsFailure)
            return Unauthorized(result.Errors);

        var data = result.Value;
        Response.Cookies.Append("refreshToken", data.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = data.RefreshExpiresAt
        });
        return Ok(new
        {
            accessToken = data.AccessToken,
            expiresAt = data.AccessExpiresAt
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutCommand cmd)
    {
        var result = await _mediator.Send(cmd);
        Response.Cookies.Delete("refreshToken");
        return HandleResult(result);
    }
}
