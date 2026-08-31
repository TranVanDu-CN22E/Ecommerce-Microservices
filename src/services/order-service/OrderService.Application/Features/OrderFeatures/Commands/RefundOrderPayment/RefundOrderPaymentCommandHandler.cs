using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Abstractions.Services;
using OrderService.Application.Common;
using OrderService.Domain.Aggregates.OrderAggregate;
using OrderService.Domain.Interface;

namespace OrderService.Application.Features.OrderFeatures.Commands.RefundOrderPayment
{
    public class RefundOrderPaymentCommandHandler : ICommandHandler<RefundOrderPaymentCommand, Result<bool>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderCacheService _redisOrderCacheService;
        public RefundOrderPaymentCommandHandler(IOrderRepository orderRepository, IOrderCacheService redisOrderCacheService)
        {
            _orderRepository = orderRepository;
            _redisOrderCacheService = redisOrderCacheService;
        }
        public async Task<Result<bool>> Handle(RefundOrderPaymentCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderByIdAsync(OrderId.Create(Guid.Parse(request.OrderId)));
            if (order == null)
            {
                return Result<bool>.Failure(new[] { OrderErrors.OrderNotFound });
            }
            order.MarkAsRefunded();
            await _orderRepository.UpdateOrderAsync(order);

            // Delete order from Redis if it exists.
            bool checkOrderInRedis = await _redisOrderCacheService.ExistsOrderAsync(order.Id.ToString(), cancellationToken);
            if (checkOrderInRedis == true)
            {
                await _redisOrderCacheService.RemoveOrderAsync(order.Id.ToString(), cancellationToken);
            }
            return Result<bool>.Success(true);
        }
    }
}
