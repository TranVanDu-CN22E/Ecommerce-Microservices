using IdentityService.Domain.Aggregates.ProvinceAggregate;

namespace IdentityService.Domain.Interfaces
{
    public interface IProvinceRepository
    {
        Task AddAsync(Province province, CancellationToken ct = default);
        Task<Province?> GetProvinceById(int id, CancellationToken ct = default);
        Task<List<Province>> GetAllAsync(CancellationToken ct = default);
    }
}
