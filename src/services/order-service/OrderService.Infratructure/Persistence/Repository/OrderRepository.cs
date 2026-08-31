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

        public async Task<Domain.Common.PagedResult<Order>> GetOrderByCustomerIdAsync(
            CustomerId customerId,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Orders
                .AsNoTracking()
                .Where(x => x.CustomerId == customerId);

            // Lấy tổng số bản ghi (chạy 1 câu lệnh COUNT riêng)
            var totalCount = await query.CountAsync(cancellationToken);

            // Lấy dữ liệu phân trang (bắt buộc phải OrderBy để dữ liệu không bị xáo trộn)
            var items = await query
                .OrderByDescending(x => x.Id) // Thay bằng cột ngày tạo hoặc Id có Index
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new Domain.Common.PagedResult<Order>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<string> AddOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _context.Orders.AddAsync(order, cancellationToken);
            return order.Id.ToString();
        }
        public async Task UpdateOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            _context.Orders.Update(order);
        }
        public async Task DeleteOrderAsync(OrderId orderId, string reason, CancellationToken cancellationToken = default)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);
            order.Cancel(reason);
        }
    }
}
