using IdentityService.Domain.Aggregates.RefreshTokenAggregate;
using IdentityService.Domain.Aggregates.UserAggregate;

namespace IdentityService.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default);
        Task AddAsync(string userId, string token, CancellationToken ct);
        Task RevokeAllUserTokensAsync(string userId, CancellationToken ct = default);
    }
}
