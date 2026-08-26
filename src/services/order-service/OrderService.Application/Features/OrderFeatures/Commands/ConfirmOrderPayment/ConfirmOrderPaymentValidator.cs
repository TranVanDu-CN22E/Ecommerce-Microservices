using FluentValidation;
using OrderService.Application.Common;
using OrderService.Application.Features.OrderFeatures.Commands.ConfirmOrderPayment;

namespace OrderService.Application.Features.OrderFeatures.Commands.ConfirmOrderPaymentStatus
{
    public class ConfirmOrderPaymentValidator : AbstractValidator<ConfirmOrderPaymentCommand>
    {
        public ConfirmOrderPaymentValidator() 
        {
            RuleFor(x => x.OrderId).NotEmpty().WithErrorCode(OrderErrors.OrderIdRequired.Code).WithMessage(OrderErrors.OrderIdRequired.Message);
            RuleFor(x => x.Amount)
                .NotEmpty().WithErrorCode(OrderErrors.AmountRequired.Code).WithMessage(OrderErrors.AmountRequired.Message)
                .Must(x => x > 0).WithErrorCode(OrderErrors.AmountMustBeGreaterThanZero.Code).WithMessage(OrderErrors.AmountMustBeGreaterThanZero.Message);
            RuleFor(x => x.Currency).NotEmpty().WithErrorCode(OrderErrors.CurrencyRequired.Code).WithMessage(OrderErrors.CurrencyRequired.Message);
        }
    }
}
