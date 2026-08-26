using FluentValidation;
using OrderService.Application.Common;

namespace OrderService.Application.Features.OrderFeatures.Commands.RefundOrderPayment
{
    public class RefundOrderPaymentValidator : AbstractValidator<RefundOrderPaymentCommand>
    {
        public RefundOrderPaymentValidator() 
        { 
            RuleFor(x => x.OrderId).NotEmpty().WithErrorCode(OrderErrors.OrderIdRequired.Code).WithMessage(OrderErrors.OrderIdRequired.Message);
        }
    }
}
