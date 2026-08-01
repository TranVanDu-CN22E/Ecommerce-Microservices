namespace CatalogService.Application.DTOs
{
    public sealed record VariantAttributeDto
    {
        public string Name { get; init; } = string.Empty;
        public string Value { get; init; } = string.Empty;
        public int? StockQuantity { get; init; }
    }
}
