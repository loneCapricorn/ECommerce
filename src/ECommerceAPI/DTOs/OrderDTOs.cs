namespace ECommerceAPI.DTOs;

public class OrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class CreateOrderDto
{
    public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
}
