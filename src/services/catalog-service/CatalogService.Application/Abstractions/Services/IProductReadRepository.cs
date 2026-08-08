using CatalogService.Application.DTOs;

namespace CatalogService.Application.Abstractions.Services
{
    public interface IProductReadRepository
    {
        Task<ProductResponseDto?> GetProductReadRepository(string productId, CancellationToken cancellationToken = default);
    }
}
