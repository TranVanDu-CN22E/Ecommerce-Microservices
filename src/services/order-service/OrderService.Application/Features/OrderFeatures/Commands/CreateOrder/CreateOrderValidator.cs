using FluentValidation;
using OrderService.Application.Common;

namespace OrderService.Application.Features.OrderFeatures.Commands.CreateOrder
{
    public sealed class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty().WithErrorCode(OrderErrors.CustomerIdRequired.Code).WithMessage(OrderErrors.CustomerIdRequired.Message);

            RuleFor(x => x.PaymentMethod)
                .NotEmpty().WithErrorCode(OrderErrors.PaymentMethodRequired.Code).WithMessage(OrderErrors.PaymentMethodRequired.Message)
                .InclusiveBetween(1, 3).WithErrorCode(OrderErrors.PaymentMethodInvalid.Code).WithMessage(OrderErrors.PaymentMethodInvalid.Message);

            RuleFor(x => x.ShippingAddress.RecipientName).NotEmpty().WithErrorCode(OrderErrors.RecipientNameRequired.Code).WithMessage(OrderErrors.RecipientNameRequired.Message);
            RuleFor(x => x.ShippingAddress.PhoneNumber)
                .NotEmpty().WithErrorCode(OrderErrors.PhoneNumberRequired.Code).WithMessage(OrderErrors.PhoneNumberRequired.Message)
                .Matches(@"^[0-9]{9,15}$").WithErrorCode(OrderErrors.PhoneNumberInvalid.Code).WithMessage(OrderErrors.PhoneNumberInvalid.Message);
            RuleFor(x => x.ShippingAddress.AddressLine).NotEmpty().WithErrorCode(OrderErrors.AddressLineRequired.Code).WithMessage(OrderErrors.AddressLineRequired.Message);
            RuleFor(x => x.ShippingAddress.Ward).NotEmpty().WithErrorCode(OrderErrors.WardRequired.Code).WithMessage(OrderErrors.WardRequired.Message);
            RuleFor(x => x.ShippingAddress.District).NotEmpty().WithErrorCode(OrderErrors.DistrictRequired.Code).WithMessage(OrderErrors.DistrictRequired.Message);
            RuleFor(x => x.ShippingAddress.Province).NotEmpty().WithErrorCode(OrderErrors.ProvinceRequired.Code).WithMessage(OrderErrors.ProvinceRequired.Message);
            RuleFor(x => x.ShippingAddress.Country).NotEmpty().WithErrorCode(OrderErrors.CountryRequired.Code).WithMessage(OrderErrors.CountryRequired.Message);

            RuleFor(x => x.ShippingFee.Amount).NotEmpty().WithErrorCode(OrderErrors.AmountRequired.Code).WithMessage(OrderErrors.AmountRequired.Message);
            RuleFor(x => x.ShippingFee.Currency).NotEmpty().WithErrorCode(OrderErrors.CurrencyRequired.Code).WithMessage(OrderErrors.CurrencyRequired.Message);
            RuleForEach(x => x.OrderItems).SetValidator(new OrderItemValidator());
        }
    }
    public sealed class OrderItemValidator : AbstractValidator<OrderItem>
    {
        public OrderItemValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithErrorCode(OrderErrors.ProductIdRequired.Code).WithMessage(OrderErrors.ProductIdRequired.Message);
            RuleFor(x => x.ProductVariantId).NotEmpty().WithErrorCode(OrderErrors.ProductVariantIdRequired.Code).WithMessage(OrderErrors.ProductVariantIdRequired.Message);
            RuleFor(x => x.ProductName).NotEmpty().WithErrorCode(OrderErrors.ProductNameRequired.Code).WithMessage(OrderErrors.ProductNameRequired.Message);
            RuleFor(x => x.VariantSku).NotEmpty().WithErrorCode(OrderErrors.VariantSkuRequired.Code).WithMessage(OrderErrors.VariantSkuRequired.Message);
            RuleFor(x => x.Quantity).NotEmpty().WithErrorCode(OrderErrors.QuantityRequired.Code).WithMessage(OrderErrors.QuantityRequired.Message);
            RuleFor(x => x.UnitPrice.Amount).NotEmpty().WithErrorCode(OrderErrors.AmountRequired.Code).WithMessage(OrderErrors.AmountRequired.Message);
            RuleFor(x => x.UnitPrice.Currency).NotEmpty().WithErrorCode(OrderErrors.CurrencyRequired.Code).WithMessage(OrderErrors.CurrencyRequired.Message);
        }
    }
}
