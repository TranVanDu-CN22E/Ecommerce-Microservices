using IdentityService.Domain.Aggregates.RefreshTokenAggregate;
using IdentityService.Domain.Aggregates.UserAggregate;
using IdentityService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IdentityDbContext _db;

        public RefreshTokenRepository(IdentityDbContext db) => _db = db;

        public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct)
            => _db.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(x => x.Token == token, ct);

        public async Task AddAsync(RefreshToken token, CancellationToken ct)
            => await _db.RefreshTokens.AddAsync(token, ct);
        public async Task RevokeAllUserTokensAsync(
            UserId userId,
            CancellationToken ct)
        {
            var tokens = await _db.RefreshTokens
                .Where(x => x.UserId == userId && !x.IsRevoked)
                .ToListAsync(ct);

            foreach (var token in tokens)
            {
                token.Revoke();
            }
        }
    }
}
