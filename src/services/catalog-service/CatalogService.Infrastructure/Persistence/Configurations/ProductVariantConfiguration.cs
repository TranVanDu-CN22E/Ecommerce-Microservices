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
            builder.Property(x => x.ProductId)
                .HasConversion(
                    v => v.Value,
                    v => ProductId.Create(v)
                )
                .HasColumnName("ProductId")
                .IsRequired();
            builder.HasOne<Product>()
                .WithMany(p => p.Variants)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.SetNull);
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

            builder.Property(v => v.ImageUrl)
                .HasColumnName("image_url")
                .HasMaxLength(500);

            builder.Property(v => v.IsActive)
                .HasColumnName("is_active")
                .IsRequired();
            builder.Property(x => x.StockQuantity)
                .HasColumnName("StockQuantity")
                .IsRequired();
            builder.Property(x => x.SoldQuantity)
                .HasColumnName("SoldQuantity")
                .IsRequired();
            builder.Property(x => x.ReservedQuantity)
                .HasColumnName("ReservedQuantity")
                .IsRequired();

            builder.Property(v => v.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Ignore(v => v.IsOnSale);

            builder.Navigation(p => p.Price).IsRequired();
        }
    }
}
