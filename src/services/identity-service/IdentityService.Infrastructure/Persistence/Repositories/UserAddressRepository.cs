using IdentityService.Domain.Aggregates.UserAggregate;
using IdentityService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories
{
    public class UserAddressRepository : IUserAddressRepository
    {
        private IdentityDbContext _db;
        public UserAddressRepository(IdentityDbContext db) => _db = db;
        public async Task AddAsync(UserAddress userAddress, CancellationToken ct)
            => await _db.UserAddresses.AddAsync(userAddress, ct);

        public async Task<UserAddress?> GetUserAddressByIdAsync(Guid id, CancellationToken ct)
            => await _db.UserAddresses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task<List<UserAddress>> GetUserAddressByUserIdAsync(Guid userId, CancellationToken ct)
        {
            return await _db.UserAddresses.AsNoTracking().Where(x => x.UserId.Value == userId).ToListAsync(ct);
        }
    }
}
