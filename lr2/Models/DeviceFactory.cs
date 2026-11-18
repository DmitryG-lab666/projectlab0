namespace SmartHomeMasterConsole.Models;

public static class DeviceFactory
{
    public static Device Create(int id, int roomId, string model, double power, string type)
    {
        var calibration = new Calibration(
            $"CAL-{id:D4}",
            DateTime.Today.AddDays(-(id % 30)),
            Math.Round((id % 7) * 0.05, 2));

        if (type == "Sensor")
        {
            return new SmartSensor(id, model, power, roomId, Math.Round(0.90 + (id % 10) / 100.0, 2), calibration);
        }

        return new SmartActuator(id, model, power, roomId, Math.Round(power * 1.5, 1), calibration);
    }
}
