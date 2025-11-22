using System;

namespace ECommerceAPI.Models;

public class User
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    public ICollection<Order> Orders { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
}
