using CatalogShared.Models;
using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Abstractions.Services;
using OrderService.Application.Common;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces.GRPC;
using OrderService.Domain.Aggregates.OrderAggregate;
using OrderService.Domain.Interface;


namespace OrderService.Application.Features.OrderFeatures.Commands.CreateOrder
{
    public class CreateOrderFromKafkaCommandHandler : ICommandHandler<CreateOrderFromKafkaCommand, Result<Guid>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IInboxRepository _inboxRepository;
        public CreateOrderFromKafkaCommandHandler(IOrderRepository orderRepository, IInboxRepository inboxRepository)
        {
            _orderRepository = orderRepository;;
            _inboxRepository = inboxRepository;
        }
        public async Task<Result<Guid>> Handle(CreateOrderFromKafkaCommand request, CancellationToken cancellationToken)
        {
            var @event = request.Event;

            //  Validate IDs trước khi làm bất cứ gì
            if (!Guid.TryParse(@event.CustomerId, out var customerId))
                return Result<Guid>.Failure(new[] {
                new Error("Order.InvalidCustomerId", $"Invalid CustomerId: {@event.CustomerId}")
            });

            await _inboxRepository.AddAsync(new InboxMessageDto(
                @event.OrderId,
                nameof(OrderPlaceEvent),
                System.Text.Json.JsonSerializer.Serialize(@event),
                DateTime.UtcNow), cancellationToken);

            // create order
            // create payment method
            var paymentMethodRquest = PaymentMethod.FromId(@event.PaymentMethod);
            var order = Order.Create(
                orderId: OrderId.Create(@event.OrderId),
                customerId: CustomerId.Create(Guid.Parse(@event.CustomerId)),
                paymentMethod: paymentMethodRquest,
                OrderService.Domain.Aggregates.OrderAggregate.ShippingAddress.Create(
                    @event.ShippingAddress.RecipientName,
                    @event.ShippingAddress.PhoneNumber,
                    @event.ShippingAddress.AddressLine,
                    @event.ShippingAddress.Ward,
                    @event.ShippingAddress.District,
                    @event.ShippingAddress.Province,
                    @event.ShippingAddress.Country),
                OrderService.Domain.Aggregates.OrderAggregate.Money.Create(@event.ShippingFee.Amount, @event.ShippingFee.Currency),
                @event.Note,
                items: new List<OrderService.Domain.Aggregates.OrderAggregate.OrderItem>());
            var orderItems = new List<OrderService.Domain.Aggregates.OrderAggregate.OrderItem>();
            foreach (var item in @event.OrderItems)
            {
                var orderItem = OrderService.Domain.Aggregates.OrderAggregate.OrderItem.Create(
                    order.Id,
                    productId: ProductId.Create(Guid.Parse(item.ProductId)),
                    productVariantId: ProductVariantId.Create(Guid.Parse(item.ProductVariantId)),
                    productName: item.ProductName,
                    variantSku: item.VariantSku,
                    variantAttribute: item.VariantAttribute,
                    unitPrice: OrderService.Domain.Aggregates.OrderAggregate.Money.Create(item.UnitPrice.Amount, item.UnitPrice.Currency),
                    quantity: item.Quantity);
                orderItems.Add(orderItem);
            }
            order.AddOrderItem(orderItems);
            await _orderRepository.AddOrderAsync(order, cancellationToken);
            return Result<Guid>.Success(order.Id.Value);
        }
    }
}
