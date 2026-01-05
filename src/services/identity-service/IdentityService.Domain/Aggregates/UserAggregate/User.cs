using IdentityService.Domain.Aggregates.RefreshTokenAggregate;
using IdentityService.Domain.Aggregates.RoleAggregate;
using IdentityService.Domain.Common;

namespace IdentityService.Domain.Aggregates.UserAggregate
{
    public sealed class User : AggregateRoot<Guid>
    {
        public UserEmail Email { get; private set; } = default!;
        public UserPhone Phone { get; private set; } = default!;
        public UserName UserName { get; private set; } = default!;
        public PasswordHash PasswordHash { get; private set; } = default!;

        public bool IsLocked { get; private set; }
        public int FailedLoginAttempts { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? LockedUntil { get; private set; }
        public List<UserRole> UserRoles { get; private set; }
        public List<UserAddress> UserAddresses { get; private set; }
        public List<RefreshToken> RefreshTokens { get; private set; }

        private User() { }

        private User(UserId userId, UserEmail email, UserPhone phone, UserName userName, PasswordHash passwordHash)
        {
            Id = userId.Value;
            Email = email;
            Phone = phone;
            UserName = userName;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new UserCreatedDomainEvent(Id, Email.Value));
        }

        public static User Create(UserEmail email, UserPhone phone, UserName userName, PasswordHash passwordHash)
            => new(UserId.New(), email, phone, userName, passwordHash);

        public void Lock(string reason)
        {
            IsLocked = true;
            LockedUntil = DateTime.UtcNow.AddHours(12);
            AddDomainEvent(new UserLockedDomainEvent(Id, reason, LockedUntil));
        }

        public bool CanLogin()
        {
            if (IsLocked && LockedUntil.HasValue && LockedUntil > DateTime.UtcNow)
                return false;

            return true;
        }

        public void IncreaseFailedLogin()
        {
            FailedLoginAttempts++;
            if (FailedLoginAttempts >= 5)
                Lock("Too many failed login attempts");
        }

        public void ResetFailedLogin()
        {
            FailedLoginAttempts = 0;
            LockedUntil = null;
        }
    }
}
