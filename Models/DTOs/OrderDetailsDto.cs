namespace BikeShop.Models;

public class OrderDetailsDto
{
    public int Id { get; set; }
    public string CustomerId { get; set; }
    
    public List<OrderProductDetailsDto> Products { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }
}