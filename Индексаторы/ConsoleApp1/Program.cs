using System;
using System.Collections.Generic;

public static class Program
{
    public static void Main()
    {
        Task1();
        Task2();
        Task3();
    }

    static void Task1()
    {
        Console.WriteLine("=== Задание 1 ===");
        var library = new Library(new[]
        {
            new Book("Мастер и Маргарита", "М. Булгаков"),
            new Book("Преступление и наказание", "Ф. Достоевский"),
            new Book("Война и мир", "Л. Толстой"),
            new Book("Евгений Онегин", "А. Пушкин")
        });

        Console.WriteLine(library[1].Title);

        library[0] = new Book("Анна Каренина", "Л. Толстой");
        Console.WriteLine(library[0]);

        try
        {
            Book missing = library[99];
            Console.WriteLine(missing);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void Task2()
    {
        Console.WriteLine("\n=== Задание 2 ===");
        var catalog = new CompanyCatalog(new[]
        {
            new Person("Bob", "bob@mail.com", "Developer"),
            new Person("Alice", "alice@mail.com", "Manager"),
            new Person("Carol", "carol@mail.com", "Designer")
        });

        Console.WriteLine(catalog[0]);
        Console.WriteLine(catalog["Alice"]);

        try
        {
            Console.WriteLine(catalog["Dave"]);
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        var user = new UserContainer("Alice", "alice@mail.com", "Manager");
        Console.WriteLine($"{user["name"]} | {user["email"]} | {user["role"]}");
        user["role"] = "Director";
        Console.WriteLine(user["role"]);

        try
        {
            Console.WriteLine(user["phone"]);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void Task3()
    {
        Console.WriteLine("\n=== Задание 3 ===");
        var grid = new GameGrid(3, 3);

        for (int i = 0; i < grid.Rows; i++)
            for (int j = 0; j < grid.Cols; j++)
                grid.SetCell(i, j, i * grid.Cols + j + 1);

        for (int i = 0; i < grid.Rows; i++)
        {
            for (int j = 0; j < grid.Cols; j++)
                Console.Write($"{grid[i, j],3}");
            Console.WriteLine();
        }
    }
}
