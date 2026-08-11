namespace CatalogService.Application.DTOs
{
    public sealed class ProductResponseDto
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string ProductSlug { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public string ThumbnailUrl { get; set; } // URL string, không phải IFormFile
        public List<string> ImageUrls { get; set; } = new(); // List URL strings
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public List<ProductVariantResponse> Variants { get; set; } = new();
    }

    public sealed class ProductVariantResponse
    {
        public string ProductVariantId { get; set; }
        public string ProductSku { get; set; }
        public Money Price { get; set; } // Chỉ giữ Money, bỏ decimal Price
        public Money? OriginalPrice { get; set; }
        public List<VariantAttributeResponse> Attributes { get; set; } = new(); // Bỏ underscore
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int StockQuantity { get; set; }
        public int SoldQuantity { get; set; }
        public int ReservedQuantity { get; set; } // Tổng số lượng đang mua nhưng chưa thanh toán
        public DateTime CreatedAt { get; set; }
    }
    public sealed class Money
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "VND";
    }
    public sealed class VariantAttributeResponse
    {
        public string ProductVariantAttributeId { get; set; }
        public string ProductVariantId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}