namespace OrderService.Domain.Aggregates.OrderAggregate
{
    public sealed class OrderItem
    {
        public OrderItemId OrderItemId { get; private set; }
        public ProductId ProductId { get; private set; }
        public ProductVariantId ProductVariantId { get; private set; }
        public string ProductName { get; private set; } = string.Empty;
        public string VariantSku { get; private set; } = string.Empty;
        public string? VariantAttribute { get; private set; } = string.Empty;

        public Money UnitPrice { get; private set; } = default!;
        public int Quantity { get; private set; }
        public Money SubTotal => UnitPrice * Quantity;

        private OrderItem() { }
        public OrderItem(OrderItemId orderItemId, ProductId productId, ProductVariantId productVariantId, string productName, string variantSku, string? variantAttribute, Money unitPrice, int quantity)
        {
            OrderItemId = orderItemId;
            ProductId = productId;
            ProductVariantId = productVariantId;
            ProductName = productName;
            VariantSku = variantSku;
            VariantAttribute = variantAttribute;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
    }
}
