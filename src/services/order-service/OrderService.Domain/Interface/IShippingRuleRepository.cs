using OrderService.Domain.Aggregates.ShippingRuleAggregate;

namespace OrderService.Domain.Interface
{
    public interface IShippingRuleRepository
    {
        Task<ShippingRule?> GetShippingRuleAsync(ShippingRuleId shippingRuleId, CancellationToken ct = default);
        Task AddShippingRuleAsync(ShippingRule shippingRule, CancellationToken ct = default);
        Task UpdateShippingRuleAsync(ShippingRule shippingRule, CancellationToken ct = default);
    }
}
