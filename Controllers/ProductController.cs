using BikeShop.Models;
using BikeShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeShop.Controllers;

[Authorize(Roles = "Admin,Employee")]
public class ProductsController : Controller
{
    private readonly IProductService productService;

    public ProductsController(IProductService productService)
    {
        this.productService = productService;
    }

    // GET: /Products
    public async Task<IActionResult> Index()
    {
        var products = await this.productService.GetAllAsync();
        return this.View(products);
    }

    // GET: /Products/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var product = await this.productService.GetByIdAsync(id);
        if (product == null)
        {
            return this.NotFound();
        }

        return this.View(product);
    }

    // GET: /Products/Create
    public IActionResult Create()
    {
        return this.View();
    }

    // POST: /Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create(ProductCreateUpdateDto dto)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(dto);
        }

        if (dto.Image == null || dto.Image.Length == 0)
        {
            this.ModelState.AddModelError("Image", "Image file is required.");
            return this.View(dto);
        }

        // Zapis pliku
        var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await dto.Image.CopyToAsync(stream);
        }

        dto.ImageUrl = $"/images/{fileName}";

        var created = await this.productService.CreateAsync(dto);
        return this.RedirectToAction(nameof(this.Details), new { id = created.Id });
    }

    // GET: /Products/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var product = await this.productService.GetByIdAsync(id);
        if (product == null)
        {
            return this.NotFound();
        }

        // Mapujemy do DTO do edycji
        var dto = new ProductCreateUpdateDto
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            Category = product.Category,
            Manufacturer = product.Manufacturer,
            ImageUrl = product.ImageUrl,
        };

        return this.View(dto);
    }

    // POST: /Products/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Edit(int id, ProductCreateUpdateDto dto)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(dto);
        }

        if (dto.Image != null && dto.Image.Length > 0)
        {
            // Nowy plik obrazu -> zapis
            var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            dto.ImageUrl = $"/images/{fileName}";
        }

        // Jeżeli obrazek nie został zmieniony, zostaje poprzedni ImageUrl
        var updated = await this.productService.UpdateAsync(id, dto);
        if (!updated)
        {
            return this.NotFound();
        }

        return this.RedirectToAction(nameof(this.Details), new { id });
    }

    // GET: /Products/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await this.productService.GetByIdAsync(id);
        if (product == null)
        {
            return this.NotFound();
        }

        return this.View(product);
    }

    // POST: /Products/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var deleted = await this.productService.DeleteAsync(id);
        if (!deleted)
        {
            return this.NotFound();
        }

        return this.RedirectToAction(nameof(this.Index));
    }

    // GET: /Products/Filter
    public async Task<IActionResult> Filter([FromQuery] ProductFilterDto filterDto)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View("Index", new List<ProductDto>());
        }

        var products = await this.productService.GetFilteredAsync(filterDto);
        return this.View("Index", products);
    }
}
