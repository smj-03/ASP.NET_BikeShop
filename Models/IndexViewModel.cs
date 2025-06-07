
namespace BikeShop.Models;

public class IndexViewModel
{
    public ProductFilterDto Filter { get; set; } = new();
    public IEnumerable<ProductDto> Products { get; set; } = new List<ProductDto>();
}