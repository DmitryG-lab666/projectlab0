public class ProductEntity : Entity
{
    public double Price { get; set; }
    public int StockQuantity { get; set; }

    public ProductEntity() : base(0, string.Empty)
    {
    }

    public ProductEntity(int id, string name, double price, int stockQuantity) : base(id, name)
    {
        Price = price;
        StockQuantity = stockQuantity;
    }

    public override string Category => "Товары";

    public override string GetEntityDetails()
    {
        return $"Товар #{Id}: {Name}, Цена: {Price:F2}, Остаток на складе: {StockQuantity}";
    }
}
