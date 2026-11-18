using System;
using System.Diagnostics;

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
        SafeMath.SafeDivide(10, 2);
        SafeMath.SafeDivide(10, 0);
    }

    static void Task2()
    {
        Console.WriteLine("\n=== Задание 2 ===");
        TransactionProcessor.ProcessTransaction(0, 0);
        TransactionProcessor.ProcessTransaction(-5, 3);
        TransactionProcessor.ProcessTransaction(10, 2);
    }

    static void Task3()
    {
        Console.WriteLine("\n=== Задание 3 ===");
        SquareCalculator.CalculateSquareTryCatch("15");
        SquareCalculator.CalculateSquareTryCatch("abc");
        SquareCalculator.CalculateSquareTryParse("15");
        SquareCalculator.CalculateSquareTryParse("abc");

        const int iterations = 10000;

        Benchmark("try-catch, 'abc'", iterations, () => SquareCalculator.IsValidTryCatch("abc"));
        Benchmark("TryParse,  'abc'", iterations, () => SquareCalculator.IsValidTryParse("abc"));
        Benchmark("try-catch, '15' ", iterations, () => SquareCalculator.IsValidTryCatch("15"));
        Benchmark("TryParse,  '15' ", iterations, () => SquareCalculator.IsValidTryParse("15"));
    }

    static void Benchmark(string name, int iterations, Func<bool> action)
    {
        action();
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
            action();
        sw.Stop();
        Console.WriteLine($"{name}: {sw.Elapsed.TotalMilliseconds:F3} мс за {iterations} вызовов");
    }
}
