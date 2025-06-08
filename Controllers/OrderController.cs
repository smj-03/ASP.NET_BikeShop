using System.Security.Claims;
using BikeShop.Models;
using BikeShop.Models.Enums;
using BikeShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace BikeShop.Controllers;

[Route("orders")]
public class OrderController : Controller
{
    private readonly IOrderService orderService;

    public OrderController(IOrderService orderService)
    {
        this.orderService = orderService;
    }
    
    // GET: orders/create
    [HttpGet("create")]
    public IActionResult Create()
    {
        // Wyświetl formularz tworzenia zamówienia
        return View(new CreateOrderDto());
    }
    
    // POST: orders/create
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateOrderDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Challenge();
        }

        dto.CustomerId = userId;

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return View(dto);
        }

        try
        {
            var result = await orderService.CreateAsync(dto);
            return RedirectToAction(nameof(Details), new { id = result.Id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }


    // GET: orders/details/5
    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var order = await orderService.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    // GET: orders/editstatus/5
    [HttpGet("editstatus/{id}")]
    public async Task<IActionResult> EditStatus(int id)
    {
        var order = await orderService.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        if (!Enum.TryParse<OrderStatus>(order.Status, out var statusEnum))
        {
            return BadRequest("Niepoprawny status zamówienia.");
        }

        var dto = new OrderStatusUpdateDto
        {
            NewStatus = statusEnum
        };

        return View(dto);
    }

    // POST: orders/editstatus/5
    [HttpPost("editstatus/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditStatus(int id, OrderStatusUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        try
        {
            var success = await orderService.UpdateStatusAsync(id, dto);
            if (!success)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }
}
