using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Common;

namespace OrderService.Application.Features.ShippingRuleFeatures.Commands.UpdateShippingRule
{
    public sealed record UpdateShippingRuleCommand : ICommand<Result<bool>>
    {
        public string ShippingRuleId { get; set; }
        public decimal BaseFeeAmount { get; set; }
        public string BaseFeeCurrency { get; set; }
        public decimal? FreeThresholdAmount { get; set; }
        public string? FreeThresholdCurrency { get; set; }
        public bool? IsActive { get; set; }
    }
}
