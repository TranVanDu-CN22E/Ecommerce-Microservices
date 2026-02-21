using CatalogService.Domain.Aggregates.CategoryAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Infrastructure.Persistence.Configurations
{
    public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    v => v.Value,
                    v => CategoryId.Create(v)
                )
                .HasColumnName("CategoryId")
                .IsRequired();
            builder.Property(x => x.Name)
                .HasConversion(
                    v => v.Value,
                    v => CategoryName.Create(v)
                )
                .HasMaxLength(255)
                .HasColumnName("Name")
                .IsRequired();
            builder.Property(x => x.Slug)
                .HasConversion(
                    v => v.Value,
                    v => CategorySlug.Create(v)
                )
                .HasMaxLength(255)
                .HasColumnName("Slug")
                .IsRequired();
            builder.Property(x => x.ParentId)
                .HasConversion(
                    v => v.HasValue ? v.Value.Value : (Guid?)null,
                    v => v.HasValue ? CategoryId.Create(v.Value) : (CategoryId?)null
                )
                .HasColumnName("ParentId");
            builder.Property(x => x.Description)
                .HasMaxLength(4000)
                .HasColumnName("Description");
            builder.Property(x => x.IsActive)
                .HasColumnName("IsActive")
                .IsRequired();
            builder.Property(x => x.DisplayOrder)
                .HasColumnName("DisplayOrder")
                .IsRequired();
            builder.Property(x => x.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();
            builder.Property(x => x.UpdatedAt)
                .HasColumnName("UpdatedAt");
             builder.HasIndex(x => x.Slug)
                .IsUnique()
                .HasDatabaseName("IX_Category_Slug");
        }
    }
}
