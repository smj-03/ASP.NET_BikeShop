using BikeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeShop.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) {
    public DbSet<Bike> Bikes { get; set; }
}