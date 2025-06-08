using BikeShop.Data;
using BikeShop.Models;
using BikeShop.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class EditModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public EditModel(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty]
    public int Id { get; set; }

    [BindProperty]
    public OrderStatus Status { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var order = await _context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id && o.CustomerId == user.Id);

        if (order == null) return NotFound();

        Id = order.Id;
        Status = order.Status;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == Id && o.CustomerId == user.Id);

        if (order == null) return NotFound();

        order.Status = Status;

        await _context.SaveChangesAsync();

        return RedirectToPage("Details", new { id = Id });
    }
}