namespace BikeShop.Models;

public class IndexViewModel
{
    public ProductFilterDto Filter { get; set; } = new ();

    public IEnumerable<ProductDto> Products { get; set; } = new List<ProductDto>();

    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
}

