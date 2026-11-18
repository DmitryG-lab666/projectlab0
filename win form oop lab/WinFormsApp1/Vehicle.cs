using System;

namespace VehicleApp;

public abstract class Vehicle
{
    public string Model { get; private set; }
    public int Power { get; private set; }

    protected Vehicle(string model, int power)
    {
        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Модель не может быть пустой.", nameof(model));
        if (power <= 0)
            throw new ArgumentOutOfRangeException(nameof(power), "Мощность должна быть положительной.");

        Model = model;
        Power = power;
    }

    public virtual string GetInfo() => $"{Model}, {Power} л.с.";

    public abstract double CalculateTax();
}
