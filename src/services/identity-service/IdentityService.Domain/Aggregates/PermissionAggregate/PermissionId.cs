using IdentityService.Domain.Common;
using Medo;

namespace IdentityService.Domain.Aggregates.PermissionAggregate
{
    public sealed class PermissionId : ValueObject
    {
        public Guid Value { get; set; }
        private PermissionId (Guid value) => Value = value;
        public static PermissionId Create(Guid value) => new PermissionId(value);
        public static PermissionId New() => new(Uuid7.NewUuid7());
        protected override IEnumerable<object?> GetEqualityComponents()
        {
           yield return Value;
        }
    }
}
