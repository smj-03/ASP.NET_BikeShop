using BikeShop.Data;
using BikeShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public DetailsModel(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public Order Order { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Unauthorized();

        Order = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id && o.CustomerId == user.Id);

        if (Order == null)
            return NotFound();

        return Page();
    }
}