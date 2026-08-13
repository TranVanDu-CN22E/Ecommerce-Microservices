namespace OrderService.Domain.Aggregates.OrderAggregate
{
    public readonly record struct ProductVariantId(Guid id)
    {
        public static ProductVariantId Create(Guid id) => id == Guid.Empty ? throw new ArgumentNullException("Product variant id cannot be empty") : new(id);
        public override string ToString() => id.ToString();
    }
}
