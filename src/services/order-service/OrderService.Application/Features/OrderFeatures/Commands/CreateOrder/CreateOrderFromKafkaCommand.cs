using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Common;
using CatalogShared.Models;

namespace OrderService.Application.Features.OrderFeatures.Commands.CreateOrder
{
    public sealed record CreateOrderFromKafkaCommand(OrderPlaceEvent Event) : ICommand<Result<Guid>>
    {
       
    }
}
