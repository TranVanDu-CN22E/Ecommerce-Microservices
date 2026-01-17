using IdentityService.Domain.Aggregates.PermissionAggregate;
using IdentityService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly IdentityDbContext _db;

        public PermissionRepository(IdentityDbContext db) => _db = db;

        public Task<Permission?> GetByIdAsync(string id, CancellationToken ct)
            => _db.Permissions.AsNoTracking().FirstOrDefaultAsync(p => p.Id.ToString() == id, ct);

        public async Task AddAsync(string name, CancellationToken ct)
        {
            var permission = Permission.Create(name);
            await _db.Permissions.AddAsync(permission, ct);
        }
    }
}
