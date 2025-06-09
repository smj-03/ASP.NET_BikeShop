using System.Threading.Tasks;
using BikeShop.Data;
using BikeShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class DetailsModel : PageModel
{
    private readonly AppDbContext context;
    private readonly UserManager<ApplicationUser> userManager;

    public DetailsModel(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        this.context = context;
        this.userManager = userManager;
    }

    public Order Order { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var user = await this.userManager.GetUserAsync(this.User);

        if (user == null)
        {
            return this.Unauthorized();
        }

        this.Order = await this.context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id && o.CustomerId == user.Id);

        if (this.Order == null)
        {
            return this.NotFound();
        }

        return this.Page();
    }
}