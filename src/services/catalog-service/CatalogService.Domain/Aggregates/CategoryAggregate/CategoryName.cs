namespace CatalogService.Domain.Aggregates.CategoryAggregate
{
    public readonly record struct CategoryName(string Value)
    {
        public static CategoryName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Category name cannot be empty.");
            if (value.Length > 100)
                throw new ArgumentException("Category name cannot exceed 100 characters.");
            return new CategoryName(value.Trim());
        }
    }
}
