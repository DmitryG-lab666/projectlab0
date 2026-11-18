namespace CorpFleet;

public interface IRepository<T> where T : class
{
    List<T> GetAll();
    T? GetById(int id);
    void Add(T item);
    bool Delete(int id);
}

public interface IStorable
{
    string FilePath { get; set; }
    void Save();
}

public interface IExportable
{
    string GetExportInfo();
}

public class Category
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = "";
    public double TaxRate { get; set; }
}

public class ServiceRecord : ICloneable
{
    public string Name { get; set; } = "";
    public double Cost { get; set; }
    public object Clone() => new ServiceRecord { Name = Name, Cost = Cost };
}

public abstract class Vehicle : IComparable<Vehicle>, ICloneable, IStorable
{
    public int Id { get; private set; }
    public string Model { get; private set; }
    public int Power { get; private set; }
    public double BasePrice { get; private set; }
    public int CategoryId { get; private set; }
    public double TaxRate { get; private set; }
    public List<ServiceRecord> Services { get; private set; } = new();
    public string FilePath { get; set; } = "";

    protected Vehicle(int id, string model, int power, double price)
    {
        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Модель не может быть пустой.", nameof(model));
        if (power <= 0)
            throw new ArgumentException("Мощность должна быть положительной.", nameof(power));
        if (price <= 0)
            throw new ArgumentException("Цена должна быть положительной.", nameof(price));

        Id = id;
        Model = model.Trim();
        Power = power;
        BasePrice = price;
    }

    public void ApplyCategory(Category category)
    {
        CategoryId = category.Id;
        TaxRate = category.TaxRate;
    }

    internal void SetId(int id) => Id = id;

    public virtual double CalculateTax() => BasePrice * TaxRate;

    public override string ToString() =>
        $"{GetType().Name} #{Id}: {Model}, {Power} л.с., {BasePrice:N0}";

    public override bool Equals(object? obj) =>
        obj is Vehicle other && Model == other.Model && Power == other.Power;

    public override int GetHashCode() => HashCode.Combine(Model, Power);

    public int CompareTo(Vehicle? other) =>
        other is null ? 1 : BasePrice.CompareTo(other.BasePrice);

    public virtual object Clone()
    {
        var copy = (Vehicle)MemberwiseClone();
        copy.Services = Services.Select(s => (ServiceRecord)s.Clone()).ToList();
        return copy;
    }

    public void Save()
    {
        if (string.IsNullOrEmpty(FilePath))
            FilePath = Path.Combine(AppContext.BaseDirectory, $"vehicle_{Id}.txt");
        File.WriteAllText(FilePath, ToString());
    }
}

public class CargoVehicle : Vehicle, IExportable
{
    public double MaxLoadTon { get; private set; }

    public CargoVehicle(int id, string model, int power, double price, double maxLoadTon)
        : base(id, model, power, price)
    {
        if (maxLoadTon <= 0)
            throw new ArgumentException("Грузоподъёмность должна быть положительной.", nameof(maxLoadTon));
        MaxLoadTon = maxLoadTon;
    }

    public override double CalculateTax() => base.CalculateTax() + MaxLoadTon * 500;

    public override string ToString() => base.ToString() + $", груз {MaxLoadTon:0.#} т";

    string IExportable.GetExportInfo() =>
        $"[CARGO] {Model}; мощность {Power} л.с.; грузоподъёмность {MaxLoadTon:0.#} т; налог {CalculateTax():N0}";
}

public class PassengerVehicle : Vehicle, IExportable
{
    public int Seats { get; private set; }

    public PassengerVehicle(int id, string model, int power, double price, int seats = 5)
        : base(id, model, power, price)
    {
        Seats = seats > 0 ? seats : 5;
    }

    public override string ToString() => base.ToString() + $", мест {Seats}";

    public string GetExportInfo() =>
        $"[PASSENGER] {Model}; мощность {Power} л.с.; мест {Seats}; налог {CalculateTax():N0}";
}
