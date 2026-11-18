using System.Text;

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Book book1 = new Book("Программирование на C#", "Иван Петров", 2020, "978-5-9775-4000-1");
        Book book2 = new Book("Программирование на C# (2-е издание)", "Иван Петров", 2022, "978-5-9775-4000-1");
        Book book3 = new Book("Алгоритмы и структуры данных", "Анна Сидорова", 2019, "978-5-4461-1111-2");

        Console.WriteLine("=== ToString() ===");
        Console.WriteLine(book1);
        Console.WriteLine(book2);
        Console.WriteLine(book3);

        Console.WriteLine();
        Console.WriteLine("=== GetType() ===");
        Console.WriteLine("Полное имя типа: " + book1.GetType().FullName);
        Console.WriteLine("book1.GetType() == typeof(Book): " + (book1.GetType() == typeof(Book)));
        Console.WriteLine("book1 is Book: " + (book1 is Book));

        Console.WriteLine();
        Console.WriteLine("=== Equals() ===");
        Console.WriteLine("book1.Equals(book2): " + book1.Equals(book2));
        Console.WriteLine("book1.Equals(book3): " + book1.Equals(book3));
        Console.WriteLine("book1.Equals(null): " + book1.Equals(null));
        Console.WriteLine("book1.Equals(\"строка\"): " + book1.Equals("строка"));

        Console.WriteLine();
        Console.WriteLine("=== Ссылки и значения ===");
        Console.WriteLine("book1 == book2 (сравнение ссылок): " + (book1 == book2));
        Console.WriteLine("book1.Equals(book2) (сравнение по ISBN): " + book1.Equals(book2));
        Console.WriteLine("ReferenceEquals(book1, book2): " + ReferenceEquals(book1, book2));
        Book book1Ref = book1;
        Console.WriteLine("book1 == book1Ref: " + (book1 == book1Ref));

        Console.WriteLine();
        Console.WriteLine("=== GetHashCode() ===");
        Console.WriteLine("book1: " + book1.GetHashCode());
        Console.WriteLine("book2: " + book2.GetHashCode());
        Console.WriteLine("book3: " + book3.GetHashCode());
        Console.WriteLine("Хеш-коды book1 и book2 совпадают: " + (book1.GetHashCode() == book2.GetHashCode()));
    }
}
