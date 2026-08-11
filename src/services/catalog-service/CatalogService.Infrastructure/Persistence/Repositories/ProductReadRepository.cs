using CatalogService.Application.Abstractions.Services;
using CatalogService.Application.DTOs;
using CatalogService.Domain.Aggregates.ProductAggregate;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Persistence.Repositories
{
    public class ProductReadRepository : IProductReadRepository
    {
        private readonly CatalogDbContext _context;
        public ProductReadRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductResponseDto>> GetHotProductRespository(int limit, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Where(p => p.IsPublished)
                .OrderByDescending(p => p.PublishedAt)
                .Take(limit)
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id.Value.ToString(),
                    ProductName = p.ProductName.ToString(),
                    ProductSlug = p.ProductSlug.ToString(),
                    Description = p.Description,
                    CategoryId = p.CategoryId.Value.ToString(),
                    ThumbnailUrl = p.ThumbnailUrl.ToString(),
                    ImageUrls = p.ImageUrls.Select(url => url.ToString()).ToList(),
                    IsPublished = p.IsPublished,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    PublishedAt = p.PublishedAt,
                    Variants = p.Variants.Select(v => new ProductVariantResponse
                    {
                        ProductVariantId = v.ProductVariantId.Value.ToString(),
                        ProductSku = v.ProductSku.Value.ToString(),
                        Price = new Application.DTOs.Money { Amount = v.Price.Amount, Currency = v.Price.Currency },
                        OriginalPrice = v.OriginalPrice != null ? new Application.DTOs.Money { Amount = v.OriginalPrice.Amount, Currency = v.OriginalPrice.Currency } : null,
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
                })
                .ToListAsync(cancellationToken);
        }


        public async Task<ProductResponseDto?> GetProductReadRepository(string productId, CancellationToken cancellationToken = default)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == ProductId.Create(Guid.Parse(productId)), cancellationToken);
            
            if (product == null)
            {
                return null ;
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
                    Price = new Application.DTOs.Money { Amount = v.Price.Amount, Currency = v.Price.Currency },
                    OriginalPrice = v.OriginalPrice != null ? new Application.DTOs.Money { Amount = v.OriginalPrice.Amount, Currency = v.OriginalPrice.Currency } : null,
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
            return await Task.FromResult<ProductResponseDto?>(response);
        }
    }
}
