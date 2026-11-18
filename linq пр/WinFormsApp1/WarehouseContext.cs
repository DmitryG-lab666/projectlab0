using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace WarehouseApp;

public class WarehouseContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "warehouse.db");
        optionsBuilder.UseSqlite($"Data Source={path}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired();
            entity.Property(p => p.Price).HasColumnType("REAL");
            entity.HasOne<Category>().WithMany().HasForeignKey(p => p.CategoryId);
        });
    }
}
