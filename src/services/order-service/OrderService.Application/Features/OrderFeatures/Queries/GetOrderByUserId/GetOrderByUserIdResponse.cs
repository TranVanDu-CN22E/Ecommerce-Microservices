using OrderService.Application.DTOs;

namespace OrderService.Application.Features.OrderFeatures.Queries.GetOrderByUserId
{
    public sealed record GetOrderByUserIdResponse
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
    }
}
