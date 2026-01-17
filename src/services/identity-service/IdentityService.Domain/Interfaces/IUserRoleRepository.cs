using IdentityService.Domain.Aggregates.RoleAggregate;
using IdentityService.Domain.Aggregates.UserAggregate;

namespace IdentityService.Domain.Interfaces
{
    public interface IUserRoleRepository
    {
        Task AddAsync(string userId, string roleId, CancellationToken cancellationToken = default);
        Task<Task>RemoveAsync(string roleId, CancellationToken cancellationToken = default);
        Task<UserRole?> GetAsync(
            string userId, string roleId,
            CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(
            string userId, string roleId,
            CancellationToken cancellationToken = default);
        Task<List<RoleId>> GetRoleIdsByUserAsync(
            string userId,
            CancellationToken cancellationToken = default);
    }
}
