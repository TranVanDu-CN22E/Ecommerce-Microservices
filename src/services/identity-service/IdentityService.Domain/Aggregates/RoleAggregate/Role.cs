using IdentityService.Domain.Aggregates.UserAggregate;
using IdentityService.Domain.Common;

namespace IdentityService.Domain.Aggregates.RoleAggregate
{
    public sealed class Role : AggregateRoot<Guid>
    {
        public string RoleName { get; private set; } = string.Empty;
        public List<Guid> Permissions { get; private set; } = new();
        public List<UserRole> UserRoles { get; private set; }
        private Role() { }
        private Role(RoleId roleId, string roleName) 
        {
            Id = roleId.Value;
            RoleName = roleName;
        }
        public static Role Create(string name) => new Role(RoleId.New(), name);
        public void AddPermission(Guid permissionId)
        {
            if (!Permissions.Contains(permissionId))
                Permissions.Add(permissionId);
        }
    }
}
