namespace OrderService.Application.DTOs
{
    public sealed record GetOrderDto
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public int OrderStatus { get; set; }
        public int PaymentMethod { get; set; }
        public int PaymentStatus { get; set; }
        public ShippingAddressDto ShippingAddress { get; set; }
        public Money ShippingFee { get; set; }
        public string? Note { get; set; } = string.Empty;
        public string? CancellationReason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
    public sealed record ShippingAddressDto
    {
        public string RecipientName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
    public sealed record OrderItemDto
    {
        public string OrderItemId { get; set; }
        public string OrderId { get; set; } = default!;
        public string ProductId { get; set; }
        public string ProductVariantId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string VariantSku { get; set; } = string.Empty;
        public string? VariantAttribute { get; set; } = string.Empty;

        public Money UnitPrice { get; set; } = default!;
        public int Quantity { get; set; }
        public Money SubTotal {  get; set; } = default!;
    }
}
