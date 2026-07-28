namespace CatalogService.Application.DTOs
{
    public sealed record CreateProductVariantDto
    {
        public string Sku { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public decimal? OriginalPrice { get; init; }
        public string? Currency { get; init; } = "VND"; // Mặc định VND hoặc lấy từ config
        public List<VariantAttributeDto>? Attributes { get; init; }
        public IFormFile? Image { get; init; }
    }
}
