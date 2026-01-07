using IdentityService.Domain.Aggregates.UserAggregate;

namespace IdentityService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task<User?> GetByIdAsync(UserId id, CancellationToken ct = default);
        Task AddAsync(User user, CancellationToken ct = default);
    }
}
