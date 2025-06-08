using BikeShop.Data;
using BikeShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public IndexModel(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IList<Order> Orders { get; set; }     

    public async Task OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        Orders = await _context.Orders
            .Where(o => o.CustomerId == user.Id)
            .ToListAsync();
    }
}   
