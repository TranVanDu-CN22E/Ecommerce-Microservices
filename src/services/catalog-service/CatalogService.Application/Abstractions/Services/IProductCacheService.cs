using CatalogService.Application.DTOs;

namespace CatalogService.Application.Abstractions.Services
{
    public interface IProductCacheService
    {
        Task<ProductResponseDto?> GetProductAsync(string productId, CancellationToken cancellationToken = default);
        Task<bool> SetProductAsync(string productId, ProductResponseDto product, CancellationToken cancellationToken = default);
        Task<bool> RefreshTtlAsync(string productId, CancellationToken cancellationToken = default);
        Task ReleaseReservedAsync(string productId, string variantId, int quantity);
        Task<bool> TryReserveStockAsync(string productId, string variantId, int quantity);
        Task ConfirmPurchaseAsync(string productId, string variantId, int quantity);
    }
}
