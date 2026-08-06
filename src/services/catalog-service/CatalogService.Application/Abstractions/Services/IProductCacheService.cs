using CatalogService.Application.DTOs;

namespace CatalogService.Application.Abstractions.Services
{
    public interface IProductCacheService
    {
        Task<ProductResponseDto?> GetProductAsync(string productId, CancellationToken cancellationToken = default);
        Task<bool> SetProductAsync(string productId, ProductResponseDto product, CancellationToken cancellationToken = default);
        Task<bool> RefreshTtlAsync(string productId, CancellationToken cancellationToken = default);
        Task<bool> IncrementReservedCountAsync(string productId, CancellationToken cancellationToken = default);
        Task<bool> DecrementReservedCountAsync(string productId, CancellationToken cancellationToken = default);
        Task<int> GetReservedCountAsync(string productId, CancellationToken cancellationToken = default);
        Task<bool> UpdateVariantStockAsync(string productId, string variantSku, int quantityChange, CancellationToken cancellationToken = default);
        Task<bool> UpdateVariantReservedAsync(string productId, string variantSku, int quantityChange, CancellationToken cancellationToken = default);
        Task<bool> DeleteProductAsync(string productId, CancellationToken cancellationToken = default);
        Task<bool> IsProductExistsAsync(string productId, CancellationToken cancellationToken = default);
    }
}
