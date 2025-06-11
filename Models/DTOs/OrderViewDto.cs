using BikeShop.Models.Enums;

namespace BikeShop.Models;

public class OrderViewDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public OrderStatus Status { get; set; }
    public string CustomerEmail { get; set; }
    public List<OrderProductDto> Items { get; set; }
}