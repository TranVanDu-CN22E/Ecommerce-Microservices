using CatalogService.Domain.Aggregates.ProductAggregate;

namespace CatalogService.Domain.Interfaces
{
    public interface IVariantAttributeRepository
    {
        Task AddVariantAttributeAsync(List<VariantAttribute> variantAttributes, CancellationToken ct);
    }
}
