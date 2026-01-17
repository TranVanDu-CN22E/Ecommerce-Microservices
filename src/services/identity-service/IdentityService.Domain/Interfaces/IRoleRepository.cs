using IdentityService.Domain.Aggregates.RoleAggregate;

namespace IdentityService.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(string id, CancellationToken ct = default);
        Task AddAsync(string name, CancellationToken ct);
    }
}
