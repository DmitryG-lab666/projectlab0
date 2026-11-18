using SmartHomeMasterConsole.Models;

namespace SmartHomeMasterConsole.Services;

public static class DelegateTools
{
    public static readonly Predicate<Device> IsCritical =
        d => d.Power > 1500 || (d is SmartSensor s && s.Accuracy < 0.92);

    public static readonly Func<Device, double> DailyConsumption =
        d => d.CalculatePowerConsumption();

    public static Func<double, bool> CreateThresholdChecker(double limit)
    {
        double threshold = limit;
        return value => value > threshold;
    }

    public static List<Device> FindCritical(IEnumerable<Device> devices) =>
        devices.ToList().FindAll(IsCritical);

    public static void ForEachDevice(IEnumerable<Device> devices, Action<Device> action)
    {
        foreach (var device in devices)
        {
            action(device);
        }
    }
}
