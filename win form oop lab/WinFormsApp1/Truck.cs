using System;
using System.IO;

namespace VehicleApp;

public class Truck : Vehicle, IStorable
{
    public double LoadCapacity { get; private set; }
    public string FilePath { get; private set; }

    public Truck(string model, int power, double loadCapacity) : base(model, power)
    {
        if (loadCapacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(loadCapacity), "Грузоподъёмность должна быть положительной.");

        LoadCapacity = loadCapacity;
    }

    public override string GetInfo() =>
        $"Грузовой: {base.GetInfo()}, {LoadCapacity} т";

    public override double CalculateTax() => Power * 25;

    public void Save(string path)
    {
        string[] lines =
        {
            "Тип: Грузовой",
            $"Модель: {Model}",
            $"Мощность: {Power} л.с.",
            $"Грузоподъёмность: {LoadCapacity} т",
            $"Налог: {CalculateTax()} руб."
        };

        File.WriteAllLines(path, lines);
        FilePath = path;
    }
}
