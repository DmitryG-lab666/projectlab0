using System;

public static class PaymentProcessor
{
    public static void ProcessTransaction()
    {
        throw new InvalidOperationException("Сбой соединения с сервером");
    }

    public static void ExecutePayment()
    {
        try
        {
            ProcessTransaction();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Лог сервиса]: Логгируем ошибку {ex.Message}");
            throw;
        }
    }

    public static void ExecutePaymentThrowEx()
    {
        try
        {
            ProcessTransaction();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Лог сервиса]: Логгируем ошибку {ex.Message}");
            throw ex;
        }
    }
}
