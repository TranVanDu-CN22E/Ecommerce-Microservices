using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Common;
using OrderService.Application.DTOs;
using OrderService.Domain.Aggregates.OrderAggregate;
using OrderService.Domain.Interface;

namespace OrderService.Application.Features.OrderFeatures.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, Result<GetOrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<Result<GetOrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(OrderId.Create(Guid.Parse(request.OrderId)), cancellationToken);
                if (order == null) return Result<GetOrderDto>.Failure(new[] { OrderErrors.OrderNotFound });
                return Result<GetOrderDto>.Success(new GetOrderDto
                {
                    OrderId = order.Id.Value.ToString(),
                    CustomerId = order.CustomerId.Value.ToString(),
                    OrderStatus = order.OrderStatus.Id,
                    PaymentMethod = order.PaymentMethod.Id,
                    PaymentStatus = order.PaymentStatus.Id,
                    ShippingAddress = new DTOs.ShippingAddressDto
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
            }catch (Exception ex)
            {
                return Result<GetOrderDto>.Failure(new[] { new Error("GetOrderByIdQuery", ex.Message) });
            }
        }
    }
}
