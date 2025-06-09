namespace BikeShop.Models;

public class ProductDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string ImageUrl { get; set; } = null!; // <-- do wyświetlania obrazka

    public string Description { get; set; } = null!;
}