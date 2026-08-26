using OrderService.Application.Abstractions.Messaging;

namespace OrderService.Application.Features.OrderFeatures.Commands.UpdateOrderStatus
{
    public sealed record UpdateOrderStatusCommand : ICommand<Common.Result<bool>>
    {
        public string OrderId { get; set; }
        public int OrderStatus { get; set; }
        public string? ReasonCancel { get; set; } // not null khi status = cancel
    }
}
