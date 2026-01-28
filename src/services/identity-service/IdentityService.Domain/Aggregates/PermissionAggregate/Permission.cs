using IdentityService.Domain.Common;

namespace IdentityService.Domain.Aggregates.PermissionAggregate
{
    public sealed class Permission : AggregateRoot<PermissionId>
    {
        public string Name { get; private set; }
        private Permission() { }
        private Permission(PermissionId id, string name)
        {
            Id = id;
            Name = name;
        }

        public static Permission Create(string name)
            => new(PermissionId.New(), name);
    }
}
