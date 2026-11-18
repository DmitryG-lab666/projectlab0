namespace VehicleApp;

public class Car : Vehicle
{
    public bool IsElectric { get; private set; }

    public Car(string model, int power, bool isElectric) : base(model, power)
    {
        IsElectric = isElectric;
    }

    public override string GetInfo() =>
        $"Легковой: {base.GetInfo()}, {(IsElectric ? "электро" : "ДВС")}";

    public override double CalculateTax() => Power * (IsElectric ? 5 : 12);
}
