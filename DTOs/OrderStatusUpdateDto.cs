using System.ComponentModel.DataAnnotations;
using BikeShop.Models.Enums;

namespace BikeShop.Models;

public class OrderStatusUpdateDto
{
    [Required]
    [EnumDataType(typeof(OrderStatus))]
    public OrderStatus NewStatus { get; set; }
}