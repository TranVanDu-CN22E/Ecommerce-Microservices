using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Common;
using System.Windows.Input;

namespace OrderService.Application.Features.OrderFeatures.Commands.ConfirmOrderPayment
{
    public sealed record ConfirmOrderPaymentCommand : ICommand<Result<bool>>
    {
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public  string Currency { get; set; }
    }
}
