using IdentityService.Domain.Aggregates.UserAggregate;
using IdentityService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext _db;

        public UserRepository(IdentityDbContext db) => _db = db;

        public Task<User?> GetByEmailAsync(string email, CancellationToken ct)
            => _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Value == email, ct);

        public Task<User?> GetByIdAsync(UserId id, CancellationToken ct)
        { 
            return _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id.Value, ct);
        }

        public async Task AddAsync(User user, CancellationToken ct)
            => await _db.Users.AddAsync(user, ct);
    }
}
