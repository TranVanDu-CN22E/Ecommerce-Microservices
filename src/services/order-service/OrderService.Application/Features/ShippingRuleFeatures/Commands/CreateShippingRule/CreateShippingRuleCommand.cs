using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Common;

namespace OrderService.Application.Features.ShippingRuleFeatures.Commands.CreateShippingRule
{
    public sealed record CreateShippingRuleCommand : ICommand<Result<bool>>
    {
        public IFormFile File { get; set; }
    }
}
