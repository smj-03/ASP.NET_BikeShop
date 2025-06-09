using BikeShop.Data;
using BikeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeShop.Services;

public class OrderCommentService : IOrderCommentService
{
    private readonly AppDbContext _context;
    private readonly OrderCommentMapper _mapper;

    public OrderCommentService(AppDbContext context, OrderCommentMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddCommentAsync(int orderId, CreateOrderCommentDto createDto, string userId)
    {
        var entity = _mapper.ToEntity(createDto);
        entity.OrderId = orderId;
        entity.CreatedByUserId = userId;
        entity.CreatedAt = DateTime.UtcNow;

        _context.OrderComments.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<OrderCommentDto>> GetCommentsForOrderAsync(int orderId)
    {
        var comments = await _context.OrderComments
            .Where(c => c.OrderId == orderId)
            .Include(c => c.CreatedByUser)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();

        return comments.Select(c =>
        {
            var dto = _mapper.ToDto(c);
            dto.CreatedByUserName = c.CreatedByUser?.UserName ?? "Nieznany";
            return dto;
        }).ToList();
    }
}
