using OrderService.Domain.Aggregates.OrderAggregate;
using OrderService.Domain.Common;

namespace OrderService.Domain.Interface
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderByIdAsync(OrderId orderId, CancellationToken cancellationToken = default);
        Task<PagedResult<Order>> GetOrderByCustomerIdAsync(
            CustomerId customerId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
        Task<string> AddOrderAsync(Order order, CancellationToken cancellationToken = default);
        Task UpdateOrderAsync(Order order, CancellationToken cancellationToken = default);
        Task DeleteOrderAsync(OrderId orderId, string reason, CancellationToken cancellationToken = default);
    }
}
