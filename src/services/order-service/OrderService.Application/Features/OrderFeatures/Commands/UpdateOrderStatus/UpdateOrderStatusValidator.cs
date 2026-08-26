using FluentValidation;
using OrderService.Application.Common;

namespace OrderService.Application.Features.OrderFeatures.Commands.UpdateOrderStatus
{
    public sealed class UpdateOrderStatusValidator : AbstractValidator<UpdateOrderStatusCommand>
    {
        public UpdateOrderStatusValidator() { 
            RuleFor(x => x.OrderId).NotEmpty().WithErrorCode(OrderErrors.OrderIdRequired.Code).WithMessage(OrderErrors.OrderIdRequired.Message);
            RuleFor(x => x.OrderStatus)
                .NotEmpty().WithErrorCode(OrderErrors.OrderStatusRequired.Code).WithMessage(OrderErrors.OrderStatusRequired.Message)
                .InclusiveBetween(1, 3).WithErrorCode(OrderErrors.OrderStatusInvalid.Code).WithMessage(OrderErrors.OrderStatusInvalid.Message);
        }
    }
}
