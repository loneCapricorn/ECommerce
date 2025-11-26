using ECommerceAPI.Data;
using ECommerceAPI.Models;

namespace ECommerceAPI.Helpers;

public static class DatabaseSeeder
{
    public static void SeedRoles(ECommerceDbContext dbContext)
    {
        if (!dbContext.Roles.Any())
        {
            dbContext.Roles.AddRange(
                new Role { Name = "Admin" },
                new Role { Name = "User" }
            );

            dbContext.SaveChanges();
        }
    }
}

