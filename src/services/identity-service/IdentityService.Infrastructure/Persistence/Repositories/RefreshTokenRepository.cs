using IdentityService.Domain.Aggregates.RefreshTokenAggregate;
using IdentityService.Domain.Aggregates.UserAggregate;
using IdentityService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IdentityDbContext _db;

        public RefreshTokenRepository(IdentityDbContext db) => _db = db;

        public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct)
            => _db.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(x => x.Token == token, ct);

        public async Task AddAsync(string userId, string token, CancellationToken ct)
        {
            var refreshToken = RefreshToken.Create(UserId.Create(Guid.Parse(userId)), token, DateTime.UtcNow.AddDays(7));
            await _db.RefreshTokens.AddAsync(refreshToken, ct);
        }
        public async Task RevokeAllUserTokensAsync(
            string userId,
            CancellationToken ct)
        {
            var tokens = await _db.RefreshTokens
                .Where(x => x.UserId.ToString() == userId && !x.IsRevoked)
                .ToListAsync(ct);

            foreach (var token in tokens)
            {
                token.Revoke();
            }
        }
    }
}
