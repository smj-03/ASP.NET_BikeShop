namespace BikeShop.Models;

using System.ComponentModel.DataAnnotations;

public class ProductCreateUpdateDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = null!;

    [Required]
    [Range(0.01, 10000.00)]
    public decimal Price { get; set; }

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Manufacturer { get; set; } = null!;

    [Required]
    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    [Required]
    public IFormFile Image { get; set; } = null!;

    public string? ImageUrl;
}