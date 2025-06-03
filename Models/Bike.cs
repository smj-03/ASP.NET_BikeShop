using System.ComponentModel.DataAnnotations;

namespace BikeShop.Models;

public class Bike {
    [Key]
    public int Id { get; set; }

    [Required]
    public string ModelName { get; set; }

    [Required]
    public double Price { get; set; }
}