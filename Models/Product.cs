using System.ComponentModel.DataAnnotations;

namespace BikeShop.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    
    [Required]
    public decimal Price { get; set; }
    
    [Required]
    [StringLength(500)]
    public string Description { get; set; }
    
    [Required]
    public string Category { get; set; }
    
    [Required]
    public string ImageUrl { get; set; }
    
    [Required]
    public string Manufacturer { get; set; }
    
    [Required]
    public int StockQuantity { get; set; }
}