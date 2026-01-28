using IdentityService.Domain.Aggregates.UserAggregate;
using IdentityService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext _db;

        public UserRepository(IdentityDbContext db) => _db = db;

        public Task<User?> GetByEmailAsync(string email, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Task.FromResult<User?>(null);

            var normalized = email.Trim().ToLowerInvariant();
            return _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Value == normalized, ct);
        }

        public Task<User?> GetByIdAsync(string userId, CancellationToken ct)
        { 
            return _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == UserId.Create(Guid.Parse(userId)), ct);
        }

        public async Task<User> AddAsync(string email, string phone, string username, string passwordHash, CancellationToken ct)
        {
            var userEmail = UserEmail.Create(email);
            var userPhone = UserPhone.Create(phone);
            var userName = UserName.Create(username);
            var password = PasswordHash.Create(passwordHash);
            var user = User.Create(userEmail, userPhone, userName, password);
            await _db.Users.AddAsync(user, ct);
            return user;
        }
        public async Task<User?> UpdateAsync(string userId, string? username, string? password, string? email, string? phone, string? reasonBan, string? reasonUnban, bool? resetLogin, CancellationToken ct = default)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == UserId.Create(Guid.Parse(userId)), ct);
            if (user != null)
            {
                user.Update(
                    email is null ? null : UserEmail.Create(email),
                    phone is null ? null : UserPhone.Create(phone),
                    username is null ? null : UserName.Create(username),
                    password is null ? null : PasswordHash.Create(password)
                    );
                if (reasonBan != null) user.Ban(reasonBan);
                if (reasonUnban != null) user.Unban(reasonUnban);
                if (resetLogin == true) user.ResetFailedLogin();

                _db.Users.Update(user);
                return user;
            }
            else return null;
        }
    }
}
