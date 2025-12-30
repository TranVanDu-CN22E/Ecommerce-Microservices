using IdentityService.Domain.Aggregates.UserAggregate;

namespace IdentityService.Application.Abstractions.Services
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        Task<Guid?> ValidateRefreshTokenAsync(string refreshToken, CancellationToken ct);
    }
}
