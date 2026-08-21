using CatalogShared.Models;

namespace CatalogService.Api.Models.Requests
{
    public class PlaceOrderRequest
    {
        public Guid OrderId { get; set; }
        public int PaymentMethod { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public Money ShippingFee { get; set; }
        public string? Note { get; set; } = string.Empty;
        public List<OrderItem> OrderItems { get; set; }
    }
}
