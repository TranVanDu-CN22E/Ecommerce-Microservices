using IdentityService.Domain.Common;
using Medo;

namespace IdentityService.Domain.Aggregates.RefreshTokenAggregate
{
    public sealed class RefreshTokenId : ValueObject
    {
        public Guid Value { get; private set; } = default!;
        private RefreshTokenId(Guid value) { Value = value; }
        public static RefreshTokenId New() => new(Uuid7.NewUuid7());

        public static RefreshTokenId Create(Guid value)
            => new(value);
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
