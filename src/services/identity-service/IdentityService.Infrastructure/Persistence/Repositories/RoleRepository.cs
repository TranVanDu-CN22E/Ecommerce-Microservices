using IdentityService.Domain.Aggregates.RoleAggregate;
using IdentityService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IdentityDbContext _db;

        public RoleRepository(IdentityDbContext db) => _db = db;

        public Task<Role?> GetByIdAsync(Guid id, CancellationToken ct)
            => _db.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id, ct);

        public async Task AddAsync(Role role, CancellationToken ct)
            => await _db.Roles.AddAsync(role, ct);
    }
}
