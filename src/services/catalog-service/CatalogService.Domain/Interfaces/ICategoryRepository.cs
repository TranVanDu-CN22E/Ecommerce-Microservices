using CatalogService.Domain.Aggregates.CategoryAggregate;

namespace CatalogService.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task AddCategoryAsync(Category category, CancellationToken ct);
        Task<Category?> GetCategoryByIdAsync(CategoryId id, CancellationToken ct);
        Task<Category?> GetCategoryByParentIdAsync(CategoryId id, CancellationToken ct);
        Task<Category?> GetCategoryBySlugAsync(CategorySlug slug, CancellationToken ct);
        Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken ct);
        Task UpdateCategoryAsync(Category category, CancellationToken ct);
        Task DeleteCategoryAsync(CategoryId id, CancellationToken ct);
    }
}
