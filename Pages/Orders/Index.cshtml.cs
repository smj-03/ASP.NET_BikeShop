using BikeShop.Data;
using BikeShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class IndexModel : PageModel
{
    private readonly AppDbContext context;
    private readonly UserManager<ApplicationUser> userManager;

    public IndexModel(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        this.context = context;
        this.userManager = userManager;
    }

    public IList<Order> Orders { get; set; }

    public async Task OnGetAsync()
    {
        var user = await this.userManager.GetUserAsync(this.User);
        this.Orders = await this.context.Orders
            .Where(o => o.CustomerId == user.Id)
            .ToListAsync();
    }
}
