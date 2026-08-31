using OrderService.Application.DTOs;

namespace OrderService.Application.Abstractions.Services
{
    public interface IShippingRuleCacheService
    {
        Task<ShippingRuleDto?> GetShippingRuleAsync(string ruleId, CancellationToken cancellationToken = default);
        Task<List<ShippingRuleDto>> GetListShippingRulesAsync(CancellationToken cancellationToken = default);
        Task<bool> AddShippingRuleAsync(string ruleId, ShippingRuleDto rule, CancellationToken cancellationToken = default);
        Task<bool> AddListShippingRulesAsync(List<ShippingRuleDto> rules, CancellationToken cancellationToken = default);
        Task<bool> RemoveShippingRuleAsync(string ruleId, CancellationToken cancellationToken = default);
    }
}
