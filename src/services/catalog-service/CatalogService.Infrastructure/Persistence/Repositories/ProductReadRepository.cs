using CatalogService.Application.Abstractions.Services;
using CatalogService.Application.DTOs;
using CatalogService.Domain.Aggregates.ProductAggregate;

namespace CatalogService.Infrastructure.Persistence.Repositories
{
    public class ProductReadRepository : IProductReadRepository
    {
        private readonly CatalogDbContext _context;
        public ProductReadRepository(CatalogDbContext context)
        {
            _context = context;
        }
        public Task<ProductResponseDto?> GetProductReadRepository(string productId, CancellationToken cancellationToken = default)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == ProductId.Create(Guid.Parse(productId)));
            
            if (product == null)
            {
                return Task.FromResult<ProductResponseDto?>(null);
            }
            var response = new ProductResponseDto
            {
                Id = product.Id.Value.ToString(),
                ProductName = product.ProductName.ToString(),
                ProductSlug = product.ProductSlug.ToString(),
                Description = product.Description,
                CategoryId = product.CategoryId.Value.ToString(),
                ThumbnailUrl = product.ThumbnailUrl.ToString(),
                ImageUrls = product.ImageUrls.Select(url => url.ToString()).ToList(),
                IsPublished = product.IsPublished,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                PublishedAt = product.PublishedAt,
                Variants = product.Variants.Select(v => new ProductVariantResponse
                {
                    ProductVariantId = v.ProductVariantId.Value.ToString(),
                    ProductSku = v.ProductSku.Value.ToString(),
                    Price = Money.Create(v.Price.Amount, v.Price.Currency),
                    OriginalPrice = v.OriginalPrice != null ? Money.Create(v.OriginalPrice.Amount, v.OriginalPrice.Currency) : null,
                    Attributes = v.Attributes.Select(a => new VariantAttributeResponse
                    {
                        ProductVariantAttributeId = a.ProductVariantAttributeId.ToString(),
                        ProductVariantId = a.ProductVariantId.Value.ToString(),
                        Name = a.Name,
                        Value = a.Value
                    }).ToList(),
                    ImageUrl = v.ImageUrl,
                    IsActive = v.IsActive,
                    StockQuantity = v.StockQuantity,
                    SoldQuantity = v.SoldQuantity,
                    ReservedQuantity = v.ReservedQuantity,
                    CreatedAt = v.CreatedAt
                }).ToList()
            };
            return Task.FromResult<ProductResponseDto?>(response);
        }
    }
}
