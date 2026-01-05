using IdentityService.Domain.Aggregates.RoleAggregate;
using IdentityService.Domain.Aggregates.UserAggregate;

namespace IdentityService.Domain.Interfaces
{
    public interface IUserRoleRepository
    {
        Task AddAsync(UserRole userRole, CancellationToken cancellationToken = default);
        Task RemoveAsync(UserRole userRole, CancellationToken cancellationToken = default);
        Task<UserRole?> GetAsync(
            UserId userId,
            RoleId roleId,
            CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(
            UserId userId,
            RoleId roleId,
            CancellationToken cancellationToken = default);
        Task<List<RoleId>> GetRoleIdsByUserAsync(
            UserId userId,
            CancellationToken cancellationToken = default);
    }
}
