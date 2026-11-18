using System.Linq;

namespace WarehouseApp;

public static class DatabaseInitializer
{
    public static void Initialize()
    {
        using var db = new WarehouseContext();
        db.Database.EnsureCreated();

        if (db.Categories.Any())
            return;

        db.Categories.AddRange(
            new Category { Id = 1, Name = "Электроника" },
            new Category { Id = 2, Name = "Бытовая техника" },
            new Category { Id = 3, Name = "Канцелярия" });

        db.Products.AddRange(
            new Product { Name = "Смартфон", Price = 45000m, Quantity = 12, CategoryId = 1 },
            new Product { Name = "Ноутбук", Price = 89990m, Quantity = 5, CategoryId = 1 },
            new Product { Name = "Наушники", Price = 7500m, Quantity = 30, CategoryId = 1 },
            new Product { Name = "Холодильник", Price = 62000m, Quantity = 4, CategoryId = 2 },
            new Product { Name = "Микроволновая печь", Price = 11500m, Quantity = 9, CategoryId = 2 },
            new Product { Name = "Пылесос", Price = 18900m, Quantity = 7, CategoryId = 2 },
            new Product { Name = "Тетрадь", Price = 55m, Quantity = 400, CategoryId = 3 },
            new Product { Name = "Ручка шариковая", Price = 30m, Quantity = 650, CategoryId = 3 });

        db.SaveChanges();
    }
}
