using IdentityService.Domain.Aggregates.ProvinceAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace IdentityService.Infrastructure.Persistence.Configurations
{
    public class ProvinceConfiguration : IEntityTypeConfiguration<Province>
    {
        public void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.ToTable("Provinces");
            builder.HasKey(p => p.Id);
            builder.Property(x => x.Id)
                .HasConversion(
                    id => id.Value,
                    value => ProvinceId.Create(value)
                )
                .ValueGeneratedNever();

            builder.Property(p => p.ProvinceName)
                .HasMaxLength(100)
                .HasColumnName("ProvinceName")
                .IsRequired();
            // Load JSON file
            var path = Path.Combine(AppContext.BaseDirectory, "Data", "provinces.json");
            Console.WriteLine(path);
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var provinces = JsonSerializer.Deserialize<List<ProvinceSeedModel>>(json, options);

                var seedData = provinces!.Select(p =>
                    new { Id = ProvinceId.Create(p.id), ProvinceName = p.name }
                );

                builder.HasData(seedData);
            }
            else
            {
                Console.WriteLine("provinces.json not found: " + path);
            }
        }
    }
    public record ProvinceSeedModel
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
