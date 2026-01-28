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

        public async Task AddAsync(string userId, string roleId, CancellationToken cancellationToken = default)
        {
            var userRole = UserRole.Create(UserId.Create(Guid.Parse(userId)), RoleId.Create(Guid.Parse(roleId)));
            await _identityDbContext.UserRoles.AddAsync(userRole, cancellationToken);
        }
        public async Task<Task> RemoveAsync(string roleId, CancellationToken cancellationToken = default)
        {
            var role = await _identityDbContext.UserRoles.FirstOrDefaultAsync(x => x.RoleId == RoleId.Create(Guid.Parse(roleId)), cancellationToken);
            if (role != null)
            {
                _identityDbContext.UserRoles.Remove(role);
                return Task.CompletedTask;
            }
            return Task.FromException(new InvalidOperationException("Role is not found"));
        }

        public async Task<bool> ExistsAsync(string userId, string roleId, CancellationToken cancellationToken = default)
        {
            return await _identityDbContext.UserRoles.AnyAsync(x => x.UserId == UserId.Create(Guid.Parse(userId)) && x.RoleId == RoleId.Create(Guid.Parse(roleId)) , cancellationToken);
        }

        public async Task<UserRole?> GetAsync(string userId, string roleId, CancellationToken cancellationToken = default)
        {
            return await _identityDbContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == UserId.Create(Guid.Parse(userId)) && x.RoleId == RoleId.Create(Guid.Parse(roleId)), cancellationToken);
        }

        public async Task<List<RoleId>> GetRoleIdsByUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _identityDbContext.UserRoles
                .Where(x => x.UserId == UserId.Create(Guid.Parse(userId)))
                .Select(x => x.RoleId)
                .ToListAsync(cancellationToken);
        }
    }
}
