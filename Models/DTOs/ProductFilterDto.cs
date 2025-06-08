using System.ComponentModel.DataAnnotations;

namespace BikeShop.Models;

public class ProductFilterDto
{
    public List<string>? Categories { get; set; }
    
    [Range(0, double.MaxValue, ErrorMessage = "Minimalna cena musi być nieujemna.")]
    public decimal? MinPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Maksymalna cena musi być nieujemna.")]
    public decimal? MaxPrice { get; set; }
}