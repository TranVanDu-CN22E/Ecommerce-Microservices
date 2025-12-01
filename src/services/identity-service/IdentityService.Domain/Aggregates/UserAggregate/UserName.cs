using IdentityService.Domain.Common;
namespace IdentityService.Domain.Aggregates.UserAggregate
{
    public sealed class UserName : ValueObject
    {
        public string Value { get; }

        private UserName(string value)
        {
            Value = value;
        }

        public static UserName Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("User name cannot be empty");

            return new(name.Trim());
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;
    }

}
