using Medo;

namespace OrderService.Domain.Aggregates.OrderAggregate
{
    public readonly record struct OrderItemId(Guid id)
    {
        public static OrderItemId NewId() => new(Uuid7.NewGuid());
        public static OrderItemId Create(Guid id) => id == Guid.Empty ? throw new ArgumentNullException("Order item id cannot be empty") : new(id);
        public override string ToString() => id.ToString();
    }
}
