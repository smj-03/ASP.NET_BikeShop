using BikeShop.Models;
using BikeShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BikeShop.Controllers;

[Authorize(Roles = "Admin,Employee")]
public class OrderCommentsController : Controller
{
    private readonly IOrderCommentService commentService;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IOrderService orderService;

    public OrderCommentsController(IOrderCommentService commentService, UserManager<ApplicationUser> userManager, IOrderService orderService)
    {
        this.commentService = commentService;
        this.userManager = userManager;
        this.orderService = orderService;
    }

    // Pobierz komentarze dla zamówienia
    [HttpGet]
    public async Task<IActionResult> List(int orderId)
    {
        var comments = await this.commentService.GetCommentsForOrderAsync(orderId);
        return this.View(comments); // lub Json(comments) jeśli API
    }

    // Wyświetl formularz dodawania komentarza
    [HttpGet]
    public async Task<IActionResult> Add()
    {
        var orders = await this.orderService.GetAllAsync();
        this.ViewBag.Orders = orders.Select(o => new SelectListItem
        {
            Value = o.Id.ToString(),
            Text = $"#{o.Id} - {o.CreatedAt:yyyy-MM-dd}",
        }).ToList();

        return this.View(new CreateOrderCommentDto());
    }

    // Dodaj komentarz do zamówienia
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(CreateOrderCommentDto createDto)
    {
        if (!this.ModelState.IsValid)
        {
            // Przy ponownym wyświetlaniu widoku potrzebna jest lista zamówień
            var orders = await this.orderService.GetAllAsync();
            this.ViewBag.Orders = orders.Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = $"#{o.Id} - {o.CreatedAt:yyyy-MM-dd}",
            }).ToList();

            return this.View(createDto);
        }

        var userId = this.userManager.GetUserId(this.User);
        await this.commentService.AddCommentAsync(createDto.OrderId, createDto, userId);

        return this.RedirectToAction("Details", "Orders", new { id = createDto.OrderId });
    }
}