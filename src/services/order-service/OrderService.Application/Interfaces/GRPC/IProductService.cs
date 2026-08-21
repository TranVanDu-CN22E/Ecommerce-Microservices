using OrderService.Application.DTOs;

namespace OrderService.Application.Interfaces.GRPC
{
    public interface IProductService
    {
        Task<GetProductDto?> GetProductByIdAsync(string productId, CancellationToken cancellationToken = default);
    }
}
