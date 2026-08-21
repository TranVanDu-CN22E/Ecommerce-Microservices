using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Common;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces.GRPC;
using OrderService.Domain.Aggregates.OrderAggregate;
using OrderService.Domain.Interface;


namespace OrderService.Application.Features.OrderFeatures.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Result<Guid>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityService _identityService;
        private readonly IProductService _productService;
        public CreateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IIdentityService identityService, IProductService productService)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _identityService = identityService;
            _productService = productService;
        }
        public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // check user available
            var customerId = await _identityService.GetUserByIdAsync(request.CustomerId);
            if(customerId == null) return Result<Guid>.Failure(new[] { OrderErrors.CustomerIdInvalid }); 

            foreach (var item in request.OrderItems)
            {
                // check product available
                var product = await _productService.GetProductByIdAsync(item.ProductId, cancellationToken);
                if (product == null) return Result<Guid>.Failure(new[] { OrderErrors.ProductIdInvalid });
                // check product variant available
                ProductVariantDto productVariant = product.Variants.Find(x => x.ProductSku == item.VariantSku);
                if (productVariant == null) return Result<Guid>.Failure(new[] { OrderErrors.ProductVariantIdInvalid });
                // check product variant stock quantity
                if (productVariant.StockQuantity - productVariant.ReservedQuantity <= 0) return Result<Guid>.Failure(new[] { OrderErrors.ProductVariantOutOfStock });
            }

            // create order
            // create payment method
            var paymentMethodRquest = PaymentMethod.FromId(request.PaymentMethod);
            var order = Order.Create(
                customerId: CustomerId.Create(Guid.Parse(request.CustomerId)),
                paymentMethod: paymentMethodRquest,
                OrderService.Domain.Aggregates.OrderAggregate.ShippingAddress.Create(
                    request.ShippingAddress.RecipientName,
                    request.ShippingAddress.PhoneNumber,
                    request.ShippingAddress.AddressLine,
                    request.ShippingAddress.Ward,
                    request.ShippingAddress.District,
                    request.ShippingAddress.Province,
                    request.ShippingAddress.Country),
                OrderService.Domain.Aggregates.OrderAggregate.Money.Create(request.ShippingFee.Amount, request.ShippingFee.Currency),
                request.Note, 
                items: new List<OrderService.Domain.Aggregates.OrderAggregate.OrderItem>());
            var orderItems = new List<OrderService.Domain.Aggregates.OrderAggregate.OrderItem>();
            foreach (var item in request.OrderItems)
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

            // update product variant stock quantity
            foreach (var item in request.OrderItems)
            {
                var productVariant = product.Variants.Find(x => x.ProductSku == item.VariantSku);
                productVariant.StockQuantity -= item.Quantity;
            }
            await _orderRepository.AddOrderAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(order.Id.Value);
        }
    }
}
