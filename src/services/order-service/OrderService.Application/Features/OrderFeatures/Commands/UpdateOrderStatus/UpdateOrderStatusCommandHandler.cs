using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Common;
using OrderService.Domain.Aggregates.OrderAggregate;
using OrderService.Domain.Interface;

namespace OrderService.Application.Features.OrderFeatures.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandHandler : ICommandHandler<UpdateOrderStatusCommand, Result<bool>>
    {
        private readonly IOrderRepository _orderRepository;
        public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result<bool>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderByIdAsync(OrderId.Create(Guid.Parse(request.OrderId)), cancellationToken);
            if (order == null)
            {
                return Result<bool>.Failure(new[] { OrderErrors.OrderNotFound });
            }
            switch (request.OrderStatus) {
                case 1:
                    if (order.OrderStatus == OrderStatus.Pending)
                    {
                        order.Confirm();
                    }
                    else return Result<bool>.Failure(new[] { OrderErrors.OrderCannotBeConfirmed });
                    break;

                case 3: 
                    if (order.CanBeCancelledByCustomer() == false) return Result<bool>.Failure(new[] { OrderErrors.OrderCannotBeCancelledByCustomer });
                    if (request.ReasonCancel == null) return Result<bool>.Failure(new[] { OrderErrors.ReasonCancelCannotBeEmpty });
                    order.Cancel(request.ReasonCancel);
                    break;
                default:
                    return Result<bool>.Failure(new[] { OrderErrors.InvalidOrderStatus });
            }
            return Result<bool>.Success(true);
        }
    }
}
