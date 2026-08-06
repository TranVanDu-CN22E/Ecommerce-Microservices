using CatalogService.Domain.Aggregates.ProductAggregate;
using CatalogService.Domain.Interfaces;
using CatalogService.Infrastructure.Persistence.Configurations;

namespace CatalogService.Infrastructure.Persistence.Repositories
{
    public class VariantAttributeRepository : IVariantAttributeRepository
    {
        private readonly CatalogDbContext _context;
        public VariantAttributeRepository(CatalogDbContext context)
        {
            _context = context;
        }
        public async Task AddVariantAttributeAsync(List<VariantAttribute> variantAttributes, CancellationToken ct)
        {
            foreach (var variantAttribute in variantAttributes)
            {
                await _context.VariantAttributes.AddAsync(variantAttribute, ct);
            }
        }
    }
}
