using IdentityService.Domain.Common;

namespace IdentityService.Domain.Aggregates.RefreshTokenAggregate
{
    public sealed class RefreshTokenId : ValueObject
    {
        public Guid Value { get; private set; } = default!;
        private RefreshTokenId(Guid value) { Value = value; }
        public static RefreshTokenId Create (Guid id) => new(id);
        public static RefreshTokenId New() => new(Guid.NewGuid());
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
