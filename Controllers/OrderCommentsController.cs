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
    private readonly IOrderCommentService _commentService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOrderService _orderService;

    public OrderCommentsController(IOrderCommentService commentService, UserManager<ApplicationUser> userManager, IOrderService orderService)
    {
        _commentService = commentService;
        _userManager = userManager;
        _orderService = orderService;
    }

    // Pobierz komentarze dla zamówienia
    [HttpGet]
    public async Task<IActionResult> List(int orderId)
    {
        var comments = await _commentService.GetCommentsForOrderAsync(orderId);
        return View(comments); // lub Json(comments) jeśli API
    }

    // Wyświetl formularz dodawania komentarza
    [HttpGet]
    public async Task<IActionResult> Add()
    {
        var orders = await _orderService.GetAllAsync();
        ViewBag.Orders = orders.Select(o => new SelectListItem
        {
            Value = o.Id.ToString(),
            Text = $"#{o.Id} - {o.CreatedAt:yyyy-MM-dd}"
        }).ToList();

        return View(new CreateOrderCommentDto());
    }

    // Dodaj komentarz do zamówienia
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(CreateOrderCommentDto createDto)
    {
        if (!ModelState.IsValid)
        {
            // Przy ponownym wyświetlaniu widoku potrzebna jest lista zamówień
            var orders = await _orderService.GetAllAsync();
            ViewBag.Orders = orders.Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = $"#{o.Id} - {o.CreatedAt:yyyy-MM-dd}"
            }).ToList();

            return View(createDto);
        }

        var userId = _userManager.GetUserId(User);
        await _commentService.AddCommentAsync(createDto.OrderId, createDto, userId);

        return RedirectToAction("Details", "Orders", new { id = createDto.OrderId });
    }
}