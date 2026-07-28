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
    
        public async Task<Product?> GetProductByIdAsync(ProductId id, CancellationToken ct)
        {
            return await _context.Products.FindAsync(id, ct);
        }
        public async Task<Product?> GetProductBySlugAsync(ProductSlug productSlug, CancellationToken ct)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.ProductSlug == productSlug, ct);
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
    
        public async Task DeleteProductAsync(ProductId id, CancellationToken ct)
        {
            var product = await GetProductByIdAsync(id, ct);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
        }

        public async Task<string?> GetThumbnailUrlAsync(ProductId id, CancellationToken ct)
        {
            var product = await GetProductByIdAsync(id, ct);
            if (product != null)
            {
                return product.ThumbnailUrl;
            }
            return null;
        }

        public async Task<List<ProductVariant>> GetVariantsAsync(ProductId productId, CancellationToken ct)
        {
            var product = await GetProductByIdAsync(productId, ct);
            if (product != null && product.Variants != null && product.Variants.Count > 0)
            {
                return product.Variants;
            }
            return new List<ProductVariant>();
        }
    }
}
