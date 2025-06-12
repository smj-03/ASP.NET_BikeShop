using BikeShop.Data;
using BikeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeShop.Services;

public class OrderCommentService : IOrderCommentService
{
    private readonly AppDbContext context;
    private readonly OrderCommentMapper mapper;

    public OrderCommentService(AppDbContext context, OrderCommentMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task AddCommentAsync(int orderId, CreateOrderCommentDto createDto, string userId)
    {
        var entity = this.mapper.ToEntity(createDto);
        entity.OrderId = orderId;
        entity.CreatedByUserId = userId;
        entity.CreatedAt = DateTime.UtcNow;

        this.context.OrderComments.Add(entity);
        await this.context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<List<OrderCommentDto>> GetCommentsForOrderAsync(int orderId)
    {
        var comments = await this.context.OrderComments
            .Where(c => c.OrderId == orderId)
            .Include(c => c.CreatedByUser)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();

        return comments.Select(c =>
        {
            var dto = this.mapper.ToDto(c);
            dto.CreatedByUserName = c.CreatedByUser?.UserName ?? "Nieznany";
            return dto;
        }).ToList();
    }
}
