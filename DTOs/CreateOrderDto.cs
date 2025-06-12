using System.ComponentModel.DataAnnotations;

namespace BikeShop.Models;

public class CreateOrderDto
{
    public string? CustomerId { get; set; }

    [Required]
    [MinLength(1)]
    public List<OrderProductDto> Products { get; set; } = new ();
}