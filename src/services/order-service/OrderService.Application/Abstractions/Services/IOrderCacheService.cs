using OrderService.Application.DTOs;

namespace OrderService.Application.Abstractions.Services
{
    public interface IOrderCacheService
    {
        Task<List<GetOrderDto>> GetOrdersByUserIdAsync(string userId, CancellationToken cancellationToken);
        Task<bool> SetOrderAsync(string orderId, GetOrderDto orderDto, CancellationToken cancellationToken);
        Task<bool> SetListOrderAsync(List<GetOrderDto> orderDtos, CancellationToken cancellationToken);
        Task<bool> ExistsOrderAsync(string orderId, CancellationToken cancellationToken);
        Task<bool> RemoveOrderAsync(string orderId, CancellationToken cancellationToken);
    }
}
