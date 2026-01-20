using IdentityService.Application.Abstractions.Services;
using IdentityService.Domain.Interfaces;
using IdentityService.Infrastructure.Persistence;
using IdentityService.Infrastructure.Persistence.Repositories;
using IdentityService.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using System;

namespace IdentityService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<IdentityDbContext>(o => o.UseNpgsql("Host=localhost;Port=5435;Database=ev_identity;Username=ev_identity;Password=ev_identity_pw"));
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IProvinceRepository, ProvinceRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserAddressRepository, UserAddressRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
            services.AddScoped<IJwtTokenService, JwtTokenGenerator>();
            return services;
        }
    }
}
