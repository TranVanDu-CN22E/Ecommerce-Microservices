namespace IdentityService.Application.Features.Auth.Commands.Login
{
    public sealed record LoginResponse(string AccessToken, string RefreshToken, DateTime AccessExpiresAt, DateTime RefreshExpiresAt);
}
