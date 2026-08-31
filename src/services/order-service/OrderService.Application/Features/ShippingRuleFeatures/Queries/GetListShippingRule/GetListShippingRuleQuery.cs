using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Common;
using OrderService.Application.DTOs;

namespace OrderService.Application.Features.ShippingRuleFeatures.Queries.GetListShippingRule
{
    public sealed record GetListShippingRuleQuery : IQuery<Result<List<ShippingRuleDto>>>
    {}
}
