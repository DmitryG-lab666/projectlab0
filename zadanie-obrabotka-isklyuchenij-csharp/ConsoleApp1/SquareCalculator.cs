using System;

public static class SquareCalculator
{
    public static void CalculateSquareTryCatch(string data)
    {
        try
        {
            int val = int.Parse(data);
            Console.WriteLine($"Квадрат: {val * val}");
        }
        catch (FormatException)
        {
            Console.WriteLine("Некорректный формат числа");
        }
    }

    public static void CalculateSquareTryParse(string data)
    {
        if (int.TryParse(data, out int val))
            Console.WriteLine($"Квадрат: {val * val}");
        else
            Console.WriteLine("Некорректный ввод (без исключения)");
    }

    public static bool IsValidTryCatch(string data)
    {
        try
        {
            int.Parse(data);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public static bool IsValidTryParse(string data) => int.TryParse(data, out _);
}
