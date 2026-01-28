using IdentityService.Domain.Aggregates.ProvinceAggregate;
using IdentityService.Domain.Aggregates.RefreshTokenAggregate;
using IdentityService.Domain.Aggregates.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(
                    id => id.Value,
                    value => RefreshTokenId.Create(value)
                )
                .ValueGeneratedNever();

            builder.Property(x => x.UserId)
                   .HasConversion(
                       v => v.Value,
                       v => UserId.Create(v))
                   .HasColumnName("UserId")
                   .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.Token)
                .HasColumnName("Token")
                .HasMaxLength(500)
                .IsRequired();
            builder.Property(x => x.ExpiresAt)
                .HasColumnName("ExpiresAt")
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(x => x.IsRevoked)
                .HasColumnName("IsRevoked");
            builder.Property(x => x.RevokedAt)
                .HasColumnName("RevokeAt")
                .HasMaxLength(100);
            builder.HasIndex(x => new { x.UserId, x.ExpiresAt}).HasDatabaseName("IX_RefreshToken_UserId"); ;
        }
    }
}
