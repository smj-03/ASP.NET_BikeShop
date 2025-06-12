using System.ComponentModel.DataAnnotations;
using BikeShop.Models.Enums;

namespace BikeShop.Models;

public class Order
{
    public int Id { get; set; }

    [Required]
    public string? CustomerId { get; set; }

    public ApplicationUser Customer { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Zamówienie musi się składać z conajmniej jedego zamówienia.")]
    public List<int> BikeIds { get; set; } = new ();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public List<OrderItem> Items { get; set; } = new ();
    
    public ICollection<OrderComment> Comments { get; set; }
}

