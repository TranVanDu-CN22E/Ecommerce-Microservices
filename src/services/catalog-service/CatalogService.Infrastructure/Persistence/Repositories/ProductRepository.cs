using CatalogService.Domain.Aggregates.ProductAggregate;
using CatalogService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogDbContext _context;
    
        public ProductRepository(CatalogDbContext context)
        {
            _context = context;
        }
    
        public async Task AddProductAsync(Product product, CancellationToken ct)
        {
            await _context.Products.AddAsync(product, ct);
        }
    
        public async Task<Product?> GetProductByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.Products.FindAsync(ProductId.Create(id), ct);
        }
    
        public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken ct)
        {
            return await _context.Products.ToListAsync(ct);
        }
    
        public async Task UpdateProductAsync(Product product, CancellationToken ct)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    
        public async Task DeleteProductAsync(Guid id, CancellationToken ct)
        {
            var product = await GetProductByIdAsync(id, ct);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
        }
    }
}
