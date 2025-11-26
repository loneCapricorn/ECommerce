using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Data;
using ECommerceAPI.Models;

namespace ECommerceAPI.Services;

public class CategoryService(ECommerceDbContext context)
{
    private readonly ECommerceDbContext _dbContext = context;

    public async Task<List<Category>> GetAll() =>
        await _dbContext.Categories.AsNoTracking()
            .Include(c => c.ProductCategories)
                .ThenInclude(pc => pc.Product)
            .ToListAsync();

    public async Task<Category?> GetById(int id) =>
        await _dbContext.Categories.AsNoTracking()
            .Include(c => c.ProductCategories)
                .ThenInclude(pc => pc.Product)
            .FirstOrDefaultAsync(c => c.CategoryId == id);

    public async Task<Category> Create(Category category)
    {
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();
        return category;
    }

    public async Task<bool> Delete(int id)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        if (category == null) return false;

        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
        return true;
    }

}
