using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using ECommerceAPI.Models;

namespace ECommerceAPI.Services;

public class JwtService(IConfiguration config)
{
    private readonly IConfiguration _config = config;

    public string GenerateToken(User user, List<string> roles)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["JWT_KEY"])
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("name", user.Name)
        };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var token = new JwtSecurityToken(
            issuer: _config["JWT_ISSUER"],
            audience: _config["JWT_AUDIENCE"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["JWT_EXPIRES"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}