using IdentityService.Domain.Aggregates.PermissionAggregate;
using IdentityService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly IdentityDbContext _db;

        public PermissionRepository(IdentityDbContext db) => _db = db;

        public Task<Permission?> GetByIdAsync(Guid id, CancellationToken ct)
            => _db.Permissions.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);

        public async Task AddAsync(Permission permission, CancellationToken ct)
            => await _db.Permissions.AddAsync(permission, ct);
    }
}
