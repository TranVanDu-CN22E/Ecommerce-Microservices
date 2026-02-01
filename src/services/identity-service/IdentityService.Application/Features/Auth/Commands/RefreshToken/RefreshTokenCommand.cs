using IdentityService.Application.Abstractions.Messaging;
using IdentityService.Application.Common;

namespace IdentityService.Application.Features.Auth.Commands.RefreshToken
{
    public sealed record RefreshTokenCommand(string RefreshToken)
        : ICommand<Result<RefreshTokenResponse>>
    {
    }
}