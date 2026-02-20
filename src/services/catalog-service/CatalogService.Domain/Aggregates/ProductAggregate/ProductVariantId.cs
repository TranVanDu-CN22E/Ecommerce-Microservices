using CatalogService.Domain.Common;
using Medo;

namespace CatalogService.Domain.Aggregates.ProductAggregate
{
    public readonly record struct ProductVariantId (Guid Value)
    {
        public static ProductVariantId New() => new(Uuid7.NewUuid7());
        public static ProductVariantId Create(Guid id)
            => id == Guid.Empty ? throw new ArgumentNullException("ProductVariantId cannot be empty")
            : new(id);
    }
}
