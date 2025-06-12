using System.ComponentModel.DataAnnotations;

namespace BikeShop.Models;

public class OrderProductDto
{
    [Required]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
    
    public decimal Price { get; set; }
}