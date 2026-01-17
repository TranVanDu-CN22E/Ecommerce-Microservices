using IdentityService.Domain.Aggregates.PermissionAggregate;

namespace IdentityService.Domain.Interfaces
{
    public interface IPermissionRepository
    {
        Task<Permission?> GetByIdAsync(string id, CancellationToken ct = default);
        Task AddAsync(string name, CancellationToken ct);
    }
}
