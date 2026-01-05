using IdentityService.Domain.Aggregates.RoleAggregate;
using IdentityService.Domain.Aggregates.UserAggregate;

namespace IdentityService.Application.Abstractions.Services
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(User user, List<RoleId> roles);
        string GenerateRefreshToken();
        Task<Guid?> ValidateRefreshTokenAsync(string refreshToken, CancellationToken ct);
    }
}
