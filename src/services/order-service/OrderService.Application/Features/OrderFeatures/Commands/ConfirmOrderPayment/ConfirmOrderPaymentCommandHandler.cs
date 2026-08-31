using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Abstractions.Services;
using OrderService.Application.Common;
using OrderService.Application.Features.OrderFeatures.Commands.ConfirmOrderPayment;
using OrderService.Domain.Aggregates.OrderAggregate;
using OrderService.Domain.Interface;

namespace OrderService.Application.Features.OrderFeatures.Commands.ConfirmOrderPayment
{
    public class ConfirmOrderPaymentCommandHandler : ICommandHandler<ConfirmOrderPaymentCommand, Result<bool>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderCacheService _redisOrderCacheService;
        public ConfirmOrderPaymentCommandHandler(IOrderRepository orderRepository, IOrderCacheService redisOrderCacheService)
        {
            _orderRepository = orderRepository;
            _redisOrderCacheService = redisOrderCacheService;
        }
        public async Task<Result<bool>> Handle(ConfirmOrderPaymentCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderByIdAsync(OrderId.Create(Guid.Parse(request.OrderId)), cancellationToken);
            if (order == null)
            {
                return Result<bool>.Failure(new[] { OrderErrors.OrderNotFound });
            }
            if (order.GrandTotal.Amount != request.Amount || order.GrandTotal.Currency != request.Currency) return Result<bool>.Failure(new[] { OrderErrors.PaymentAmountMismatch });
            order.MarkAsPaid(
                amount: request.Amount,
                currency: request.Currency
                );
            await _orderRepository.UpdateOrderAsync(order, cancellationToken);

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
