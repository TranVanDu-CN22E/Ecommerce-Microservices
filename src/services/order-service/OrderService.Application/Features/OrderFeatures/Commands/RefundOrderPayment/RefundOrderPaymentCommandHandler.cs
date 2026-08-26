using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Common;
using OrderService.Domain.Aggregates.OrderAggregate;
using OrderService.Domain.Interface;

namespace OrderService.Application.Features.OrderFeatures.Commands.RefundOrderPayment
{
    public class RefundOrderPaymentCommandHandler : ICommandHandler<RefundOrderPaymentCommand, Result<bool>>
    {
        private readonly IOrderRepository _orderRepository;
        public RefundOrderPaymentCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
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
            return Result<bool>.Success(true);
        }
    }
}
