namespace SmartHomeMasterConsole.Models;

public class SmartActuator : Device
{
    public double LoadCapacity { get; private set; }

    public SmartActuator(int id, string model, double power, int roomId, double loadCapacity, Calibration calibration)
        : base(id, model, power, roomId, calibration)
    {
        LoadCapacity = loadCapacity;
    }

    public override string DeviceType => "Actuator";

    public override string GetStatus() => $"{base.GetStatus()}, нагрузка до {LoadCapacity:F1} Вт";

    public override double CalculatePowerConsumption() => Power * (_isActive ? 24.0 : 0.5) / 1000.0;

    public new string Describe() => $"Исполнитель #{Id} {Model}, нагрузка до {LoadCapacity:F1} Вт";
}
