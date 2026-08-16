using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Aggregates.ShippingRuleAggregate;

namespace OrderService.Infratructure.Persistence.Configurations
{
    public class ShippingRuleConfiguration : IEntityTypeConfiguration<ShippingRule>
    {
        public void Configure(EntityTypeBuilder<ShippingRule> builder)
        {
            builder.ToTable("ShippingRule");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever().HasColumnName("ShippingRuleId").IsRequired();
            builder.Property(x => x.Province).HasColumnName("Province").IsRequired();
            builder.OwnsOne(x => x.BaseFee, money =>
            {
                money.Property(m => m.Amount).HasColumnName("BaseFeeAmount").IsRequired();
                money.Property(m => m.Currency).HasColumnName("BaseFeeCurrency").IsRequired();
            });
            builder.OwnsOne(x => x.FreeThreshold, money =>
            {
                money.Property(m => m.Amount).HasColumnName("FreeThresholdAmount").IsRequired();
                money.Property(m => m.Currency).HasColumnName("FreeThresholdCurrency").IsRequired();
            });
            builder.Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
            builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").IsRequired();
        }
    }
}