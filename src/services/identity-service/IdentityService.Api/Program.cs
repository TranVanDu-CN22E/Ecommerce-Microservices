using IdentityService.Application;
using IdentityService.Infrastructure;
using IdentityService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "IdentityService_";
});
builder.WebHost.ConfigureKestrel(options =>
{
    // Port 8080: REST + Swagger
    options.ListenAnyIP(8080, o => o.Protocols = HttpProtocols.Http1);

    // Port 8081: gRPC (HTTP/2 required)
    options.ListenAnyIP(8081, o => o.Protocols = HttpProtocols.Http2);
});
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<Program>>();
    try
    {
        var configuration = ConfigurationOptions.Parse(
            builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379",
            true
        );

        // Quan trọng: Cho phép retry thay vì abort
        configuration.AbortOnConnectFail = false;
        configuration.ConnectTimeout = 5000;
        configuration.ConnectRetry = 3;

        var multiplexer = ConnectionMultiplexer.Connect(configuration);
        logger.LogInformation("Redis connected successfully");
        return multiplexer;
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Redis connection failed. Application will run without cache.");
        // Return a dummy multiplexer
        return ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false");
    }
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer("Bearer", opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    })
    .AddJwtBearer("SystemBearer", opt =>
    {
        // Token của service nội bộ
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSystem:Issuer"],
            ValidAudience = builder.Configuration["JwtSystem:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSystem:SigningKey"]!))
        };
    });


var app = builder.Build();
app.ApplyMigrations();
app.Use(async (context, next) =>
{
    if (context.User.Identity?.IsAuthenticated == true)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var roles = string.Join(",",
            context.User.FindAll(ClaimTypes.Role).Select(c => c.Value));

        // Thêm header cho downstream services
        context.Request.Headers["X-User-Id"] = userId;
        context.Request.Headers["X-User-Roles"] = roles;
    }

    await next();
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
