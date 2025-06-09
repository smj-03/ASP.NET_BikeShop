using BikeShop.Models;
using Riok.Mapperly.Abstractions;

namespace BikeShop.Mappers;

[Mapper]
public partial class OrderMapper
{
    public partial Order ToOrder(CreateOrderDto dto);

    public partial OrderDetailsDto ToOrderDetailsDto(Order order);

    [MapProperty(nameof(OrderItem.ProductId), nameof(OrderProductDetailsDto.ProductId))]
    [MapProperty(nameof(OrderItem.Product.Name), nameof(OrderProductDetailsDto.ProductName))]
    public partial OrderProductDetailsDto ToProductDetailsDto(OrderItem item);
}