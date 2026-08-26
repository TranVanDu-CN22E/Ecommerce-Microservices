using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Common;

namespace OrderService.Application.Features.OrderFeatures.Commands.RefundOrderPayment
{
    public sealed class RefundOrderPaymentCommand : ICommand<Result<bool>>
    {
        public string OrderId { get; set; }
    }
}
