using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Data;
using ECommerceAPI.Models;

namespace ECommerceAPI.Services;

public class ProductService(ECommerceDbContext context)
{
    private readonly ECommerceDbContext _dbContext = context;

    public async Task<List<Product>> GetAll() =>
        await _dbContext.Products.AsNoTracking()
            .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
            .ToListAsync();

    public async Task<Product?> GetById(int id) =>
        await _dbContext.Products.AsNoTracking()
            .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
            .FirstOrDefaultAsync(p => p.ProductId == id);

    public async Task<Product> Create(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<bool> Delete(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null) return false;

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
