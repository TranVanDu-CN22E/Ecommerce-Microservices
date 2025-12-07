using IdentityService.Domain.Aggregates.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Infrastructure.Persistence.Configurations
{
    public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.ToTable("UserAddresses");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.OwnsOne(x => x.UserId, e =>
                {
                    e.Property(p => p.Value)
                    .HasColumnName("UserId")
                    .IsRequired();
                });
            builder.HasOne(x => x.User)
                .WithMany(x => x.UserAddresses)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(x => x.Phone, e =>
            {
                e.Property(p => p.Value)
                .HasColumnName("Phone")
                .IsRequired();
            });

            builder.OwnsOne(x => x.ProvinceId, e =>
            {
                e.Property(p=>p.Value)
                .HasColumnName("ProvinceId")
                .IsRequired();
            });
            builder.HasOne(x => x.Province)
                .WithMany(x => x.UserAddresses)
                .HasForeignKey(x => x.ProvinceId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(x => x.Address).HasColumnName("Address").HasMaxLength(200).IsRequired();
            builder.Property(x => x.Note).HasColumnName("Note").HasMaxLength(200);

            builder.Navigation(x => x.UserId).AutoInclude();
            builder.Navigation(x => x.Phone).AutoInclude();
            builder.Navigation(x => x.ProvinceId).AutoInclude();

            builder.HasIndex(x => x.UserId).HasDatabaseName("IX_UserAddress_UserId");
        }
    }
}
