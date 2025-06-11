using BikeShop.Data;
using BikeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeShop.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext context;
    private readonly ProductMapper mapper;

    public ProductService(AppDbContext context, ProductMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await this.context.Products.ToListAsync();
        return products.Select(p => this.mapper.Map(p));
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await this.context.Products.FindAsync(id);
        if (product == null)
        {
            return null;
        }

        return this.mapper.Map(product);
    }

    public async Task<ProductDto> CreateAsync(ProductCreateUpdateDto dto)
    {
        var product = this.mapper.Map(dto);
        this.context.Products.Add(product);
        await this.context.SaveChangesAsync();
        return this.mapper.Map(product);
    }

    public async Task<bool> UpdateAsync(int id, ProductCreateUpdateDto dto)
    {
        var existingProduct = await this.context.Products.FindAsync(id);
        if (existingProduct == null)
        {
            return false;
        }

        this.mapper.UpdateProductFromDto(dto, existingProduct);
        await this.context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await this.context.Products.FindAsync(id);
        if (product == null)
        {
            return false;
        }

        this.context.Products.Remove(product);
        await this.context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ProductDto>> GetFilteredAsync(ProductFilterDto filter)
    {
        var query = this.context.Products.AsQueryable();

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
        return products.Select(p => this.mapper.Map(p));
    }
    public IQueryable<ProductDto> GetAllQueryable()
    {
        // Tu mapowanie na DTO można zrobić na poziomie Linq (projekcja)
        return context.Products
            .OrderBy(p => p.Name)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                // inne właściwości, jeśli są
            });
    }
    
    public IQueryable<ProductDto> GetFilteredQueryable(ProductFilterDto filter)
    {
        var query = context.Products.AsQueryable();

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

        return query.OrderBy(p => p.Name)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl
            });
    }

}
