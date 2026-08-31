using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Common;
using OrderService.Domain.Aggregates.ShippingRuleAggregate;
using OrderService.Domain.Interface;

namespace OrderService.Application.Features.ShippingRuleFeatures.Commands.UpdateShippingRule
{
    public sealed class UpdateShippingRuleCommandHandler : ICommandHandler<UpdateShippingRuleCommand, Result<bool>>
    {
        private readonly IShippingRuleRepository _shippingRuleRepository;
        public UpdateShippingRuleCommandHandler(IShippingRuleRepository shippingRuleRepository)
        {
            _shippingRuleRepository = shippingRuleRepository;
        }

        public async Task<Result<bool>> Handle(UpdateShippingRuleCommand request, CancellationToken cancellationToken)
        {

            var shippingRule = await _shippingRuleRepository.GetShippingRuleAsync(ShippingRuleId.FromGuid(Guid.Parse(request.ShippingRuleId)), cancellationToken);
            if (shippingRule == null)
            {
                return Result<bool>.Failure(new[] { ShippingRuleErrors.ShippingRuleNotFound });
            }

            shippingRule.Update(
                Domain.Aggregates.OrderAggregate.Money.Create(request.BaseFeeAmount, request.BaseFeeCurrency.ToString()),
                Domain.Aggregates.OrderAggregate.Money.Create(request.FreeThresholdAmount.Value, request.FreeThresholdCurrency.ToString())
                );
            await _shippingRuleRepository.UpdateShippingRuleAsync(shippingRule, cancellationToken);
            return Result<bool>.Success(true);

        }
    }
}
