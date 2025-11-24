using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Data;
using ECommerceAPI.Models;

namespace ECommerceAPI.Services;

public class UserService(ECommerceDbContext context)
{
    private readonly ECommerceDbContext _dbContext = context;

    public async Task<List<User>> GetAll() =>
        await _dbContext.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .ToListAsync();

    public async Task<User?> GetById(int id) =>
        await _dbContext.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.UserId == id);
}
