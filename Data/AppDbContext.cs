using BikeShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BikeShop.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }
   
    public DbSet<OrderComment> OrderComments { get; set; }

    public DbSet<Bike> Bikes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfiguracja relacji OrderComment -> Order bez cascade delete
        modelBuilder.Entity<OrderComment>()
            .HasOne(oc => oc.Order)
            .WithMany(o => o.Comments) // pamiętaj o kolekcji w Order!
            .HasForeignKey(oc => oc.OrderId)
            .OnDelete(DeleteBehavior.Restrict); // brak kaskady

        // Konfiguracja relacji OrderComment -> ApplicationUser z cascade delete (jeśli chcesz)
        modelBuilder.Entity<OrderComment>()
            .HasOne(oc => oc.CreatedByUser)
            .WithMany() // jeśli nie masz kolekcji komentarzy w ApplicationUser
            .HasForeignKey(oc => oc.CreatedByUserId)
            .OnDelete(DeleteBehavior.Cascade); // lub Restrict jeśli wolisz

    }
}