using System;

public class BankAccount : IAccountControl, IReportable
{
    public int Balance { get; private set; }
    public DateTime CreatedAt { get; }

    public event Action<object, AccountEventArgs> TransactionOccurred;

    public BankAccount(int initialBalance, DateTime createdAt)
    {
        Balance = initialBalance;
        CreatedAt = createdAt;
    }

    public void Put(int sum)
    {
        Balance += sum;
        TransactionOccurred?.Invoke(this, new AccountEventArgs("Пополнение счета", sum));
    }

    public void Take(int sum)
    {
        if (Balance >= sum)
        {
            Balance -= sum;
            TransactionOccurred?.Invoke(this, new AccountEventArgs("Снятие со счета", sum));
        }
        else
        {
            TransactionOccurred?.Invoke(this, new AccountEventArgs("Недостаточно средств", 0));
        }
    }

    public string GetReportInfo() =>
        $"Счет открыт: {CreatedAt:dd.MM.yyyy}, текущий баланс: {Balance} руб.";
}
