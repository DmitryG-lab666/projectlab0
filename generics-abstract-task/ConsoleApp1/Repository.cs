public class Repository<T> where T : Entity, new()
{
    private readonly List<T> _items = new List<T>();

    public int Count => _items.Count;

    public void Add(T item)
    {
        _items.Add(item);
    }

    public T GetAt(int index)
    {
        return _items[index];
    }

    public IEnumerable<T> GetAll()
    {
        return _items;
    }

    public T? FindById(int id)
    {
        foreach (T item in _items)
        {
            if (item.Id == id)
            {
                return item;
            }
        }
        return null;
    }

    public void PrintAllDetails()
    {
        foreach (T item in _items)
        {
            Console.WriteLine(item.GetEntityDetails());
        }
    }
}
