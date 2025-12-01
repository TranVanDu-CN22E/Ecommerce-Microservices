using IdentityService.Domain.Common;

namespace IdentityService.Domain.Aggregates.PermissionAggregate
{
    public sealed class Permission : AggregateRoot<Guid>
    {
        public PermissionId PermissionId { get; private set; } = default!;
        public string Name { get; private set; } = string.Empty;
        private Permission() { }
        private Permission(PermissionId id, string name)
        {
            Id = id.Value;
            PermissionId = id;
            Name = name;
        }

        public static Permission Create(string name)
            => new(PermissionId.New(), name);
    }
}
