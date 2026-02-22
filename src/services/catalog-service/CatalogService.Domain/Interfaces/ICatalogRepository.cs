using CatalogService.Domain.Aggregates.CategoryAggregate;

namespace CatalogService.Domain.Interfaces
{
    public interface ICatalogRepository
    {
        Task AddCategoryAsync(Category category, CancellationToken ct);
        Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken ct);
        Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken ct);
        Task UpdateCategoryAsync(Category category, CancellationToken ct);
        Task DeleteCategoryAsync(Guid id, CancellationToken ct);
    }
}
