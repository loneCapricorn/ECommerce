using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Data;
using ECommerceAPI.Models;
using ECommerceAPI.DTOs;

namespace ECommerceAPI.Services;

public class OrderService(ECommerceDbContext context)
{
    private readonly ECommerceDbContext _dbContext = context;

    public async Task<Order> CreateOrder(int userId, CreateOrderDto dto)
    {
        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            OrderItems = new List<OrderItem>()
        };

        foreach (var item in dto.Items)
        {
            var product = await _dbContext.Products.FindAsync(item.ProductId);
            if (product == null) throw new Exception($"Product {item.ProductId} not found");

            if (product.Stock < item.Quantity)
                throw new Exception($"Not enough stock for product {product.Name}");

            product.Stock -= item.Quantity;

            order.OrderItems.Add(new OrderItem
            {
                ProductId = product.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
        }

        order.TotalAmount = order.OrderItems.Sum(i => i.Quantity * i.UnitPrice);

        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();

        return order;
    }

    public async Task<List<Order>> GetOrdersByUser(int userId) =>
        await _dbContext.Orders.AsNoTracking()
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .ToListAsync();
}
