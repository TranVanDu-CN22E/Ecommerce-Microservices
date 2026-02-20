using Medo;
namespace CatalogService.Domain.Aggregates.CategoryAggregate
{
    public readonly record struct CategoryId (Guid Value)
    {
        public static CategoryId New() => new(Uuid7.NewUuid7());
        public static CategoryId Create(Guid value) => value == Guid.Empty ? throw new ArgumentNullException("Category id cannot be empty") : new(value);
    }
}
