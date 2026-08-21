using OrderService.Domain.Common;
using System.Net.NetworkInformation;

namespace OrderService.Domain.Aggregates.OrderAggregate
{
    public sealed class Order : AggregateRoot<OrderId>
    {
        public CustomerId CustomerId { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }
        public PaymentStatus PaymentStatus { get; private set; }
        public ShippingAddress ShippingAddress { get; private set; }
        public Money ShippingFee { get; private set; }
        public string? Note { get; private set; } = string.Empty;
        public string? CancellationReason { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        public DateTime? ConfirmedAt { get; private set; }
        public DateTime? CancelledAt { get; private set; }
        public DateTime? PaidAt { get; private set; }
        public List<OrderItem> Items { get; private set; } = new();

        // ─── Computed ───

        public Money ItemsTotal => Items
            .Aggregate(Money.Zero(), (acc, item) => acc + item.SubTotal);

        public Money GrandTotal => ItemsTotal + ShippingFee;

        private Order() { }
        private Order(OrderId orderId, CustomerId customerId, PaymentMethod paymentMethod, ShippingAddress shippingAddress, Money shippingFee, string? note, List<OrderItem> items)
        {
            Id = orderId;
            CustomerId = customerId;
            OrderStatus = OrderStatus.Pending;
            PaymentMethod = paymentMethod;
            PaymentStatus = PaymentStatus.Unpaid;
            ShippingAddress = shippingAddress;
            ShippingFee = shippingFee;
            Note = note;
            Items = items;
            CreatedAt = DateTime.UtcNow;
        }
        public static Order Create(CustomerId customerId, PaymentMethod paymentMethod, ShippingAddress shippingAddress, Money shippingFee, string? note, List<OrderItem> items)
        {
            var orderId = OrderId.New();
            return new Order(orderId, customerId, paymentMethod, shippingAddress, shippingFee, note, items);
        }
        public void AddOrderItem(List<OrderItem> items)
        {
            if (items != null && items.Count != 0)
            {
                Items = items;
            }
        }
        public void Confirm()
        {
            if (OrderStatus != OrderStatus.Pending)
                throw new InvalidOperationException("Only pending orders can be confirmed.");
            OrderStatus = OrderStatus.Completed;
            ConfirmedAt = DateTime.UtcNow;
        }
        public void Cancel(string cancellationReason)
        {
            if (OrderStatus != OrderStatus.Pending)
                throw new InvalidOperationException("Only pending orders can be cancelled.");
            OrderStatus = OrderStatus.Cancelled;
            CancellationReason = cancellationReason;
            CancelledAt = DateTime.UtcNow;
        }
        public void MarkAsPaid(decimal amount, string currency)
        {
            if (PaymentStatus == PaymentStatus.Unpaid)
                throw new InvalidOperationException("Order is already paid.");
            if (OrderStatus == OrderStatus.Cancelled)
                throw new InvalidOperationException("Cannot mark a cancelled order as paid.");
            PaymentStatus = PaymentStatus.Paid;
            PaidAt = DateTime.UtcNow;
        }
        public void MarkAsRefunded()
        {
            if (PaymentStatus != PaymentStatus.Paid)
                throw new InvalidOperationException("Only paid orders can be refunded.");
            PaymentStatus = PaymentStatus.Refunded;
        }
        public bool CanBeCancelledByCustomer()
            => OrderStatus == OrderStatus.Pending;
    }
}
