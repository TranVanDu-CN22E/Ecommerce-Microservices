using FluentValidation;

namespace CatalogService.Application.Features.CatalogFeatures.Commands.DeleteCategory
{
    public sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required.");
        }
    }
}
