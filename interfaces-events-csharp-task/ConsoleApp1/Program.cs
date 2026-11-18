using System;

public static class Program
{
    public static void Main()
    {
        IAccountControl account = new BankAccount(1000, DateTime.Now);

        account.TransactionOccurred += ShowMessage;
        account.TransactionOccurred += (sender, e) =>
            Console.WriteLine($"[Лямбда] Сумма: {e.Sum} руб. | Сообщение: {e.Message}");

        Console.WriteLine("--- Операции с двумя обработчиками ---");
        account.Put(500);
        account.Take(300);
        account.Take(2000);

        account.TransactionOccurred -= ShowMessage;

        Console.WriteLine("\n--- После отписки именованного обработчика ---");
        account.Put(100);

        Console.WriteLine();
        IReportable report = (IReportable)account;
        Console.WriteLine(report.GetReportInfo());
    }

    static void ShowMessage(object sender, AccountEventArgs e)
    {
        Console.WriteLine($"[Метод] {e.Message}");
    }
}
