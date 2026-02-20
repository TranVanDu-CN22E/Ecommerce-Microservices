using CatalogService.Domain.Common;

namespace CatalogService.Domain.Aggregates.CategoryAggregate
{
    public sealed class Category : AggregateRoot<CategoryId>
    {
        public CategoryName Name { get; private set; }
        public CategorySlug Slug { get; private set; } // Unique slug for URL generation eg:website.com/ao-thun-nam
        public CategoryId? ParentId { get; private set; }
        public string? Description { get; private set; }
        public bool IsActive { get; private set; }
        public int DisplayOrder { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        private Category() { }
        private Category(CategoryId id, CategoryName name, CategorySlug slug, CategoryId? parentId, string? description, int displayOrder)
        {
            Id = id;
            Name = name;
            Slug = slug;
            ParentId = parentId;
            Description = description;
            IsActive = true;
            DisplayOrder = displayOrder;
            CreatedAt = DateTime.UtcNow;
        }
        public static Category Create(CategoryName name, CategorySlug slug, CategoryId? parentId, string? description, int displayOrder)
        {
            return new Category(
                CategoryId.New(),
                name,
                slug,
                parentId,
                description,
                displayOrder
                );
        }
        public void Update(CategoryName? name, CategorySlug? slug, CategoryId? parentId, string? description, int? displayOrder)
        {
            if (name is not null) Name = name.Value;
            if (slug is not null) Slug = slug.Value;
            if (parentId is not null) ParentId = parentId;
            if (description is not null) Description = description;
            if (displayOrder.HasValue) DisplayOrder = displayOrder.Value;
            UpdatedAt = DateTime.UtcNow;
        }
        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;


    }
}
