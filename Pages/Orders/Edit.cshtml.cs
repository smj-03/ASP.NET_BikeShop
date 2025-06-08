using BikeShop.Data;
using BikeShop.Models;
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
    public Order Order { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var user = await _userManager.GetUserAsync(User);

        Order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id && o.CustomerId == user.Id);

        if (Order == null)
            return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var user = await _userManager.GetUserAsync(User);
        var orderToUpdate = await _context.Orders.FirstOrDefaultAsync(o => o.Id == Order.Id && o.CustomerId == user.Id);

        if (orderToUpdate == null)
            return NotFound();

        orderToUpdate.CreatedAt = Order.CreatedAt;
        orderToUpdate.Status = Order.Status;

        await _context.SaveChangesAsync();

        return RedirectToPage("Index");
    }
}