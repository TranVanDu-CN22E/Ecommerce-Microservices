using CatalogService.Domain.Aggregates.CategoryAggregate;
using CatalogService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : ICatalogRepository
    {
        private readonly CatalogDbContext _context;
        public CategoryRepository(CatalogDbContext context)
        {
            _context = context;
        }
        public async Task AddCategoryAsync(Category category, CancellationToken ct)
        {
            await _context.Categories.AddAsync(category, ct);
        }
        public async Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.Categories.FindAsync(CategoryId.Create(id), ct);
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken ct)
        {
            return await _context.Categories.ToListAsync(ct);
        }
        public async Task UpdateCategoryAsync(Category category, CancellationToken ct)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync(ct);
        }
        public async Task DeleteCategoryAsync(Guid id, CancellationToken ct)
        {
            var category = await GetCategoryByIdAsync(id, ct);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
        }
    }
}
