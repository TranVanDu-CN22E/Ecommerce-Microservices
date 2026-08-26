using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Common;
using OrderService.Application.DTOs;

namespace OrderService.Application.Features.OrderFeatures.Queries.GetOrderById
{
    public sealed class GetOrderByIdQuery : IQuery<Result<GetOrderDto>>
    {
        public string OrderId { get; set; } = string.Empty;
    }
}
