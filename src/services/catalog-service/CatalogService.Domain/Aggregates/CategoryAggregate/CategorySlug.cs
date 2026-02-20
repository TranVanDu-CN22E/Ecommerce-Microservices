namespace CatalogService.Domain.Aggregates.CategoryAggregate
{
    public readonly record struct CategorySlug (string Value)
    {
        public static CategorySlug Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Category slug cannot be empty.");
            if (value.Length > 100)
                throw new ArgumentException("Category slug cannot exceed 100 characters.");
            return new CategorySlug(value.Trim().ToLowerInvariant());
        }
    }
}
