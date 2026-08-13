using Medo;

namespace OrderService.Domain.Aggregates.OrderAggregate
{
    public readonly record struct OrderId(Guid Value)
    {
        public static OrderId New() => new(Uuid7.NewGuid());
        public static OrderId Create(Guid id) => id == Guid.Empty ? throw new ArgumentNullException("Order id cannot be empty") : new(id);
    }
}
