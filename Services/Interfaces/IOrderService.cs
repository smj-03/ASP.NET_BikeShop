using BikeShop.Models;
using BikeShop.Models.Enums;

namespace BikeShop.Services;

public interface IOrderService
{
    Task<OrderDetailsDto> CreateAsync(CreateOrderDto dto);

    Task<OrderDetailsDto?> GetByIdAsync(int id);

    Task<bool> UpdateStatusAsync(int id, OrderStatusUpdateDto dto);

    Task<List<Order>> GetAllAsync();

    Task<int> GetFilteredCountAsync(OrderFilterDto filter);

    Task<List<OrderViewDto>> GetFilteredPagedAsync(OrderFilterDto filter, int page, int pageSize);

    Task<List<OrderViewDto>> GetPagedWithDetailsAsync(int page, int pageSize);
}