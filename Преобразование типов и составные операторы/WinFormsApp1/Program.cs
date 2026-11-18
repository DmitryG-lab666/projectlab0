using System;

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
        StorageSize size1 = 2097152L;
        long rawBytes = (long)size1;
        MegaByte mb = size1;
        StorageSize size2 = (StorageSize)new MegaByte { Value = 1.5 };

        Console.WriteLine($"size1.Bytes = {size1.Bytes}");
        Console.WriteLine($"rawBytes    = {rawBytes}");
        Console.WriteLine($"mb.Value    = {mb.Value} МБ");
        Console.WriteLine($"size2.Bytes = {size2.Bytes}");

        StorageSize lossy = (StorageSize)new MegaByte { Value = 0.0000009 };
        Console.WriteLine($"0.0000009 МБ -> {lossy.Bytes} байт");
    }

    static void Task2()
    {
        Console.WriteLine("\n=== Задание 2 ===");
        var warehouse = new Inventory { TotalUnits = 100 };
        Console.WriteLine($"Id до: {warehouse.Id}");
        warehouse += 50;
        warehouse += 25;
        Console.WriteLine($"Id после: {warehouse.Id}, TotalUnits = {warehouse.TotalUnits}");

        var other = new Inventory { TotalUnits = 10 };
        warehouse += other;
        Console.WriteLine($"После объединения партий: {warehouse.TotalUnits}");
    }

    static void Task3()
    {
        Console.WriteLine("\n=== Задание 3 ===");
        var warehouse = new Inventory { TotalUnits = 100 };

        warehouse++;
        Console.WriteLine($"После warehouse++: {warehouse.TotalUnits}, Id = {warehouse.Id}");

        Inventory next = ++warehouse;
        Console.WriteLine($"next.Id = {next.Id}, next.TotalUnits = {next.TotalUnits}");

        Inventory regular = new Inventory { TotalUnits = 0 };
        Inventory express = new ExpressInventory { TotalUnits = 0 };
        regular += 10;
        express += 10;
        Console.WriteLine($"Inventory: {regular.TotalUnits}, ExpressInventory: {express.TotalUnits}");
    }
}
