using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Common;
using OrderService.Application.DTOs;
using System.Windows.Input;

namespace OrderService.Application.Features.OrderFeatures.Queries.GetOrderByUserId
{
    public sealed class GetOrderByUserIdQuery : IQuery<Result<List<GetOrderDto>>>
    {
        public string CustomerId { get; set; } = string.Empty;
        public int pageNumber { get; set; } = 1;
    }
}
