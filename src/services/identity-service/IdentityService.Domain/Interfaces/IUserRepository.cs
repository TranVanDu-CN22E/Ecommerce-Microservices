using IdentityService.Domain.Aggregates.UserAggregate;
using Microsoft.AspNetCore.Identity;
using System.Numerics;

namespace IdentityService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task<User?> GetByIdAsync(string userId, CancellationToken ct = default);
        Task<User> AddAsync(string email, string phone, string username, string passwordHash, CancellationToken ct = default);
        Task<User?> UpdateAsync(string userId, string? username, string? password, string? email, string? phone, string? reasonBan, string? reasonUnban, bool? resetLogin, CancellationToken ct = default);
    }
}
