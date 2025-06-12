namespace BikeShop.Models;

public class OrderComment
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public Order Order { get; set; }

    public string CommentText { get; set; }

    public string CreatedByUserId { get; set; }

    public ApplicationUser CreatedByUser { get; set; }

    public DateTime CreatedAt { get; set; }
}