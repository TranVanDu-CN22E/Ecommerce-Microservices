using IdentityService.Domain.Common;

namespace IdentityService.Domain.Aggregates.ProvinceAggregate
{
    public sealed class ProvinceId : ValueObject
    {
        public int Value { get; }
        private ProvinceId(int value)
        {
            if (value <= 0)
                throw new ArgumentException("ProvinceId must be greater than zero.", nameof(value));

            Value = value;
        }
        public static ProvinceId Create(int id) => new(id);
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
        public override string ToString() => Value.ToString();
    }
}
