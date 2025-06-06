namespace BikeShop.Models;

public class ProductCreateUpdateDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string Category { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string Manufacturer { get; set; } = null!;
    public int StockQuantity { get; set; }
}