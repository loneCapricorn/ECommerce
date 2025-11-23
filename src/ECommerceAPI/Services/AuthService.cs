using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Helpers;

namespace ECommerceAPI.Services;

public class AuthService(ECommerceDbContext context, JwtService jwt)
{
    private readonly ECommerceDbContext _dbContext = context;
    private readonly JwtService _jwtService = jwt;

    public async Task<string?> Login(LoginDto dto)
    {
        var user = await _dbContext.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null || !PasswordHasher.Verify(dto.Password, user.PasswordHash))
            return null;

        var roles = user.UserRoles.Select(r => r.Role.Name).ToList();
        return _jwtService.GenerateToken(user, roles);
    }

    public async Task<bool> Register(RegisterDto dto)
    {
        if (await _dbContext.Users.AnyAsync(u => u.Email == dto.Email))
            return false;

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = PasswordHasher.Hash(dto.Password)
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
