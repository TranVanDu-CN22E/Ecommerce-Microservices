using IdentityService.Domain.Aggregates.ProvinceAggregate;

namespace IdentityService.Domain.Interfaces
{
    public interface IProvinceRepository
    {
        Task AddAsync(Province province, CancellationToken ct);
        Task<Province?> GetProvinceById(int id, CancellationToken ct);
        Task<List<Province>> GetAllAsync(CancellationToken ct);
    }
}
