using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Aggregates.ShippingRuleAggregate;
using OrderService.Domain.Interface;

namespace OrderService.Infratructure.Persistence.Repository
{
    public class ShippingRuleRepository : IShippingRuleRepository
    {
        private OrderDbContext _context;
        public ShippingRuleRepository(OrderDbContext context)
        {
            _context = context;
        }
        public async Task<ShippingRule?> GetShippingRuleAsync(ShippingRuleId shippingRuleId, CancellationToken ct = default)
        {
            return await _context.ShippingRules.FirstOrDefaultAsync(sr => sr.Id == shippingRuleId, ct);
        }
        public async Task<List<ShippingRule>> GetListShippingRulesAsync(CancellationToken ct = default)
        {
            return await _context.ShippingRules.Where(sr => sr.IsActive == true).ToListAsync(ct);
        }
        public async Task AddShippingRuleAsync(ShippingRule shippingRule, CancellationToken ct = default)
        {
            await _context.ShippingRules.AddAsync(shippingRule, ct);
        }
        public async Task UpdateShippingRuleAsync(ShippingRule shippingRule, CancellationToken ct = default)
        {
            _context.ShippingRules.Update(shippingRule);
            await _context.SaveChangesAsync(ct);
        }
    }
}
