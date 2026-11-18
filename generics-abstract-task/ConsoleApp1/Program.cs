using System.Text;

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Repository<UserEntity> users = new Repository<UserEntity>();
        users.Add(new UserEntity(1, "Администратор Иван", "ivan@example.com", "Admin"));
        users.Add(new UserEntity(2, "Менеджер Анна", "anna@example.com", "User"));

        Repository<ProductEntity> products = new Repository<ProductEntity>();
        products.Add(new ProductEntity(101, "Ноутбук", 75000.0, 10));
        products.Add(new ProductEntity(102, "Мышь", 1500.5, 50));
        products.Add(new ProductEntity(103, "Клавиатура", 3200.0, 30));

        Console.WriteLine("=== Пользователи ===");
        users.PrintAllDetails();

        Console.WriteLine();
        Console.WriteLine("=== Товары ===");
        products.PrintAllDetails();

        Console.WriteLine();
        Console.WriteLine("=== Сводка (PrintSummary) ===");
        foreach (UserEntity user in users.GetAll())
        {
            user.PrintSummary();
        }
        foreach (ProductEntity product in products.GetAll())
        {
            product.PrintSummary();
        }

        Console.WriteLine();
        Console.WriteLine("=== Поиск товара по Id ===");
        ProductEntity? found = products.FindById(102);
        if (found != null)
        {
            Console.WriteLine("Найдено: " + found.GetEntityDetails());
        }
        else
        {
            Console.WriteLine("Товар не найден");
        }

        ProductEntity? notFound = products.FindById(999);
        Console.WriteLine(notFound == null ? "Id 999: не найдено (null)" : notFound.GetEntityDetails());

        Console.WriteLine();
        Console.WriteLine("=== Swap ===");
        ProductEntity first = products.GetAt(0);
        ProductEntity second = products.GetAt(1);
        Console.WriteLine($"До обмена: first = {first.Name}, second = {second.Name}");
        Helper.Swap(ref first, ref second);
        Console.WriteLine($"После обмена: first = {first.Name}, second = {second.Name}");

        int x = 5;
        int y = 10;
        Helper.Swap(ref x, ref y);
        Console.WriteLine($"Swap<int>: x = {x}, y = {y}");

        Console.WriteLine();
        Console.WriteLine($"Количество пользователей: {users.Count}, товаров: {products.Count}");
    }
}
