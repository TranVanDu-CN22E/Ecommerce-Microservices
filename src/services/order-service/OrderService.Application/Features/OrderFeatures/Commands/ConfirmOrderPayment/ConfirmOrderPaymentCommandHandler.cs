using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Common;
using OrderService.Application.Features.OrderFeatures.Commands.ConfirmOrderPayment;
using OrderService.Domain.Aggregates.OrderAggregate;
using OrderService.Domain.Interface;

namespace OrderService.Application.Features.OrderFeatures.Commands.ConfirmOrderPayment
{
    public class ConfirmOrderPaymentCommandHandler : ICommandHandler<ConfirmOrderPaymentCommand, Result<bool>>
    {
        private readonly IOrderRepository _orderRepository;
        public ConfirmOrderPaymentCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
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
            return Result<bool>.Success(true);
        }
    }
}
