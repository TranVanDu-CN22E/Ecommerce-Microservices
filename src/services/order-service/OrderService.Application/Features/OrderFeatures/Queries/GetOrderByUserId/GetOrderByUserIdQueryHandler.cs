using MediatR;
using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Abstractions.Services;
using OrderService.Application.Common;
using OrderService.Application.DTOs;
using OrderService.Domain.Aggregates.OrderAggregate;
using OrderService.Domain.Interface;

namespace OrderService.Application.Features.OrderFeatures.Queries.GetOrderByUserId
{
    public class GetOrderByUserIdQueryHandler : IQueryHandler<GetOrderByUserIdQuery, Result<List<GetOrderDto>>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderCacheService _redisOrderCacheService;
        public GetOrderByUserIdQueryHandler(IOrderRepository orderRepository, IOrderCacheService kafkaOrderCacheService)
        {
            _orderRepository = orderRepository;
            _redisOrderCacheService = kafkaOrderCacheService;
        }

        public async Task<Result<List<GetOrderDto>>> Handle(GetOrderByUserIdQuery request, CancellationToken cancellationToken)
        {
            var ordersCache = await _redisOrderCacheService.GetOrdersByUserIdAsync(request.CustomerId, cancellationToken);
            
            if (ordersCache != null && ordersCache.Count() > 0)
            {
                return Result<List<GetOrderDto>>.Success(ordersCache.ToList());
            }

            var orders = await _orderRepository.GetOrderByCustomerIdAsync(CustomerId.Create(Guid.Parse(request.CustomerId)),request.pageNumber, 10, cancellationToken);
            if (orders == null || orders.TotalCount == 0)
            {
                return Result<List<GetOrderDto>>.Failure(new[] { OrderErrors.OrderNotFound });
            }
            var response = new List<GetOrderDto>();
            foreach (var order in orders.Items)
            {
                response.Add(new GetOrderDto
                {
                    OrderId = order.Id.ToString(),
                    CustomerId = order.CustomerId.ToString(),
                    OrderStatus = order.OrderStatus.Id,
                    PaymentMethod = order.PaymentMethod.Id,
                    PaymentStatus = order.PaymentStatus.Id,
                    ShippingAddress = new ShippingAddressDto
                    {
                        RecipientName = order.ShippingAddress.RecipientName,
                        PhoneNumber = order.ShippingAddress.PhoneNumber,
                        AddressLine = order.ShippingAddress.AddressLine,
                        Ward = order.ShippingAddress.Ward,
                        District = order.ShippingAddress.District,
                        Province = order.ShippingAddress.Province,
                        Country = order.ShippingAddress.Country,
                    },
                    ShippingFee = new DTOs.Money
                    {
                        Amount = order.ShippingFee.Amount,
                        Currency = order.ShippingFee.Currency,
                    },
                    Note = order.Note,
                    CancellationReason = order.CancellationReason,
                    CreatedAt = order.CreatedAt,
                    ConfirmedAt = order.ConfirmedAt,
                    CancelledAt = order.CancelledAt,
                    PaidAt = order.PaidAt,
                    Items = order.Items.Select(item => new DTOs.OrderItemDto
                    {
                        OrderItemId = item.OrderItemId.ToString(),
                        OrderId = item.OrderItemId.ToString(),
                        ProductId = item.ProductId.ToString(),
                        ProductVariantId = item.ProductVariantId.ToString(),
                        ProductName = item.ProductName.ToString(),
                        VariantSku = item.VariantSku.ToString(),
                        VariantAttribute = item.VariantAttribute.ToString(),
                        UnitPrice = new DTOs.Money
                        {
                            Amount = item.UnitPrice.Amount,
                            Currency = item.UnitPrice.Currency,
                        },
                        Quantity = item.Quantity,
                        SubTotal = new DTOs.Money
                        {
                            Amount = item.SubTotal.Amount,
                            Currency = item.SubTotal.Currency,
                        }
                    }).ToList()
                });
            }
            await _redisOrderCacheService.SetListOrderAsync(response, cancellationToken);
            return Result<List<GetOrderDto>>.Success(response);
        }
    }
}
