namespace CorpFleet;

public class VehicleEventArgs : EventArgs
{
    public string Message { get; }
    public double OperationPrice { get; }

    public VehicleEventArgs(string message, double operationPrice)
    {
        Message = message;
        OperationPrice = operationPrice;
    }
}

public class VehicleRepository : IRepository<Vehicle>
{
    private readonly List<Vehicle> _items = new();

    public List<Vehicle> GetAll() => new(_items);
    public Vehicle? GetById(int id) => _items.FirstOrDefault(v => v.Id == id);
    public void Add(Vehicle item) => _items.Add(item);

    public bool Delete(int id)
    {
        var v = GetById(id);
        return v != null && _items.Remove(v);
    }

    public void SortInPlace() => _items.Sort(); // использует IComparable<Vehicle>
}

public class FleetManager
{
    public event Action<object, VehicleEventArgs>? StatusChanged;

    public VehicleRepository Repo { get; } = new();

    public void Notify(string message, double price = 0) =>
        StatusChanged?.Invoke(this, new VehicleEventArgs(message, price));

    public void Load(IEnumerable<Vehicle> vehicles)
    {
        int n = 0;
        foreach (var v in vehicles) { Repo.Add(v); n++; }
        Notify($"Загружено ТС из БД: {n}");
    }

    public void AddVehicle(Vehicle v)
    {
        Repo.Add(v);
        Notify($"Добавлено: {v.Model}", v.BasePrice);
    }

    public bool DeleteVehicle(int id)
    {
        var v = Repo.GetById(id);
        bool ok = Repo.Delete(id);
        if (ok && v != null) Notify($"Удалено: {v.Model}", v.BasePrice);
        return ok;
    }

    public Vehicle? CloneVehicle(int id)
    {
        if (Repo.GetById(id) is not { } src) return null;
        var copy = (Vehicle)src.Clone();
        copy.SetId(Repo.GetAll().Max(x => x.Id) + 1);
        Repo.Add(copy);
        Notify($"Глубокая копия: {copy.Model}", copy.BasePrice);
        return copy;
    }

    public void SortByPrice()
    {
        Repo.SortInPlace();
        Notify("Сортировка по BasePrice (IComparable)");
    }

    public List<Vehicle> Find(Predicate<Vehicle> predicate) => Repo.GetAll().FindAll(predicate);

    public double Total(Func<Vehicle, double> selector) => Repo.GetAll().Sum(selector);

    public int CountPowerful(int limit)
    {
        int counter = 0;                                   
        Repo.GetAll().ForEach(v => { if (v.Power > limit) counter++; });
        Notify($"Мощнее {limit} л.с.: {counter} шт.");
        return counter;
    }

    public Func<double, double> MakeDiscount(double percent) =>
        price => price * (1 - percent / 100.0);            
}
