using IdentityService.Application.Abstractions.Messaging;
using IdentityService.Application.Common;

namespace IdentityService.Application.Features.User.Queries.GetUserById
{
    public sealed record GetUserByIdQuery : IQuery<Result<GetUserByIdQueryResponse>>
    {
        public string UserId { get; init; } = string.Empty;
        public GetUserByIdQuery(string userId)
        {
            UserId = userId;
        }
    }
}
