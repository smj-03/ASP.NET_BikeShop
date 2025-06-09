using BikeShop.Models;
using Riok.Mapperly.Abstractions;

namespace BikeShop.Data;

[Mapper]
public partial class OrderCommentMapper
{
    // Mapowanie z encji na DTO
    public partial OrderCommentDto ToDto(OrderComment comment);

    // Mapowanie z DTO (lub innego obiektu) na encję (do tworzenia nowego komentarza)
    public partial OrderComment ToEntity(CreateOrderCommentDto createDto);
}