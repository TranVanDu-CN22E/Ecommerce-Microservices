using IdentityService.Domain.Aggregates.RoleAggregate;
using IdentityService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IdentityDbContext _db;

        public RoleRepository(IdentityDbContext db) => _db = db;

        public Task<Role?> GetByIdAsync(string id, CancellationToken ct)
            => _db.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == RoleId.Create(Guid.Parse(id)), ct);

        public async Task AddAsync(string name, CancellationToken ct)
        {
            var role = Role.Create(name);
            await _db.Roles.AddAsync(role, ct);
        }
    }
}
