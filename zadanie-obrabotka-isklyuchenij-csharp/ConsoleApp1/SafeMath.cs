using System;

public static class SafeMath
{
    public static void SafeDivide(int x, int y)
    {
        try
        {
            int result = x / y;
            Console.WriteLine($"Результат {x} / {y} = {result}");
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine("Ошибка деления на 0: " + ex.Message);
        }
        finally
        {
            Console.WriteLine("Операция деления завершена (блок finally)");
        }
    }
}
