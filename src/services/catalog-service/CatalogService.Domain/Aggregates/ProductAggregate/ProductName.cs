using CatalogService.Domain.Common;

namespace CatalogService.Domain.Aggregates.ProductAggregate
{
    public readonly record struct ProductName(string Value)
    {
        public static ProductName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Product name cannot be null or whitespace.", nameof(value));
            if (value.Length > 200)
                throw new ArgumentException("Product name cannot exceed 100 characters.", nameof(value));
            return new ProductName(value.Trim());
        }
    }
}
