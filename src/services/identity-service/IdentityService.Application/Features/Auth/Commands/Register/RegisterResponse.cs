namespace IdentityService.Application.Features.Auth.Commands.Register
{
    public sealed record RegisterResponse
    (
        string UserId,
        string Email,
        string UserName,
        DateTime CreateAt
    );
}
