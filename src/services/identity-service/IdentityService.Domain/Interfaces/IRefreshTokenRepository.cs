using IdentityService.Domain.Aggregates.RefreshTokenAggregate;

namespace IdentityService.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct);
        Task AddAsync(RefreshToken refreshToken, CancellationToken ct);
    }
}
