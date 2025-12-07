using IdentityService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain.Aggregates.UserAggregate
{
    public sealed class UserPhone : ValueObject
    {
        public string Value { get; private set; } = string.Empty;
        private UserPhone(string value) => Value = value;
        public static UserPhone Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Phone cannot be empty", nameof(value));
            if (value.Length != 10 || !value.All(char.IsDigit)) throw new ArgumentException("Phone must be 10 digits", nameof(value));
            return new(value);
        }
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
