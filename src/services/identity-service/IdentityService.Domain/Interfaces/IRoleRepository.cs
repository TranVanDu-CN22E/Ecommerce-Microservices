using IdentityService.Domain.Aggregates.RoleAggregate;

namespace IdentityService.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(Guid id, CancellationToken ct);
        Task AddAsync(Role role, CancellationToken ct);
    }
}
