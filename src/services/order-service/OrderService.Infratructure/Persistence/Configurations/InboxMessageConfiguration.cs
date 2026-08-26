using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Infratructure.Persistence.Inbox;

namespace OrderService.Infratructure.Persistence.Configurations
{
    public class InboxMessageConfiguration : IEntityTypeConfiguration<InboxMessage>
    {
        public void Configure(EntityTypeBuilder<InboxMessage> builder)
        {
            builder.ToTable("InboxMessages");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.ReceivedOnUtc).IsRequired();
            builder.Property(x => x.ProcessedOnUtc).IsRequired(false);
            builder.Property(x => x.Error).IsRequired(false);
        }
    }
}
/*public Guid Id { get; set; }
public string Type { get; set; } = string.Empty;
public string Content { get; set; } = string.Empty;
public DateTime ReceivedOnUtc { get; set; }
public DateTime? ProcessedOnUtc { get; set; }
public string? Error { get; set; }*/
