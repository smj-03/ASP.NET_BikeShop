﻿namespace BikeShop.Models;

public class OrderProductDetailsDto
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }
}