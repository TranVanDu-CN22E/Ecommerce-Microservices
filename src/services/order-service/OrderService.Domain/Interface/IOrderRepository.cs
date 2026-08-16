using OrderService.Domain.Aggregates.OrderAggregate;

namespace OrderService.Domain.Interface
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderByIdAsync(OrderId orderId, CancellationToken cancellationToken = default);
        Task<string> AddOrderAsync(Order order, CancellationToken cancellationToken = default);
        Task UpdateOrderAsync(Order order, CancellationToken cancellationToken = default);
        Task DeleteOrderAsync(OrderId orderId, string reason, CancellationToken cancellationToken = default);
    }
}
