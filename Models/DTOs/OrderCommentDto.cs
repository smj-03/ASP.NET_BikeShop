namespace BikeShop.Models;

public class OrderCommentDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string CommentText { get; set; }
    public string CreatedByUserName { get; set; }
    public DateTime CreatedAt { get; set; }
}