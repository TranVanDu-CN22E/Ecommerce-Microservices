using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Application.Common;
using CatalogService.Domain.Aggregates.CategoryAggregate;
using CatalogService.Domain.Interfaces;

namespace CatalogService.Application.Features.Catalog.Commands.UpdateCategory
{
    public sealed class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, Result>
    {
        private readonly ICategoryRepository _categoryRepository;
        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryId = CategoryId.Create(Guid.Parse(request.Id));
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId, cancellationToken);
            if (category is null)
                return Result.Failure<Result>(new[] { new Error("Category.NotFound", $"Category '{request.Id}' not found.") });

            CategorySlug? slug = null;
            if (request.Slug is not null)
            {
                slug = CategorySlug.Create(request.Slug);
                var conflict = await _categoryRepository.GetCategoryBySlugAsync(slug.Value, cancellationToken);
                if (conflict is not null && conflict.Id != category.Id)
                    return Result.Failure<Result>(new[] { new Error("Category.SlugConflict", $"Slug '{request.Slug}' is already taken.") });
            }
            CategoryId? parentId = null;
            if(request.ParentId is not null) {
                if (!Guid.TryParse(request.ParentId, out var parsedParentId))
                {
                    return Result.Failure<Result>(new[] { new Error("Category.InvalidParentId", $"ParentId '{request.ParentId}' is not a valid GUID.") });
                }
                parentId = CategoryId.Create(parsedParentId);
                var parent = await _categoryRepository.GetCategoryByIdAsync(parentId.Value, cancellationToken);
                if (parent is null)
                {
                    return Result.Failure<Result>(new[] { new Error("Category.ParentNotFound", $"Parent category '{request.ParentId}' not found.") });
                }
            }
            category.Update(
                request.Name is not null ? CategoryName.Create(request.Name) : null,
                slug,
                parentId,
                request.Description,
                request.DisplayOrder);

            await _categoryRepository.UpdateCategoryAsync(category, cancellationToken);

            return Result.Success();
        }
    }
}
