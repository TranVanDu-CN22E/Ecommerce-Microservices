using IdentityService.Application.Abstractions.Messaging;
using IdentityService.Application.Common;

namespace IdentityService.Application.Features.Auth.Commands.Logout
{
    public sealed record LogoutCommand(string UserId) : ICommand<Result>;
}
