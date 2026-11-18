namespace ClinicCrm;

public interface IRepository<T> where T : class
{
    List<T> GetAll();
    T? GetById(int id);
    void Add(T item);
}

public interface IExportable
{
    string GetSummary();
}

public class MedicalLicense
{
    public string Number { get; set; }
    public DateTime ExpiryDate { get; set; }

    public MedicalLicense(string number, DateTime expiryDate)
    {
        Number = number;
        ExpiryDate = expiryDate;
    }
}

public class Department
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public decimal BaseRate { get; set; }
}

public class Appointment
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public string PatientName { get; set; } = "";
    public decimal Cost { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } = "Planned";
}

public abstract class MedicalEmployee : IComparable
{
    public int Id { get; private set; }
    public string FullName { get; private set; }
    public decimal BaseSalary { get; private set; }

    protected MedicalEmployee(int id, string fullName, decimal baseSalary)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("ФИО не может быть пустым.", nameof(fullName));
        if (baseSalary < 0)
            throw new ArgumentException("Оклад не может быть отрицательным.", nameof(baseSalary));

        Id = id;
        FullName = fullName.Trim();
        BaseSalary = baseSalary;
    }

    public virtual string GetInfo() => $"{FullName} (ID {Id}), оклад {BaseSalary:N0}";

    public abstract decimal CalculateMonthlyPayout(int appointmentsCount);

    public override string ToString() => $"[{Id}] {FullName}, оклад {BaseSalary:N0}";

    public override bool Equals(object? obj)
    {
        if (obj is MedicalEmployee other)
            return Id == other.Id;
        return false;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (obj is MedicalEmployee other)
            return other.BaseSalary.CompareTo(BaseSalary);
        throw new ArgumentException("Объект не является MedicalEmployee.", nameof(obj));
    }
}

public class Doctor : MedicalEmployee, IExportable, ICloneable
{
    public string Category { get; private set; }
    public int DepartmentId { get; private set; }
    public MedicalLicense License { get; set; }

    public Doctor(int id, string fullName, decimal baseSalary, string category, int departmentId, MedicalLicense license)
        : base(id, fullName, baseSalary)
    {
        Category = category;
        DepartmentId = departmentId;
        License = license;
    }

    public override string GetInfo() => base.GetInfo() + $", категория: {Category}";

    public override decimal CalculateMonthlyPayout(int appointmentsCount) =>
        BaseSalary + appointmentsCount * 450m;

    string IExportable.GetSummary() =>
        $"Врач: {FullName}\nКатегория: {Category}\nЛицензия: {License.Number} до {License.ExpiryDate:dd.MM.yyyy}\n" +
        $"Выплата за 20 приемов: {CalculateMonthlyPayout(20):N0}";

    public virtual object Clone()
    {
        var copy = (Doctor)MemberwiseClone();
        copy.License = new MedicalLicense(License.Number, License.ExpiryDate);
        return copy;
    }
}

public class Surgeon : Doctor
{
    private readonly int _complexOperations;

    public Surgeon(int id, string fullName, decimal baseSalary, string category, int departmentId,
        MedicalLicense license, int complexOperations)
        : base(id, fullName, baseSalary, category, departmentId, license)
    {
        _complexOperations = complexOperations;
    }

    public override string GetInfo() => base.GetInfo() + $", сложных операций: {_complexOperations}";

    public override decimal CalculateMonthlyPayout(int appointmentsCount)
    {
        decimal categoryBonus = Category == "Высшая" ? 20000m : 10000m;
        return BaseSalary + appointmentsCount * 1200m + categoryBonus;
    }
}
