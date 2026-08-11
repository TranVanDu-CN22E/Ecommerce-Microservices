using CatalogShared.Models;
using CatalogShared.Protos;

namespace CatalogShared.Extensions
{
    public static class GrpcMappingExtensions
    {
        public static ProductDto ToProto(this ProductGrpcModel model)
        {
            if (model == null) return null;
            var dto = new ProductDto
            {
                Id = model.Id,
                ProductName = model.ProductName,
                ProductSlug = model.ProductSlug,
                Description = model.Description,
                CategoryId = model.CategoryId,
                ThumbnailUrl = model.ThumbnailUrl,
                IsPublished = model.IsPublished,
                CreatedAt = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(model.CreatedAt.ToUniversalTime()),
                UpdatedAt = model.UpdatedAt.HasValue
                    ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(model.UpdatedAt.Value.ToUniversalTime())
                    : null,
                PublishedAt = model.PublishedAt.HasValue
                    ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(model.PublishedAt.Value.ToUniversalTime())
                    : null
            };
            // Mapping nested list of image URLs
            dto.ImageUrls.AddRange(model.ImageUrls);
            // Mapping nested list of variants
            foreach (var variant in model.Variants)
            {
                var variantDto = new ProductVariantDto
                {
                    ProductVariantId = variant.ProductVariantId,
                    ProductSku = variant.ProductSku,
                    Price = new Protos.Money
                    {
                        Amount = Google.Protobuf.ByteString.CopyFrom(ToBytes(variant.Price.Amount)),
                        Currency = variant.Price.Currency
                    },
                    OriginalPrice = variant.OriginalPrice != null
                        ? new Protos.Money
                        {
                            Amount = Google.Protobuf.ByteString.CopyFrom(ToBytes(variant.OriginalPrice.Amount)),
                            Currency = variant.OriginalPrice.Currency
                        }
                        : null,
                    ImageUrl = variant.ImageUrl ?? string.Empty,
                    IsActive = variant.IsActive,
                    StockQuantity = variant.StockQuantity,
                    SoldQuantity = variant.SoldQuantity,
                    ReservedQuantity = variant.ReservedQuantity,
                    CreatedAt = Google.Protobuf.WellKnownTypes.Timestamp
                        .FromDateTime(variant.CreatedAt.ToUniversalTime())
                };

                foreach (var attribute in variant.Attributes)
                {
                    variantDto.VariantAttributes.Add(new VariantAttributeDto
                    {
                        ProductVariantAttributeId = attribute.ProductVariantAttributeId,
                        ProductVariantId = attribute.ProductVariantId,
                        Name = attribute.Name,
                        Value = attribute.Value
                    });
                }

                dto.Variants.Add(variantDto);
            }

            return dto;
        }
        public static byte[] ToBytes(decimal value)
        {
            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream);
            writer.Write(value);
            return memoryStream.ToArray();
        }

        // Dịch ngược từ Byte[] về Decimal
        public static decimal ToDecimal(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return 0;
            if (bytes.Length < 16) throw new ArgumentException("Byte array is too small for a decimal.");
            using var memoryStream = new MemoryStream(bytes);
            using var reader = new BinaryReader(memoryStream);
            return reader.ReadDecimal();
        }
    }
}
