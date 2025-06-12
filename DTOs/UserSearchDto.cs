namespace BikeShop.Models;

public class UserSearchDto
{
    public string? Query { get; set; }

    public string? Role { get; set; } // "Client" or "Employee"
}