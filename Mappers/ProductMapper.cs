using BikeShop.Models;
using Riok.Mapperly.Abstractions;

[Mapper]
public partial class ProductMapper
{
    public partial ProductDto Map(Product product);
    public partial Product Map(ProductCreateUpdateDto dto);
    
    public partial void UpdateProductFromDto(ProductCreateUpdateDto dto, Product product);
} 