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
    private readonly IOrderService _orderService;
    private readonly AppDbContext _context;

    public CreateModel(IOrderService orderService, AppDbContext context)
    {
        _orderService = orderService;
        _context = context;
    }

    [BindProperty]
    public List<OrderProductDto> Products { get; set; } = new() { new(), new() };

    public List<Product> AvailableProducts { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        AvailableProducts = await _context.Products.ToListAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        AvailableProducts = await _context.Products.ToListAsync();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var dto = new CreateOrderDto
        {
            CustomerId = userId,
            Products = Products
        };

        try
        {
            var result = await _orderService.CreateAsync(dto);
            return RedirectToPage("/Orders/Details", new { id = result.Id });
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
} 