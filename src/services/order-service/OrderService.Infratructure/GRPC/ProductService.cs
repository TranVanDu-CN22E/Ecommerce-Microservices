using CatalogShared.Protos;
using OrderService.Application.DTOs;

namespace OrderService.Infratructure.GRPC
{
    public sealed class ProductService : OrderService.Application.Interfaces.GRPC.IProductService
    {
        private readonly ProductGrpcService.ProductGrpcServiceClient _productGrpcServiceClient;
        public ProductService(ProductGrpcService.ProductGrpcServiceClient productGrpcServiceClient)
        {
            _productGrpcServiceClient = productGrpcServiceClient;
        }
        public async Task<GetProductDto?> GetProductByIdAsync(string productId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _productGrpcServiceClient.GetProductByIdAsync(new GetProductByIdRequest { ProductId = productId }, cancellationToken: cancellationToken);
                if (response.Product != null)
                {
                    return new GetProductDto
                    {
                        Id = response.Product.Id,
                        ProductName = response.Product.ProductName,
                        ProductSlug = response.Product.ProductSlug,
                        Description = response.Product.Description,
                        CategoryId = response.Product.CategoryId,
                        ThumbnailUrl = response.Product.ThumbnailUrl,
                        ImageUrls = response.Product.ImageUrls.ToList(),
                        IsPublished = response.Product.IsPublished,
                        CreatedAt = response.Product.CreatedAt.ToDateTime(),
                        UpdatedAt = response.Product.UpdatedAt.ToDateTime(),
                        PublishedAt = response.Product.PublishedAt.ToDateTime(),
                        Variants = response.Product.Variants.Select(v => new Application.DTOs.ProductVariantDto
                        {
                            ProductVariantId = v.ProductVariantId,
                            ProductSku = v.ProductSku,
                            Price = new Application.DTOs.Money { Amount = CatalogShared.Extensions.GrpcMappingExtensions.ToDecimal(v.Price.Amount.ToByteArray()), Currency = v.Price.Currency },
                            OriginalPrice = new Application.DTOs.Money { Amount = CatalogShared.Extensions.GrpcMappingExtensions.ToDecimal(v.OriginalPrice.Amount.ToByteArray()), Currency = v.OriginalPrice.Currency },
                            Attributes = v.VariantAttributes.Select(av => new Application.DTOs.VariantAttributeDto
                            {
                                ProductVariantAttributeId = av.ProductVariantAttributeId.ToString(),
                                ProductVariantId = av.ProductVariantId.ToString(),
                                Name = av.Name,
                                Value = av.Value
                            }).ToList() ?? new List<Application.DTOs.VariantAttributeDto>(),
                            ImageUrl = v.ImageUrl,
                            IsActive = v.IsActive,
                            StockQuantity = v.StockQuantity,
                            SoldQuantity = v.SoldQuantity,
                            ReservedQuantity = v.ReservedQuantity,
                            CreatedAt = v.CreatedAt.ToDateTime(),
                        }).ToList() ?? new List<Application.DTOs.ProductVariantDto>()
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use your preferred logging framework)
                Console.WriteLine($"Error in GetUserByIdAsync: {ex.Message}");
                return null;
            }
        }
    }
}
