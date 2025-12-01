using IdentityService.Domain.Common;

namespace IdentityService.Domain.Aggregates.RoleAggregate
{
    public sealed class RoleId : ValueObject
    {
        public Guid Value { get;}
        private RoleId(Guid value) => Value = value;
        public static RoleId Create(Guid id) => new(id);
        public static RoleId New() => new(Guid.NewGuid());

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
