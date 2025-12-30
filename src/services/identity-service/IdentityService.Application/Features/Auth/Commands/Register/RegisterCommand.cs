using IdentityService.Application.Abstractions.Messaging;
using IdentityService.Application.Common;

namespace IdentityService.Application.Features.Auth.Commands.Register
{
    public sealed record RegisterCommand(
        string Email,
        string Phone,
        string UserName,
        string Password) : ICommand<Result<RegisterResponse>>;
}
