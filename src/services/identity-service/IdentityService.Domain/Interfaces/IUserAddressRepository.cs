using IdentityService.Domain.Aggregates.UserAggregate;

namespace IdentityService.Domain.Interfaces
{
    public interface IUserAddressRepository
    {
        Task AddAsync(string userId, string address, string? note, string phone, int provinceId, CancellationToken ct);
        Task<UserAddress?> GetUserAddressByIdAsync (string id, CancellationToken ct = default);
        Task<List<UserAddress>> GetUserAddressByUserIdAsync(string userId, CancellationToken ct = default);
    }
}
