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
            => _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.ToString() == email, ct);

        public Task<User?> GetByIdAsync(string userId, CancellationToken ct)
        { 
            return _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id.ToString() == userId, ct);
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
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId, ct);
            if (user != null)
            {
                user.Update(
                    UserEmail.Create(email),
                    UserPhone.Create(phone),
                    UserName.Create(username),
                    PasswordHash.Create(password)
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
