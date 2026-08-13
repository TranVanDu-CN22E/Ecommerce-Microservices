using Medo;

namespace OrderService.Domain.Aggregates.OrderAggregate
{
    public readonly record struct SellerId(Guid Value)
    {
        public static SellerId New() => new(Uuid7.NewGuid());
        public static SellerId Create(Guid id) => id == Guid.Empty ? throw new ArgumentNullException("Seller id cannot be empty") : new(id);
    }
}
