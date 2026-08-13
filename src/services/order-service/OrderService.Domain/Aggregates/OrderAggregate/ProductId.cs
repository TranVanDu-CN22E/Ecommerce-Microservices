namespace OrderService.Domain.Aggregates.OrderAggregate
{
    public readonly record struct ProductId(Guid id)
    {
        public static ProductId Create(Guid id) => id == Guid.Empty ? throw new ArgumentNullException("Product id cannot be empty") : new(id);
        public override string ToString() => id.ToString();
    }
}
