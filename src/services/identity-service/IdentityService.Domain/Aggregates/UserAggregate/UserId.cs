using IdentityService.Domain.Common;

namespace IdentityService.Domain.Aggregates.UserAggregate
{
    public sealed class UserId : ValueObject
    {
        public Guid Value { get; }

        private UserId(Guid value) => Value = value;

        public static UserId Create(Guid id) => new(id);
        public static UserId New() => new(Guid.NewGuid());

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value.ToString();
    }
}
