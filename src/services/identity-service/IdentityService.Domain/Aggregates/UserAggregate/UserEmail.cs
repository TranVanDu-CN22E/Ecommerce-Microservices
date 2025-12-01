using IdentityService.Domain.Common;
using System.Text.RegularExpressions;


namespace IdentityService.Domain.Aggregates.UserAggregate
{
    public sealed class UserEmail : ValueObject
    {
        public string Value { get; }

        private UserEmail(string value)
        {
            Value = value;
        }

        public static UserEmail Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty");

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Invalid email format");

            return new(email.Trim().ToLower());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;
    }
}
