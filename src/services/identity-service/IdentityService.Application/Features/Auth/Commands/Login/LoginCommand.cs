using IdentityService.Application.Abstractions.Messaging;
using IdentityService.Application.Common;

namespace IdentityService.Application.Features.Auth.Commands.Login
{
    public sealed record LoginCommand(string Email, string Password)
        : ICommand<Result<LoginResponse>>;
}
