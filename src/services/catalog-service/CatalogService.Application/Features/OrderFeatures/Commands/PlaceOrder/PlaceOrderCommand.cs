using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Application.Common;
using CatalogShared.Models;
using MediatR;

namespace CatalogService.Application.Features.OrderFeatures.Commands.PlaceOrder
{
    public sealed record PlaceOrderCommand(OrderPlaceEvent Event) : ICommand<Result<Guid>>;
}
