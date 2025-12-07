using IdentityService.Domain.Aggregates.ProvinceAggregate;
using IdentityService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories
{
    public class ProvinceRepository : IProvinceRepository
    {
        private IdentityDbContext _db;
        public ProvinceRepository(IdentityDbContext db) => _db = db;
        public async Task AddAsync(Province province, CancellationToken ct)
        {
            await _db.Provinces.AddAsync(province, ct);
        }

        public async Task<List<Province>> GetAllAsync(CancellationToken ct)
        {
            return await _db.Provinces.AsNoTracking().ToListAsync();
        }

        public async Task<Province?> GetProvinceById(int id, CancellationToken ct)
        {
            return await _db.Provinces.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        }
    }
}
