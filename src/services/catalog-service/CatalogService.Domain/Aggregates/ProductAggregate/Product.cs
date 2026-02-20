using CatalogService.Domain.Aggregates.CategoryAggregate;
using CatalogService.Domain.Common;

namespace CatalogService.Domain.Aggregates.ProductAggregate
{
    public sealed class Product : AggregateRoot<ProductId>
    {
        public ProductName ProductName { get; private set; }
        public ProductSlug ProductSlug { get; private set; } // Unique slug for URL generation eg:website.com/ao-thun-nam-co-be
        public string Description { get; private set; }
        public CategoryId CategoryId { get; private set; }
        public string ThumbnailUrl { get; private set; } // Main image for the product
        public List<string> ImageUrls { get; private set; } = new(); // Additional images for the product
        public bool IsPublished { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? PublishedAt { get; private set; }
        public List<ProductVariant> Variants { get; private set; } = new();

        private Product() { }
        private Product(ProductId id, ProductName productName, ProductSlug productSlug, string description, CategoryId categoryId, string thumbnailUrl)
        {
            Id = id;
            ProductName = productName;
            ProductSlug = productSlug;
            Description = description;
            CategoryId = categoryId;
            ThumbnailUrl = thumbnailUrl;
            IsPublished = false;
            CreatedAt = DateTime.UtcNow;
        }

        public static Product Create(
            ProductName productName,
            ProductSlug productSlug,
            string description,
            CategoryId categoryId,
            string thumbnailUrl
            )
        {
            return new Product(
                ProductId.New(),
                productName,
                productSlug,
                description,
                categoryId,
                thumbnailUrl
                );
        }
        public void Update(
            ProductName? productName,
            ProductSlug? productSlug,
            string? description,
            CategoryId? categoryId,
            string? thumbnailUrl
            )
        {
            if (productName is not null) ProductName = productName.Value;
            if (productSlug is not null) ProductSlug = productSlug.Value;
            if (description is not null) Description = description;
            if (categoryId is not null) CategoryId = categoryId.Value;
            if (categoryId is not null) CategoryId = categoryId.Value;
            if (thumbnailUrl is not null) ThumbnailUrl = thumbnailUrl;
            UpdatedAt = DateTime.UtcNow;
        }
        public void Publish()
        {
            if (IsPublished) return;
            if (!Variants.Any(v => v.IsActive))
                throw new InvalidOperationException("Cannot publish a product with no active variants.");

            IsPublished = true;
            PublishedAt = DateTime.UtcNow;
        }
        public void Unpublish()
        {
            if (!IsPublished) return;
            IsPublished = false;
        }
        public ProductVariant AddVariant(ProductSku sku, Money price, Money? originalPrice, IEnumerable<VariantAttribute> attributes, string? imageUrl)
        {
            if (Variants.Any(v => v.Sku == sku))
                throw new InvalidOperationException($"Variant with SKU '{sku.Value}' already exists.");

            var variant = new ProductVariant(ProductVariantId.New(), sku, price, originalPrice, attributes, imageUrl, true);
            Variants.Add(variant);
            return variant;
        }
        public void RemoveVariant(ProductVariantId variantId)
        {
            var variant = GetVariantOrThrow(variantId);
            if (Variants.Count == 1)
                throw new InvalidOperationException("Cannot remove the last variant from a product.");

            if (variant is null)
                throw new ArgumentException("Variant not found", nameof(variantId));
            Variants.Remove(variant);
        }
        public void UpdateVariantPrice(ProductVariantId variantId, Money? price, Money? originalPrice)
        {
            var variant = GetVariantOrThrow(variantId);
            variant.UpdatePrice(price, originalPrice);
        }
        public void AddImages(IEnumerable<string> imageUrls)
        {
            foreach (var url in imageUrls)
            {
                if (!string.IsNullOrWhiteSpace(url) && !ImageUrls.Contains(url))
                    ImageUrls.Add(url);
            }
        }
        public void RemoveImage(string imageUrl)
        {
            if (ImageUrls.Contains(imageUrl))
                ImageUrls.Remove(imageUrl);
        }
        public Money GetLowestPrice()
        {
            var activeVariants = Variants.Where(v => v.IsActive).ToList();
            if (!activeVariants.Any())
                throw new InvalidOperationException("No active variants available to determine price.");
            return activeVariants.Min(v => v.Price);
        }
        private ProductVariant GetVariantOrThrow(ProductVariantId variantId)
            => Variants.FirstOrDefault(v => v.Id == variantId)
               ?? throw new InvalidOperationException($"Variant '{variantId.Value}' not found.");
    }
}
