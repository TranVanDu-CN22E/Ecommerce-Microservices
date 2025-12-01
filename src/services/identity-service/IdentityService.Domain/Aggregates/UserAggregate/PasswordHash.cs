using IdentityService.Domain.Common;

namespace IdentityService.Domain.Aggregates.UserAggregate
{
    public sealed class PasswordHash : ValueObject
    {
        public string Value { get;}
        private PasswordHash(string value)
        {
            Value = value;
        }
        public static PasswordHash Create (string hash)
        {
            if (string.IsNullOrEmpty(hash))
            {
                throw new ArgumentException("Password hash cannot be empty");
            }
            return new PasswordHash(hash);
        }
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
