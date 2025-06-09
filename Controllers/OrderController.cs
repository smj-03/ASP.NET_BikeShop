using System.Security.Claims;
using BikeShop.Models;
using BikeShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace BikeShop.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : Controller
{
    private readonly IOrderService orderService;

    public OrderController(IOrderService orderService)
    {
        this.orderService = orderService;
    }

    // POST: api/orders
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return this.Unauthorized();
        }

        dto.CustomerId = userId;

        try
        {
            var result = await this.orderService.CreateAsync(dto);
            return this.CreatedAtAction(nameof(this.GetOrderById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return this.BadRequest(new { error = ex.Message });
        }
    }

    // GET: api/orders/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await this.orderService.GetByIdAsync(id);
        if (order == null)
        {
            return this.NotFound();
        }

        return this.Ok(order);
    }

    // PUT: api/orders/5/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatusUpdateDto dto)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        try
        {
            var success = await this.orderService.UpdateStatusAsync(id, dto);
            return success ? this.NoContent() : this.NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return this.BadRequest(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return this.BadRequest(new { error = ex.Message });
        }
    }
}