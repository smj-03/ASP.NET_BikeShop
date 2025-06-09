using System.Security.Claims;
using BikeShop.Data;
using BikeShop.Models;
using BikeShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BikeShop.Pages.Orders;

public class CreateModel : PageModel
{
    private readonly IOrderService orderService;
    private readonly AppDbContext context;

    public CreateModel(IOrderService orderService, AppDbContext context)
    {
        this.orderService = orderService;
        this.context = context;
    }

    [BindProperty]
    public List<OrderProductDto> Products { get; set; } = new () { new (), new () };

    public List<Product> AvailableProducts { get; set; } = new ();

    public async Task<IActionResult> OnGetAsync()
    {
        this.AvailableProducts = await this.context.Products.ToListAsync();
        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        this.AvailableProducts = await this.context.Products.ToListAsync();

        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return this.Unauthorized();
        }

        var dto = new CreateOrderDto
        {
            CustomerId = userId,
            Products = this.Products,
        };

        try
        {
            var result = await this.orderService.CreateAsync(dto);
            return this.RedirectToPage("/Orders/Details", new { id = result.Id });
        }
        catch (ArgumentException ex)
        {
            this.ModelState.AddModelError(string.Empty, ex.Message);
            return this.Page();
        }
    }
}