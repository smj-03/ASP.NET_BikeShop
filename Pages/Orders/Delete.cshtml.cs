using BikeShop.Data;
using BikeShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class DeleteModel : PageModel
{
    private readonly AppDbContext context;
    private readonly UserManager<ApplicationUser> userManager;

    public DeleteModel(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        this.context = context;
        this.userManager = userManager;
    }

    [BindProperty]
    public Order Order { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var user = await this.userManager.GetUserAsync(this.User);
        this.Order = await this.context.Orders.FirstOrDefaultAsync(o => o.Id == id && o.CustomerId == user.Id);

        if (this.Order == null)
        {
            return this.NotFound();
        }

        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var user = await this.userManager.GetUserAsync(this.User);
        var order = await this.context.Orders.FirstOrDefaultAsync(o => o.Id == id && o.CustomerId == user.Id);

        if (order == null)
        {
            return this.NotFound();
        }

        this.context.Orders.Remove(order);
        await this.context.SaveChangesAsync();

        return this.RedirectToPage("Index");
    }
}