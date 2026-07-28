using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Application.Common;
using CatalogService.Domain.Aggregates.CategoryAggregate;
using CatalogService.Domain.Interfaces;

namespace CatalogService.Application.Features.CatalogFeatures.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, Result<string>>
    {
        private readonly ICategoryRepository _categoryRepository;
        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository) => _categoryRepository = categoryRepository;
        public async Task<Result<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryId = CategoryId.Create(Guid.Parse(request.CategoryId));
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId, cancellationToken);
            if (category == null)
            {
                return Result<string>.Failure(new[] { new Error("Category.NotFound", $"Category '{request.CategoryId}' not found.") });
            }

            var children = await _categoryRepository.GetCategoryByParentIdAsync(categoryId, cancellationToken);
            if (children != null)
            {
                return Result<string>.Failure(new[] { new Error("Category.HasChildren", $"Category '{request.CategoryId}' has child.") });
            }

            category.Deactivate();
            await _categoryRepository.UpdateCategoryAsync(category, cancellationToken);
            return Result<string>.Success($"Category '{request.CategoryId}' deleted successfully.");
        }
    }
}
