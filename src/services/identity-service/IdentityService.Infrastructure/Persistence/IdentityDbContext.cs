using IdentityService.Domain.Aggregates.PermissionAggregate;
using IdentityService.Domain.Aggregates.ProvinceAggregate;
using IdentityService.Domain.Aggregates.RefreshTokenAggregate;
using IdentityService.Domain.Aggregates.RoleAggregate;
using IdentityService.Domain.Aggregates.UserAggregate;
using IdentityService.Infrastructure.Inbox;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence
{
    public class IdentityDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<UserAddress> UserAddresses => Set<UserAddress>();
        public DbSet<Province> Provinces => Set<Province>();

        public DbSet<InboxMessage> InboxMessages => Set<InboxMessage>();
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
