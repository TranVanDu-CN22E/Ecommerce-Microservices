using IdentityService.Application.Abstractions.Messaging;
using IdentityService.Application.Common;
using IdentityService.Application.DTOs;

namespace IdentityService.Application.Features.User.Queries.GetCurrentUser
{
    public sealed record GetCurrentUserQuery(string UserId) : IQuery<Result<UserResponse>>;
}
