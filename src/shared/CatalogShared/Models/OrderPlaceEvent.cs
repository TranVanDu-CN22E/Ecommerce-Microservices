namespace CatalogShared.Models
{
    public sealed record OrderPlaceEvent
    {
        public Guid OrderId { get; set; }
        public string CustomerId { get; set; } = string.Empty;
        public int PaymentMethod { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public Money ShippingFee { get; set; }
        public string? Note { get; set; } = string.Empty;
        public List<OrderItem> OrderItems { get; set; }
        public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
    }
    public sealed record ShippingAddress
    {
        public string RecipientName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
    public sealed record OrderItem
    {
        public string ProductId { get; set; }
        public string ProductVariantId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string VariantSku { get; set; } = string.Empty;
        public string? VariantAttribute { get; set; } = string.Empty;
        public Money UnitPrice { get; set; } = default!;
        public int Quantity { get; set; }
    }
}
