using CatalogService.Domain.Common;

namespace CatalogService.Domain.Aggregates.ProductAggregate
{
    public readonly record struct ProductSku (string Value)
    {
        public static ProductSku Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Product SKU cannot be null or whitespace.", nameof(value));
            if (value.Length > 50)
                throw new ArgumentException("Product SKU cannot exceed 50 characters.", nameof(value));
            return new ProductSku(value.ToUpperInvariant().Trim());
        }
    }
}
