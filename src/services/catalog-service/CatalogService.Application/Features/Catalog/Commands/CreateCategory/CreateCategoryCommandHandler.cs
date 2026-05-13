using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Application.Common;
using CatalogService.Domain.Aggregates.CategoryAggregate;
using CatalogService.Domain.Interfaces;

namespace CatalogService.Application.Features.Catalog.Commands.CreateCategory
{
    public sealed class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, Result<Guid>>
    {
        private readonly ICategoryRepository _categoryRepository;
        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var slug = CategorySlug.Create(request.Slug);
            var existing = await _categoryRepository.GetCategoryBySlugAsync(slug, cancellationToken);
            if (existing is not null)
            {
                return Result<Guid>.Failure(new[] { new Error("Category.SlugConflict", "A category with the same slug already exists." )});
            }
            CategoryId? parentId = null;
            if (!string.IsNullOrWhiteSpace(request.ParentId) && Guid.TryParse(request.ParentId, out var parsedParentId))
            {
                parentId = CategoryId.Create(parsedParentId);
                var parent = await _categoryRepository.GetCategoryByIdAsync(parentId.Value, cancellationToken);
                if (parent is null)
                {
                    return Result<Guid>.Failure(new[] { new Error("Category.ParentNotFound", $"Parent category '{request.ParentId}' not found.") });
                }
            }

            var category = Category.Create(
                CategoryName.Create(request.Name),
                slug,
                parentId,
                request.Description,
                request.DisplayOrder);

            await _categoryRepository.AddCategoryAsync(category, cancellationToken);
            return Result<Guid>.Success(category.Id.Value);
        }
    }
}
