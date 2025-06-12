using BikeShop.Models.Enums;

namespace BikeShop.Models;

public class OrderFilterDto
{
    public OrderStatus? Status { get; set; }

    public string? CustomerId { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}