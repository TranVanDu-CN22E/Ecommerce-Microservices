using IdentityService.Domain.Aggregates.RefreshTokenAggregate;
using IdentityService.Domain.Aggregates.UserAggregate;

namespace IdentityService.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct);
        Task AddAsync(RefreshToken refreshToken, CancellationToken ct);
        Task RevokeAllUserTokensAsync(UserId userId, CancellationToken ct);
    }
}
