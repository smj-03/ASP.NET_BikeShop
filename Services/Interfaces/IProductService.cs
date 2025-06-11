using BikeShop.Models;

namespace BikeShop.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();

    Task<ProductDto?> GetByIdAsync(int id);

    Task<ProductDto> CreateAsync(ProductCreateUpdateDto dto);

    Task<bool> UpdateAsync(int id, ProductCreateUpdateDto dto);

    Task<bool> DeleteAsync(int id);

    Task<IEnumerable<ProductDto>> GetFilteredAsync(ProductFilterDto filter);
    IQueryable<ProductDto> GetAllQueryable();
    IQueryable<ProductDto> GetFilteredQueryable(ProductFilterDto filter);
}