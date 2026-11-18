namespace SmartHomeMasterConsole.Models;

public abstract class Device : IExportable, IComparable<Device>, ICloneable
{
    protected bool _isActive;

    public int Id { get; private set; }
    public string Model { get; private set; }
    public double Power { get; private set; }
    public int RoomId { get; private set; }
    public Calibration Calibration { get; private set; }

    public abstract string DeviceType { get; }
    public bool IsActive => _isActive;

    public event EventHandler<DeviceEventArgs>? StatusChanged;

    protected Device(int id, string model, double power, int roomId, Calibration calibration)
    {
        Id = id;
        Model = model;
        Power = power;
        RoomId = roomId;
        Calibration = calibration;
    }

    public virtual string GetStatus() => _isActive ? "Активно" : "Выключено";

    public abstract double CalculatePowerConsumption();

    public string Describe() => $"Устройство #{Id} {Model}";

    public void SetActive(bool value)
    {
        if (_isActive == value)
        {
            return;
        }

        _isActive = value;
        NotifyStatus($"{Model} (#{Id}): {GetStatus()}");
    }

    public void NotifyStatus(string message)
    {
        StatusChanged?.Invoke(this, new DeviceEventArgs(message));
    }

    public override string ToString() => $"{DeviceType} #{Id} {Model}, {Power:F1} Вт, комната {RoomId}";

    public override bool Equals(object? obj) => obj is Device other && Id == other.Id && Model == other.Model;

    public override int GetHashCode() => HashCode.Combine(Id, Model);

    string IExportable.GetExportData() => $"{Id};{Model};{Power:F1};{DeviceType};{RoomId}";

    public int CompareTo(Device? other) => other is null ? 1 : Power.CompareTo(other.Power);

    public object Clone()
    {
        var copy = (Device)MemberwiseClone();
        copy.Calibration = Calibration.Clone();
        copy.StatusChanged = null;
        return copy;
    }
}
