public abstract class Entity
{
    public int Id { get; protected set; }
    public string Name { get; protected set; }

    protected Entity(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public abstract string Category { get; }

    public abstract string GetEntityDetails();

    public void PrintSummary()
    {
        Console.WriteLine($"[{Category}] Id: {Id}, Название: {Name}");
    }
}
