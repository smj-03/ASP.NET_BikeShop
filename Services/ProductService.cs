using BikeShop.Data;
using BikeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeShop.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    private readonly ProductMapper _mapper;

    public ProductService(AppDbContext context, ProductMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _context.Products.ToListAsync();
        return products.Select(p => _mapper.Map(p));
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return null;
        return _mapper.Map(product);
    }

    public async Task<ProductDto> CreateAsync(ProductCreateUpdateDto dto)
    {
        var product = _mapper.Map(dto);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return _mapper.Map(product);
    }

    public async Task<bool> UpdateAsync(int id, ProductCreateUpdateDto dto)
    {
        var existingProduct = await _context.Products.FindAsync(id);
        if (existingProduct == null) return false;

        _mapper.UpdateProductFromDto(dto, existingProduct);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<IEnumerable<ProductDto>> GetFilteredAsync(ProductFilterDto filter)
    {
        var query = _context.Products.AsQueryable();

        if (filter.Categories != null && filter.Categories.Any())
        {
            query = query.Where(p => filter.Categories.Contains(p.Category));
        }

        if (filter.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= filter.MaxPrice.Value);
        }

        var products = await query.ToListAsync();
        return products.Select(p => _mapper.Map(p));
    }
}
