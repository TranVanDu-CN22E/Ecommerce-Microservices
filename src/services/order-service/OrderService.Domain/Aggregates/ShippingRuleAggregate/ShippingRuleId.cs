using Medo;

namespace OrderService.Domain.Aggregates.ShippingRuleAggregate
{
    public readonly record struct ShippingRuleId (Guid Value)
    {
        public static ShippingRuleId New() => new(Uuid7.NewGuid());
        public static ShippingRuleId FromGuid(Guid value) => new(value);
    }
}
