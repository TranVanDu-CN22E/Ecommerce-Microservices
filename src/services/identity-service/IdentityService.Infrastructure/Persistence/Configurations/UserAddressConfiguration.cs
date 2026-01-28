using IdentityService.Domain.Aggregates.ProvinceAggregate;
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
            builder.Property(x => x.Id)
                .HasConversion(
                    v => v.Value,
                    v => UserAddressId.Create(v))
                .ValueGeneratedNever();

            builder.Property(x => x.UserId)
                .HasConversion(
                    v => v.Value,
                    v => UserId.Create(v))
                .HasColumnName("UserId")
                .IsRequired();
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
            builder.Property(x => x.ProvinceId)
                   .HasConversion(
                       v => v.Value,
                       v => ProvinceId.Create(v))
                   .HasColumnName("ProvinceId")
                   .IsRequired();
            builder.HasOne(x => x.Province)
                .WithMany(x => x.UserAddresses)
                .HasForeignKey(x => x.ProvinceId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(x => x.Address).HasColumnName("Address").HasMaxLength(200).IsRequired();
            builder.Property(x => x.Note).HasColumnName("Note").HasMaxLength(200);

            builder.Navigation(x => x.User).AutoInclude();
            builder.Navigation(x => x.Province).AutoInclude();


            builder.HasIndex(x => x.UserId).HasDatabaseName("IX_UserAddress_UserId");
        }
    }
}
