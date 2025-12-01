using IdentityService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain.Aggregates.UserAggregate
{
    public sealed class UserAddressId : ValueObject
    {
        public Guid Value { get; }
        private UserAddressId(Guid value) { this.Value = value; }
        public static UserAddressId Create(Guid value) => new(value);
        public static UserAddressId New() => new UserAddressId(Guid.NewGuid());


        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
        public override string ToString() => Value.ToString();
    }
}
