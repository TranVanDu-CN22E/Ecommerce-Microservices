using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Aggregates.OrderAggregate;
using OrderService.Domain.Interface;

namespace OrderService.Infratructure.Persistence.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;
        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }
        public async Task<Order?> GetOrderByIdAsync(OrderId orderId, CancellationToken cancellationToken = default)
        {
            return await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);
        }
        public async Task<string> AddOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _context.Orders.AddAsync(order, cancellationToken);
            return order.Id.ToString();
        }
        public async Task UpdateOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync(); 
        }
        public async Task DeleteOrderAsync(OrderId orderId, string reason, CancellationToken cancellationToken = default)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);
            order.Cancel(reason);
            await _context.SaveChangesAsync();
        }
    }
}
