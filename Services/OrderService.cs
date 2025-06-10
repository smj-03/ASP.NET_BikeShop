using BikeShop.Data;
using BikeShop.Mappers;
using BikeShop.Models;
using BikeShop.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BikeShop.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext context;
    private readonly OrderMapper mapper;

    public OrderService(AppDbContext context, OrderMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<OrderDetailsDto> CreateAsync(CreateOrderDto dto)
    {
        var order = new Order
        {
            CustomerId = dto.CustomerId,
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            Items = new List<OrderItem>(),
        };

        foreach (var productDto in dto.Products)
        {
            var product = await this.context.Products.FindAsync(productDto.ProductId);
            if (product == null)
            {
                throw new ArgumentException($"Produkt o ID {productDto.ProductId} nie istnieje");
            }

            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = productDto.Quantity,
            });
        }

        this.context.Orders.Add(order);
        await this.context.SaveChangesAsync();

        await this.context.Entry(order)
            .Collection(o => o.Items)
            .Query()
            .Include(i => i.Product)
            .LoadAsync();

        return this.mapper.ToOrderDetailsDto(order);
    }

    public async Task<OrderDetailsDto?> GetByIdAsync(int id)
    {
        var order = await this.context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Include(o => o.Comments)        // dołącz komentarze zamówienia
            .FirstOrDefaultAsync(o => o.Id == id);

        return order == null ? null : this.mapper.ToOrderDetailsDto(order);
    }

    public async Task<bool> UpdateStatusAsync(int id, OrderStatusUpdateDto dto)
    {
        var order = await this.context.Orders.FindAsync(id);
        if (order == null)
        {
            return false;
        }

        if (order.Status != OrderStatus.Pending)
        {
            throw new InvalidOperationException("Status można zmienić tylko, jeśli jest Pending.");
        }

        if (dto.NewStatus != OrderStatus.Completed && dto.NewStatus != OrderStatus.Canceled)
        {
            throw new ArgumentException("Dozwolone statusy to tylko Completed i Canceled.");
        }

        order.Status = dto.NewStatus;
        await this.context.SaveChangesAsync();
        return true;
    }
    
    public async Task<List<Order>> GetAllAsync()
    {
        return await context.Orders
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }
}