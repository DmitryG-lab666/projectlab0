public class Book
{
    public string Title { get; private set; }
    public string Author { get; private set; }
    public int Year { get; private set; }
    public string Isbn { get; private set; }

    public Book(string title, string author, int year, string isbn)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Название не может быть пустым", nameof(title));
        }
        if (string.IsNullOrWhiteSpace(author))
        {
            throw new ArgumentException("Автор не может быть пустым", nameof(author));
        }
        if (year < 0 || year > DateTime.Now.Year)
        {
            throw new ArgumentOutOfRangeException(nameof(year), "Некорректный год издания");
        }
        if (string.IsNullOrWhiteSpace(isbn))
        {
            throw new ArgumentException("ISBN не может быть пустым", nameof(isbn));
        }

        Title = title;
        Author = author;
        Year = year;
        Isbn = isbn;
    }

    public override string ToString()
    {
        if (string.IsNullOrWhiteSpace(Author))
        {
            return base.ToString() ?? nameof(Book);
        }
        return $"\"{Title}\" — {Author} ({Year} г.) [ISBN: {Isbn}]";
    }

    public override bool Equals(object? obj)
    {
        if (obj is Book other)
        {
            return Isbn == other.Isbn;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Isbn.GetHashCode();
    }
}
