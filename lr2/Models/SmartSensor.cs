namespace SmartHomeMasterConsole.Models;

public class SmartSensor : Device
{
    public double Accuracy { get; private set; }

    public SmartSensor(int id, string model, double power, int roomId, double accuracy, Calibration calibration)
        : base(id, model, power, roomId, calibration)
    {
        Accuracy = accuracy;
    }

    public override string DeviceType => "Sensor";

    public override string GetStatus() => $"{base.GetStatus()}, точность {Accuracy:P0}";

    public override double CalculatePowerConsumption() => Power * (_isActive ? 24.0 : 2.4) / 1000.0;

    public new string Describe() => $"Датчик #{Id} {Model}, точность {Accuracy:P0}";
}
