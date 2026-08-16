using Medo;
using System.Reflection.Metadata.Ecma335;

namespace OrderService.Domain.Aggregates.OrderAggregate
{
    public readonly record struct ShippingAddressId (Guid id)
    {
        public static ShippingAddressId Create(Guid id) => id == Guid.Empty ? throw new ArgumentNullException("Shipping Address id cannot be empty") : new(id);
        public static ShippingAddressId New() => new (Uuid7.NewGuid());
    }
}
