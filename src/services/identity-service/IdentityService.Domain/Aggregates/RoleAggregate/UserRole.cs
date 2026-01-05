using IdentityService.Domain.Aggregates.UserAggregate;
using IdentityService.Domain.Common;
using Medo;

namespace IdentityService.Domain.Aggregates.RoleAggregate
{
    public class UserRole : Entity<Guid>
    {
        public UserId UserId { get; private set; }
        public RoleId RoleId { get; private set; }
        public User User { get; private set; }
        public Role Role { get; private set; }

        private UserRole() { }

        public UserRole(UserId userId, RoleId roleId)
        {
            Id = Uuid7.NewUuid7();
            UserId = userId;
            RoleId = roleId;
        }
        public static UserRole Create(UserId userId, RoleId roleId)
            => new(userId, roleId);
    }
}
