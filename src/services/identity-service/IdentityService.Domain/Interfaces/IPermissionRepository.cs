using IdentityService.Domain.Aggregates.PermissionAggregate;

namespace IdentityService.Domain.Interfaces
{
    public interface IPermissionRepository
    {
        Task<Permission?> GetByIdAsync(Guid id, CancellationToken ct);
        Task AddAsync(Permission permission, CancellationToken ct);
    }
}
