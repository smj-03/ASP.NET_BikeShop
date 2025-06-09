using BikeShop.Models;
using BikeShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeShop.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Employee")]
public class ProductsController : ControllerBase
{
    private readonly IProductService productService;

    public ProductsController(IProductService productService)
    {
        this.productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await this.productService.GetAllAsync();
        return this.Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await this.productService.GetByIdAsync(id);
        if (product == null)
        {
            return this.NotFound();
        }

        return this.Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Employee")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] ProductCreateUpdateDto dto)
    {
        var imageFile = dto.Image;

        // Zapisz plik do wwwroot/images
        var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
        var filePath = Path.Combine("wwwroot", "images", fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        var imageUrl = $"/images/{fileName}";

        var productDto = new ProductCreateUpdateDto
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            Category = dto.Category,
            Manufacturer = dto.Manufacturer,
            ImageUrl = imageUrl,
        };

        var created = await this.productService.CreateAsync(productDto);
        return this.CreatedAtAction(nameof(this.GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Employee")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductCreateUpdateDto dto)
    {
        var updated = await this.productService.UpdateAsync(id, dto);
        if (!updated)
        {
            return this.NotFound();
        }

        return this.NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await this.productService.DeleteAsync(id);
        if (!deleted)
        {
            return this.NotFound();
        }

        return this.NoContent();
    }

    [HttpGet("filter")]
    public async Task<IActionResult> Filter([FromQuery] ProductFilterDto filterDto)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var products = await this.productService.GetFilteredAsync(filterDto);
        return this.Ok(products);
    }
}