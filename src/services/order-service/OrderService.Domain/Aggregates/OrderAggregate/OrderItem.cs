using System.Runtime.InteropServices;

namespace OrderService.Domain.Aggregates.OrderAggregate
{
    public sealed class OrderItem
    {
        public OrderItemId OrderItemId { get; private set; }
        public OrderId OrderId { get; private set; } = default!;
        public ProductId ProductId { get; private set; }
        public ProductVariantId ProductVariantId { get; private set; }
        public string ProductName { get; private set; } = string.Empty;
        public string VariantSku { get; private set; } = string.Empty;
        public string? VariantAttribute { get; private set; } = string.Empty;

        public Money UnitPrice { get; private set; } = default!;
        public int Quantity { get; private set; }
        public Money SubTotal => UnitPrice * Quantity;

        private OrderItem() { }
        public OrderItem(OrderItemId orderItemId, OrderId orderId, ProductId productId, ProductVariantId productVariantId, string productName, string variantSku, string? variantAttribute, Money unitPrice, int quantity)
        {
            OrderItemId = orderItemId;
            OrderId = orderId;
            ProductId = productId;
            ProductVariantId = productVariantId;
            ProductName = productName;
            VariantSku = variantSku;
            VariantAttribute = variantAttribute;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
        public static OrderItem Create(OrderId orderId, ProductId productId, ProductVariantId productVariantId, string productName, string variantSku, string? variantAttribute, Money unitPrice, int quantity)
        {
            var orderItemId = OrderItemId.NewId();
            return new OrderItem(orderItemId, orderId, productId, productVariantId, productName, variantSku, variantAttribute, unitPrice, quantity);
        }
    }
}
