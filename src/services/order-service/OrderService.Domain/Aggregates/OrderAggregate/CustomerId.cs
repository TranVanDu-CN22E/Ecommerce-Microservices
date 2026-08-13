using Medo;

namespace OrderService.Domain.Aggregates.OrderAggregate
{
    public readonly record struct CustomerId(Guid Value)
    {
        public static CustomerId New() => new(Uuid7.NewGuid());
        public static CustomerId Create(Guid id) => id == Guid.Empty ? throw new ArgumentNullException("Customer id cannot be empty") : new(id);
    }
}
