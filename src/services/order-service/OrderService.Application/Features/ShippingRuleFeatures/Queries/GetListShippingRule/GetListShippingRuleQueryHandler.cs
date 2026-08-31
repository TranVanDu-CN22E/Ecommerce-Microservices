using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Common;
using OrderService.Application.DTOs;
using OrderService.Domain.Interface;

namespace OrderService.Application.Features.ShippingRuleFeatures.Queries.GetListShippingRule
{
    public sealed class GetListShippingRuleQueryHandler : IQueryHandler<GetListShippingRuleQuery, Result<List<ShippingRuleDto>>>
    {
        private readonly IShippingRuleRepository _shippingRuleRepository;
        public GetListShippingRuleQueryHandler(IShippingRuleRepository shippingRuleRepository)
        {
            _shippingRuleRepository = shippingRuleRepository;
        }
        public async Task<Result<List<ShippingRuleDto>>> Handle(GetListShippingRuleQuery request, CancellationToken cancellationToken)
        {
            var shippingRules = await _shippingRuleRepository.GetListShippingRulesAsync(cancellationToken);

            if (shippingRules == null || shippingRules.Count == 0)
            {
                return Result<List<ShippingRuleDto>>.Failure(new[] { new Error("GetListShippingRuleQuery", "No shipping rules found") });
            }

            var result = new List<ShippingRuleDto>();
            foreach (var shippingRule in shippingRules)
            {
                result.Add(new ShippingRuleDto
                {
                    Id = shippingRule.Id.ToString(),
                    Province = shippingRule.Province.ToString(),
                    BaseFee = new Money { Amount = shippingRule.BaseFee.Amount, Currency = shippingRule.BaseFee.Currency },
                    FreeThreshold = new Money { Amount = shippingRule.FreeThreshold.Amount, Currency = shippingRule.FreeThreshold.Currency },
                    IsActive = shippingRule.IsActive,
                    CreatedAt = shippingRule.CreatedAt,
                    UpdatedAt = shippingRule.UpdatedAt
                });
            }
            return Result<List<ShippingRuleDto>>.Success(result); 
        }
    }
}
