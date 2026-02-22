using CatalogService.Domain.Aggregates.ProductAggregate;

namespace CatalogService.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product, CancellationToken ct);
        Task<Product?> GetProductByIdAsync(Guid id, CancellationToken ct);
        Task UpdateProductAsync(Product product, CancellationToken ct);
        Task DeleteProductAsync(Guid id, CancellationToken ct);
    }
}
