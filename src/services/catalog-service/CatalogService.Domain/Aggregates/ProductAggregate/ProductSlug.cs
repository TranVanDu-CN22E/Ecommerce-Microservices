using CatalogService.Domain.Common;

namespace CatalogService.Domain.Aggregates.ProductAggregate
{
    public readonly record struct ProductSlug(string Value)
    {
        public static ProductSlug Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Product slug cannot be null or whitespace.", nameof(value));
            if (value.Length > 200)
                throw new ArgumentException("Product slug cannot exceed 200 characters.", nameof(value));
            return new ProductSlug(value.Trim().ToLowerInvariant());
        }
    }
}
