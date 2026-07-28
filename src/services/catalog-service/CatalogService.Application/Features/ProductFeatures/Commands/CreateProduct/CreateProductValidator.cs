using CatalogService.Application.Common;
using CatalogService.Application.DTOs;
using FluentValidation;

namespace CatalogService.Application.Features.ProductFeatures.Commands.CreateProduct
{
    public sealed class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithErrorCode(ProductErrors.ProductNameRequired.Code).WithMessage(ProductErrors.ProductNameRequired.Message)
                .MaximumLength(200).WithErrorCode(ProductErrors.ProductNameMaxLength.Code).WithMessage(ProductErrors.ProductNameMaxLength.Message);

            RuleFor(x => x.Slug)
                .NotEmpty().WithErrorCode(ProductErrors.ProductSlugRequired.Code).WithMessage(ProductErrors.ProductSlugRequired.Message)
                .MaximumLength(200).WithErrorCode(ProductErrors.ProductSlugMaxLength.Code).WithMessage(ProductErrors.ProductSlugMaxLength.Message)
                .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$").WithErrorCode(ProductErrors.ProductSlugInvalid.Code).WithMessage(ProductErrors.ProductSlugInvalid.Message);

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category ID is required.");

            RuleFor(x => x.Thumbnail)
                .NotEmpty().WithErrorCode(ProductErrors.ProductThumbnailRequired.Code).WithMessage(ProductErrors.ProductThumbnailRequired.Message);

            RuleForEach(x => x.Variants).SetValidator(new CreateProductVariantValidator());
        }
    }
    public sealed class CreateProductVariantValidator : AbstractValidator<CreateProductVariantDto>
    {
        public CreateProductVariantValidator()
        {
            // Validate SKU: Nên quy định format rõ ràng để dễ quản lý kho
            RuleFor(x => x.Sku)
                .NotEmpty().WithErrorCode(ProductErrors.VariantSkuRequired.Code).WithMessage(ProductErrors.VariantSkuRequired.Message)
                .MaximumLength(50).WithErrorCode(ProductErrors.VariantSkuMaxLenth.Code).WithMessage(ProductErrors.VariantSkuMaxLenth.Message)
                .Matches("^[A-Z0-9-]+$").WithErrorCode(ProductErrors.VariantSkuInvalid.Code).WithMessage(ProductErrors.VariantSkuInvalid.Message);

            // Validate Price: Phải lớn hơn 0 theo đúng logic Domain ProductVariant
            RuleFor(x => x.Price)
                .GreaterThan(0).WithErrorCode(ProductErrors.VariantPriceGreaterThanZero.Code).WithMessage(ProductErrors.VariantPriceGreaterThanZero.Message);

            // Validate Currency: Khớp với Money.Create
            RuleFor(x => x.Currency)
                .NotEmpty().WithErrorCode(ProductErrors.VariantCurrencyRequired.Code).WithMessage(ProductErrors.VariantCurrencyRequired.Message)
                .Length(3).WithErrorCode(ProductErrors.VariantCurrencyMaxLength.Code).WithMessage(ProductErrors.VariantCurrencyMaxLength.Message)
                .Must(currency => currency.All(char.IsLetter)).WithErrorCode(ProductErrors.VariantCurrencyLetter.Code).WithMessage(ProductErrors.VariantCurrencyLetter.Message);

            // Validate OriginalPrice
            RuleFor(x => x.OriginalPrice)
                .GreaterThanOrEqualTo(0).When(x => x.OriginalPrice.HasValue)
                .WithErrorCode(ProductErrors.VariantOriginalPriceGreaterThanOrEqualToZero.Code).WithMessage(ProductErrors.VariantOriginalPriceGreaterThanOrEqualToZero.Message);

            // Business Rule: Giá gốc phải lớn hơn hoặc bằng giá bán (nếu có)
            RuleFor(x => x)
                .Must(x => !x.OriginalPrice.HasValue || x.OriginalPrice.Value >= x.Price)
                .WithErrorCode(ProductErrors.VariantOriginalPriceGreterThanOrEqualToSellingPrice.Code).WithMessage(ProductErrors.VariantOriginalPriceGreterThanOrEqualToSellingPrice.Message);
        }
    }
}
