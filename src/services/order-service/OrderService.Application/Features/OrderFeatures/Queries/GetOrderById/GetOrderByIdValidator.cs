using FluentValidation;
using OrderService.Application.Common;

namespace OrderService.Application.Features.OrderFeatures.Queries.GetOrderById
{
    public sealed class GetOrderByIdValidator : AbstractValidator<GetOrderByIdQuery>
    {
        public GetOrderByIdValidator() {
            RuleFor(x => x.OrderId).NotEmpty().WithErrorCode(OrderErrors.OrderIdRequired.Code).WithMessage(OrderErrors.OrderIdRequired.Message);
        }
    }
}
