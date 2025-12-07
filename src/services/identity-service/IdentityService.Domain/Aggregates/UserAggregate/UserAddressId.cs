using IdentityService.Domain.Common;
using Medo;
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
        public static UserAddressId New() => new(Uuid7.NewUuid7());


        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
        public override string ToString() => Value.ToString();
    }
}
