using IdentityService.Domain.Aggregates.RoleAggregate;
using IdentityService.Domain.Aggregates.UserAggregate;
using IdentityService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        public readonly IdentityDbContext _identityDbContext;
        public UserRoleRepository(IdentityDbContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }

        public async Task AddAsync(UserRole userRole, CancellationToken cancellationToken = default)
        {
            await _identityDbContext.UserRoles.AddAsync(userRole, cancellationToken);
        }
        public Task RemoveAsync(UserRole userRole, CancellationToken cancellationToken = default)
        {
            _identityDbContext.UserRoles.Remove(userRole);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(UserId userId, RoleId roleId, CancellationToken cancellationToken = default)
        {
            return await _identityDbContext.UserRoles.AnyAsync(x => x.UserId == userId && x.RoleId == roleId , cancellationToken);
        }

        public async Task<UserRole?> GetAsync(UserId userId, RoleId roleId, CancellationToken cancellationToken = default)
        {
            return await _identityDbContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId, cancellationToken);
        }

        public async Task<List<RoleId>> GetRoleIdsByUserAsync(UserId userId, CancellationToken cancellationToken = default)
        {
            return await _identityDbContext.UserRoles
                .Where(x => x.UserId == userId)
                .Select(x => x.RoleId)
                .ToListAsync(cancellationToken);
        }
    }
}
