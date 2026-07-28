using CatalogService.Domain.Aggregates.ProductAggregate;

namespace CatalogService.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product, CancellationToken ct);
        Task<Product?> GetProductByIdAsync(ProductId id, CancellationToken ct);
        Task UpdateProductAsync(Product product, CancellationToken ct);
        Task DeleteProductAsync(ProductId id, CancellationToken ct);
        Task<string?> GetThumbnailUrlAsync(ProductId id, CancellationToken ct);
        Task<List<ProductVariant>> GetVariantsAsync(ProductId productId, CancellationToken ct);
    }
}
