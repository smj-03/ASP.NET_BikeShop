using System.Security.Claims;
using BikeShop.Models;
using BikeShop.Models.Enums;

using BikeShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BikeShop.Controllers;

[Route("orders")]
public class OrderController : Controller
{
    private readonly IOrderService orderService;
    private readonly IProductService productService;

    public OrderController(IOrderService orderService, IProductService productService)
    {
        this.orderService = orderService;
        this.productService = productService;
    }

    // GET: orders/create
    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        var products = (await this.productService.GetAllAsync())
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

        ViewBag.Products = products;

        return this.View(new CreateOrderDto());
    }

    // POST: orders/create
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateOrderDto dto)
    {
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return this.Challenge();
        }

        dto.CustomerId = userId;

        if (!this.ModelState.IsValid)
        {

            var products = (await this.productService.GetAllAsync())
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();

            ViewBag.Products = products;

            return this.View(dto);
        }

        try
        {
            var result = await this.orderService.CreateAsync(dto);
            return this.RedirectToAction(nameof(this.Details), new { id = result.Id });
        }
        catch (Exception ex)
        {
            var products = (await this.productService.GetAllAsync())
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();

            ViewBag.Products = products;
            this.ModelState.AddModelError(string.Empty, ex.Message);
            return this.View(dto);
        }
    }

    // GET: orders/details/5
    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var order = await this.orderService.GetByIdAsync(id);
        if (order == null)
        {
            return this.NotFound();
        }

        return this.View(order);
    }

    // GET: orders/editstatus/5
    [HttpGet("editstatus/{id}")]
    public async Task<IActionResult> EditStatus(int id)
    {
        var order = await this.orderService.GetByIdAsync(id);
        if (order == null)
        {
            return this.NotFound();
        }
        if (!Enum.TryParse<OrderStatus>(order.Status, out var statusEnum))
        {
            return this.BadRequest("Niepoprawny status zamówienia.");
        }

        var dto = new OrderStatusUpdateDto
        {
            NewStatus = statusEnum,
        };

        return this.View(dto);
    }

    // POST: orders/editstatus/5
    [HttpPost("editstatus/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditStatus(int id, OrderStatusUpdateDto dto)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(dto);
        }

        try
        {
            var success = await this.orderService.UpdateStatusAsync(id, dto);
            if (!success)
            {
                return this.NotFound();
            }

            return this.RedirectToAction(nameof(this.Details), new { id });
        }
        catch (InvalidOperationException ex)
        {
            this.ModelState.AddModelError(string.Empty, ex.Message);
            return this.View(dto);
        }
        catch (ArgumentException ex)
        {
            this.ModelState.AddModelError(string.Empty, ex.Message);
            return this.View(dto);
        }
    }
}
