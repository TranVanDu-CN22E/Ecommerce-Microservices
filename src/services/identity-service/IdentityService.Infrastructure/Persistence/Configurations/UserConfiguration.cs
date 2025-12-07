using IdentityService.Domain.Aggregates.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.OwnsOne(x => x.Email, e =>
            {
                e.Property(p => p.Value)
                    .HasColumnName("Email")
                    .HasMaxLength(255)
                    .IsRequired();
            });
            builder.HasIndex(x => x.Email.Value)
                .IsUnique()
                .HasDatabaseName("IX_User_Email");

            builder.OwnsOne(x => x.Phone, e =>
            {
                e.Property(p => p.Value)
                    .HasColumnName("Phone")
                    .HasMaxLength(20)
                    .IsRequired();
            });
            builder.HasIndex(x => x.Phone.Value)
                .IsUnique()
                .HasDatabaseName("IX_User_Phone");

            builder.OwnsOne(x => x.UserName, e =>
            {
                e.Property(p => p.Value)
                    .HasColumnName("UserName")
                    .HasMaxLength(100)
                    .IsRequired();
            });

            builder.OwnsOne(x => x.PasswordHash, e =>
            {
                e.Property(p => p.Value)
                    .HasColumnName("PasswordHash")
                    .HasMaxLength(500)
                    .IsRequired();
            });
            builder.OwnsOne(x => x.RoleId, e =>
            {
                e.Property(p => p.Value)
                .HasColumnName("RoleId")
                .IsRequired();
            });
            builder.HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.IsLocked).IsRequired();
            builder.Property(x => x.FailedLoginAttempts).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.LockedUntil);

            builder.Navigation(x => x.Email).AutoInclude();
            builder.Navigation(x => x.Phone).AutoInclude();
            builder.Navigation(x => x.UserName).AutoInclude();
            builder.Navigation(x => x.PasswordHash).AutoInclude();
            builder.Navigation(x => x.Role).AutoInclude();

        }
    }
}
