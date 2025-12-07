using IdentityService.Domain.Aggregates.ProvinceAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Infrastructure.Persistence.Configurations
{
    public class ProvinceConfiguration : IEntityTypeConfiguration<Province>
    {
        public void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.ToTable("Provinces");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedNever();

            builder.Property(p => p.ProvinceName)
                .HasMaxLength(100)
                .HasColumnName("ProvinceName")
                .IsRequired();
            // Load JSON file
            var path = Path.Combine(AppContext.BaseDirectory, "Data", "provinces.json");
            Console.WriteLine(path);

            var json = File.ReadAllText(path);
            var provinces = System.Text.Json.JsonSerializer.Deserialize<List<ProvinceSeedModel>>(json);

            // Convert to EF data format
            var seedData = provinces!.Select(p =>
                new { Id = p.Id, Name = p.Name }
            );

            builder.HasData(seedData);
        }
    }
    public record ProvinceSeedModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
