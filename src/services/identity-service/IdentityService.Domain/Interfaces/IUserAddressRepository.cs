using IdentityService.Domain.Aggregates.UserAggregate;

namespace IdentityService.Domain.Interfaces
{
    public interface IUserAddressRepository
    {
        Task AddAsync (UserAddress userAddress, CancellationToken ct);
        Task<UserAddress?> GetUserAddressByIdAsync (Guid id, CancellationToken ct);
        Task<List<UserAddress>> GetUserAddressByUserIdAsync(UserId userId, CancellationToken ct);
    }
}
