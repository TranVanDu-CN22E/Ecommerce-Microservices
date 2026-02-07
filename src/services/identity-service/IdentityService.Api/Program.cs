using IdentityService.Application;
using IdentityService.Application.Abstractions.Services;
using IdentityService.Infrastructure;
using IdentityService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
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
