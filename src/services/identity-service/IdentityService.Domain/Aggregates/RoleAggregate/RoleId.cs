using IdentityService.Domain.Common;
using Medo;

namespace IdentityService.Domain.Aggregates.RoleAggregate
{
    public sealed class RoleId : ValueObject
    {
        public Guid Value { get;}
        private RoleId(Guid value) => Value = value;
        public static RoleId Create(Guid id) => new(id);
        public static RoleId New() => new(Uuid7.NewUuid7());

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
