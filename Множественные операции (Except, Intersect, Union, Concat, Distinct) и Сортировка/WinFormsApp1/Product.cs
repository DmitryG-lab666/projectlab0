using System;

namespace LinqSetOperations
{
    public class Product : IEquatable<Product>
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Category { get; private set; }
        public decimal Price { get; private set; }
        public string Supplier { get; private set; }

        public Product(int id, string name, string category, decimal price, string supplier)
        {
            Id = id;
            Name = name;
            Category = category;
            Price = price;
            Supplier = supplier;
        }

        // Два товара идентичны, если совпадают Name и Category (без учёта регистра)
        public bool Equals(Product other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase)
                && string.Equals(Category, other.Category, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj) => Equals(obj as Product);

        // Хеш обязан быть согласован с Equals: считаем по тем же полям и тем же правилам регистра
        public override int GetHashCode()
        {
            int h1 = Name == null ? 0 : StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
            int h2 = Category == null ? 0 : StringComparer.OrdinalIgnoreCase.GetHashCode(Category);
            return HashCode.Combine(h1, h2);
        }

        public override string ToString() => $"{Name} ({Category}) - {Price:C} [{Supplier}]";
    }
}
