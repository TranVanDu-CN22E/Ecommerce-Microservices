using CatalogService.Domain.Aggregates.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Infrastructure.Persistence.Configurations
{
    public class VariantAttributeConfiguration : IEntityTypeConfiguration<VariantAttribute>
    {
        public void Configure(EntityTypeBuilder<VariantAttribute> builder)
        {
            builder.ToTable("VariantAttributes");
            builder.HasKey(x => x.ProductVariantAttributeId);
            builder.Property(x => x.ProductVariantAttributeId)
                .ValueGeneratedNever()
                .HasColumnName("ProductVariantAttributeId")
                .IsRequired();
            //Foreign key to ProductVariant
            builder.Property(x => x.ProductVariantId)
                .HasColumnName("ProductVariantId")
                .IsRequired();
            builder.HasOne<ProductVariant>() // VariantAttribute có 1 ProductVariant
               .WithMany(x => x.Attributes) // Nếu ProductVariant có danh sách VariantAttributes thì điền x => x.VariantAttributes vào đây
               .HasForeignKey(x => x.ProductVariantId) // Xác định trường làm khóa ngoại
               .OnDelete(DeleteBehavior.SetNull);
            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .HasColumnName("Name")
                .IsRequired();
            builder.Property(x => x.Value)
                .HasMaxLength(100)
                .HasColumnName("Value")
                .IsRequired();
        }
    }
}
