using System;

public interface IAccountControl
{
    int Balance { get; }

    void Put(int sum);

    void Take(int sum);

    event Action<object, AccountEventArgs> TransactionOccurred;
}
