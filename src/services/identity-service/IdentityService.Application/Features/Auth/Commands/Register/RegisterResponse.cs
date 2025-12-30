namespace IdentityService.Application.Features.Auth.Commands.Register
{
    public sealed record RegisterResponse
    (
        Guid UserId,
        string Email,
        string UserName,
        DateTime CreateAt
    );
}
