using System.ComponentModel.DataAnnotations;

namespace BikeShop.Models;

public class CreateOrderCommentDto
{
    [Required]
    public int OrderId { get; set; }

    [Required]
    public string CommentText { get; set; }
}