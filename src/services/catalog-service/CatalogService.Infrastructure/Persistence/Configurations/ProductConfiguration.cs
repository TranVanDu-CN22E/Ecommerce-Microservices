using CatalogService.Domain.Aggregates.CategoryAggregate;
using CatalogService.Domain.Aggregates.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Infrastructure.Persistence.Configurations
{
    public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    v => v.Value,
                    v => ProductId.Create(v)
                )
                .HasColumnName("ProductId")
                .IsRequired();

            builder.Property(x => x.ProductName)
                .HasConversion(
                    v => v.Value,
                    v => ProductName.Create(v)
                )
                .HasMaxLength(255)
                .HasColumnName("ProductName")
                .IsRequired();

            builder.Property(x => x.ProductSlug)
                .HasConversion(
                    v => v.Value,
                    v => ProductSlug.Create(v)
                )
                .HasColumnName("Price")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(4000)
                .HasColumnName("Description")
                .IsRequired();

            builder.Property(x => x.CategoryId)
                .HasConversion(
                    v => v.Value,
                    v => CategoryId.Create(v)
                )
                .HasColumnName("CategoryId")
                .IsRequired();

            builder.Property(x => x.ThumbnailUrl)
                .HasMaxLength(500)
                .HasColumnName("ThumbnailUrl")
                .IsRequired();
             builder.Property(x => x.ImageUrls)
                .HasColumnName("ImageUrls")
                .HasColumnType("text[]")
                .IsRequired();
             builder.Property(x => x.IsPublished)
                .HasColumnName("IsPublished")
                .IsRequired();
             builder.Property(x => x.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();
             builder.Property(x => x.UpdatedAt)
                .HasColumnName("UpdatedAt")
                .IsRequired(false);
             builder.Property(x => x.PublishedAt)
                .HasColumnName("PublishedAt")
                .IsRequired(false);
        }
    }
}
