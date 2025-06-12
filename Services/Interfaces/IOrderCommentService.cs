using BikeShop.Models;

namespace BikeShop.Services;

public interface IOrderCommentService
{
    Task AddCommentAsync(int orderId, CreateOrderCommentDto createDto, string userId);

    Task<List<OrderCommentDto>> GetCommentsForOrderAsync(int orderId);
}