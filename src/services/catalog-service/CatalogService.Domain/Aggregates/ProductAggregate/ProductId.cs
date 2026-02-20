using Medo;

namespace CatalogService.Domain.Aggregates.ProductAggregate
{
    public readonly record struct ProductId (Guid Value)
    {
        public static ProductId New() => new(Uuid7.NewUuid7());
        public static ProductId Create(Guid id) => id == Guid.Empty ? throw new ArgumentNullException("Product id cannot be empty") : new(id);
    }
}
