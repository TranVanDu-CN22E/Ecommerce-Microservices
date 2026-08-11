namespace CatalogShared.Models
{
    public sealed class ProductGrpcModel
    {
        public string Id { get; init; } = string.Empty;
        public string ProductName { get; init; } = string.Empty;
        public string ProductSlug { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string CategoryId { get; init; } = string.Empty;
        public string ThumbnailUrl { get; init; } = string.Empty; // URL string, không phải IFormFile
        public List<string> ImageUrls { get; init; } = new(); // List URL strings
        public bool IsPublished { get; init; } = false;
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public DateTime? PublishedAt { get; init; }
        public List<ProductVariantGrpcModel> Variants { get; init; } = new();
    }
    public sealed class ProductVariantGrpcModel
    {
        public string ProductVariantId { get; init; } = string.Empty;
        public string ProductSku { get; init; } = string.Empty;
        public Money Price { get; init; } // Chỉ giữ Money, bỏ decimal Price
        public Money? OriginalPrice { get; init; }
        public List<VariantAttributeGrpcModel> Attributes { get; init; } = new();
        public string? ImageUrl { get; init; } = null;
        public bool IsActive { get; init; } = true;
        public int StockQuantity { get; init; } = 0;
        public int SoldQuantity { get; init; } = 0; // Tổng số lượng đã bán
        public int ReservedQuantity { get; init; } = 0; // Tổng số lượng đang mua nhưng chưa thanh toán
        public DateTime CreatedAt { get; init; }
    }
    public sealed class Money
    {
        public decimal Amount { get; init; } = decimal.Zero;
        public string Currency { get; init; } = string.Empty;
    }
    public sealed class VariantAttributeGrpcModel
    {
        public string ProductVariantAttributeId { get; init; } = string.Empty;
        public string ProductVariantId { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Value { get; init; } = string.Empty;
    }
}
