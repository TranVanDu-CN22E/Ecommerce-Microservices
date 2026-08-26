using CatalogService.Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Infrastructure.Persistence.Configurations
{
    public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("OutboxMessages");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Type)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(m => m.Content)
                .IsRequired();

            builder.Property(m => m.Error);

            // Index để tối ưu query polling của Background Worker
            builder.HasIndex(m => new { m.ProcessedOnUtc, m.OccurredOnUtc })
                .HasFilter("\"ProcessedOnUtc\" IS NULL") //  Đã sửa thành dấu ngoặc kép cho Postgres
                .HasDatabaseName("IX_OutboxMessages_Pending");

        }
    }
}
