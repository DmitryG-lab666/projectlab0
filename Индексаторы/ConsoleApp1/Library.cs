using System;

public class Library
{
    private Book[] books;

    public Library(Book[] initialBooks) => books = initialBooks;

    public Book this[int index]
    {
        get
        {
            if (index >= 0 && index < books.Length)
                return books[index];
            throw new ArgumentOutOfRangeException(nameof(index), "Индекс вне границ библиотеки.");
        }
        set
        {
            if (index >= 0 && index < books.Length)
                books[index] = value;
            else
                throw new ArgumentOutOfRangeException(nameof(index), "Индекс вне границ библиотеки.");
        }
    }
}
