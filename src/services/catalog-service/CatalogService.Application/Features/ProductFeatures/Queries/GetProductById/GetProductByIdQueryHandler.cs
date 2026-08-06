using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Application.Common;
using CatalogService.Application.DTOs;
using CatalogService.Domain.Aggregates.ProductAggregate;
using CatalogService.Domain.Interfaces;

namespace CatalogService.Application.Features.ProductFeatures.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, Result<ProductResponseDto>>
    {
        private readonly IProductRepository _productRepository;
        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<Result<ProductResponseDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductByIdAsync(ProductId.Create(Guid.Parse(request.ProductId)), cancellationToken);
            if (product == null)
            {
                return Result<ProductResponseDto>.Failure(new[] {new Error("", "Product not found" )});
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

            return Result<ProductResponseDto>.Success(response);
        }
    }
}
