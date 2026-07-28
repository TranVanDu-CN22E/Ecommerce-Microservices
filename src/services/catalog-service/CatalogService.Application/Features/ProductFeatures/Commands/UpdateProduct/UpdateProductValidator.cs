using CatalogService.Application.Common;
using CatalogService.Application.Features.ProductFeatures.Commands.CreateProduct;
using FluentValidation;
using System.Data;

namespace CatalogService.Application.Features.ProductFeatures.Commands.UpdateProduct
{
    public sealed class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator() 
        {
            RuleFor(x => x.Id)
               .NotEmpty().WithErrorCode(ProductErrors.ProductIdRequired.Code).WithMessage(ProductErrors.ProductIdRequired.Message);

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
}
