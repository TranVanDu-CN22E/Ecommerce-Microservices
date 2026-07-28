using CatalogService.Domain.Aggregates.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Infrastructure.Persistence.Configurations
{
    public sealed class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.ToTable("ProductVariants");
            builder.HasKey(x => x.ProductVariantId);

            builder.Property(x => x.ProductVariantId)
                .ValueGeneratedNever()
                .HasConversion(
                    v => v.Value,
                    v => ProductVariantId.Create(v)
                )
                .HasColumnName("ProductVariantId")
                .IsRequired();

            builder.Property(x => x.ProductSku)
                .HasConversion(
                    v => v.Value,
                    v => ProductSku.Create(v)
                )
                .HasMaxLength(100)
                .HasColumnName("ProductSku")
                .IsRequired();

            builder.OwnsOne(p => p.Price, money =>
            {
                money.Property(m => m.Amount)
                     .HasColumnName("PriceAmount")
                     .HasPrecision(18, 2)
                     .IsRequired();

                money.Property(m => m.Currency)
                     .HasColumnName("PriceCurrency")
                     .HasMaxLength(3)
                     .IsRequired();
            });

            builder.OwnsOne(v => v.OriginalPrice, price =>
            {
                price.Property(m => m.Amount)
                    .HasColumnName("OriginalPriceAmount")
                    .HasColumnType("numeric(18,2)");

                price.Property(m => m.Currency)
                    .HasColumnName("OriginalPriceCurrency")
                    .HasMaxLength(3);
            });

            builder.OwnsMany(pv => pv.Attributes, attr =>
            {
                attr.WithOwner().HasForeignKey("ProductVariantId"); // FK về ProductVariant
                attr.Property(a => a.Name).HasMaxLength(100).IsRequired();
                attr.Property(a => a.Value).HasMaxLength(100).IsRequired();
                attr.Property(a => a.StockQuantity).IsRequired();
                attr.ToTable("ProductVariantAttributes"); // tên bảng riêng
            });

            builder.Property(v => v.ImageUrl)
                .HasColumnName("image_url")
                .HasMaxLength(500);

            builder.Property(v => v.IsActive)
                .HasColumnName("is_active")
                .IsRequired();

            builder.Property(v => v.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Ignore(v => v.IsOnSale);

            builder.Navigation(p => p.Price).IsRequired();
        }
    }
}
