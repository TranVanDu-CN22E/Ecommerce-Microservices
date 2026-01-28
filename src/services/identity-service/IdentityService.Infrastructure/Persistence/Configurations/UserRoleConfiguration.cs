using IdentityService.Domain.Aggregates.RoleAggregate;
using IdentityService.Domain.Aggregates.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Infrastructure.Persistence.Configurations
{
    public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");

            // Primary Key
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            // UserId (Value Object)
            builder.Property(x => x.UserId)
                .HasConversion(
                    id => id.Value,
                    value => UserId.Create(value)
                )
                .IsRequired();

            // RoleId (Value Object)
            builder.Property(x => x.RoleId)
                .HasConversion(
                    id => id.Value,
                    value => RoleId.Create(value)
                )
                .IsRequired();

            // Relationship: User 1 - * UserRoles
            builder.HasOne(x => x.User)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Role 1 - * UserRoles
            builder.HasOne(x => x.Role)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional: unique constraint để tránh trùng role
            builder.HasIndex(x => new { x.UserId, x.RoleId })
                .IsUnique()
                .HasDatabaseName("IX_UserRoles_UserId_RoleId");
        }
    }
}
