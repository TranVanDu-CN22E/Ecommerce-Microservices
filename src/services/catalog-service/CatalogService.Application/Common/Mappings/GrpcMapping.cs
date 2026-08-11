using CatalogService.Application.DTOs;
using CatalogShared.Models;
using CatalogShared.Protos;
namespace CatalogService.Application.Common.Mappings
{
    public static class GrpcMapping
    {
        public static ProductGrpcModel ToSharedModel(this ProductResponseDto response)
        {
            if (response == null) return null;
            var productModel = new ProductGrpcModel
            {
                Id = response.Id,
                ProductName = response.ProductName,
                ProductSlug = response.ProductSlug,
                Description = response.Description,
                CategoryId = response.CategoryId,
                ThumbnailUrl = response.ThumbnailUrl,
                IsPublished = response.IsPublished,
                CreatedAt = response.CreatedAt,
                UpdatedAt = response.UpdatedAt,
                PublishedAt = response.PublishedAt,
                ImageUrls = response.ImageUrls ?? new List<string>(),
            };
            foreach (var variant in response.Variants ?? new List<ProductVariantResponse>())
            {
                var variantDto = new ProductVariantGrpcModel
                {
                    ProductVariantId = variant.ProductVariantId,
                    ProductSku = variant.ProductSku,
                    Price = new CatalogShared.Models.Money
                    {
                        Amount = variant.Price.Amount,
                        Currency = variant.Price.Currency
                    },
                    OriginalPrice = variant.OriginalPrice != null
                        ? new CatalogShared.Models.Money
                        {
                            Amount = variant.OriginalPrice.Amount,
                            Currency = variant.OriginalPrice.Currency
                        }
                        : null,
                    ImageUrl = variant.ImageUrl ?? string.Empty,
                    IsActive = variant.IsActive,
                    StockQuantity = variant.StockQuantity,
                    SoldQuantity = variant.SoldQuantity,
                    ReservedQuantity = variant.ReservedQuantity,
                    CreatedAt = variant.CreatedAt,
                };

                foreach (var attribute in variant.Attributes)
                {
                    variantDto.Attributes.Add(new VariantAttributeGrpcModel
                    {
                        ProductVariantAttributeId = attribute.ProductVariantAttributeId,
                        ProductVariantId = attribute.ProductVariantId,
                        Name = attribute.Name,
                        Value = attribute.Value
                    });
                }

                productModel.Variants.Add(variantDto);
            }
            return productModel;
        }
    }
}
