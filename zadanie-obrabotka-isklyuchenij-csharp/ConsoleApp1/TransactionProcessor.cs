using System;

public static class TransactionProcessor
{
    public static void ProcessTransaction(int errorCode, int attempts)
    {
        int x = 1, y = 0;

        try
        {
            int res = x / y;
            Console.WriteLine(res);
        }
        catch (DivideByZeroException) when (attempts == 0)
        {
            Console.WriteLine("Фильтр 1: Попытки исчерпаны (attempts == 0)");
        }
        catch (DivideByZeroException ex) when (errorCode < 0)
        {
            Console.WriteLine($"Фильтр 2: Отрицательный код {errorCode}: {ex.Message}");
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine($"Общий catch: {ex.Message}");
        }
    }
}
