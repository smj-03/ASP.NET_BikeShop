using BikeShop.Data;
using BikeShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteModel(AppDbContext context, UserManager<ApplicationUser> userManager)
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

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id && o.CustomerId == user.Id);

        if (order == null)
            return NotFound();

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return RedirectToPage("Index");
    }
}