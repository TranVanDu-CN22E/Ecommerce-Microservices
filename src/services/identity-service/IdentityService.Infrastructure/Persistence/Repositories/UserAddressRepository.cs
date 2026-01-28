using IdentityService.Domain.Aggregates.ProvinceAggregate;
using IdentityService.Domain.Aggregates.UserAggregate;
using IdentityService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories
{
    public class UserAddressRepository : IUserAddressRepository
    {
        private IdentityDbContext _db;
        public UserAddressRepository(IdentityDbContext db) => _db = db;
        public async Task AddAsync(string userId, string address, string? note, string phone, int provinceId, CancellationToken ct)
        {
            var userAddress = UserAddress.Create(UserId.Create(Guid.Parse(userId)), address, note, UserPhone.Create(phone), ProvinceId.Create(provinceId));
            await _db.UserAddresses.AddAsync(userAddress, ct);
        }

        public async Task<UserAddress?> GetUserAddressByIdAsync(string id, CancellationToken ct)
            => await _db.UserAddresses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == UserAddressId.Create(Guid.Parse(id)), ct);

        public async Task<List<UserAddress>> GetUserAddressByUserIdAsync(string userId, CancellationToken ct)
        {
            return await _db.UserAddresses.AsNoTracking().Where(x => x.UserId == UserId.Create(Guid.Parse(userId))).ToListAsync(ct);
        }
    }
}
