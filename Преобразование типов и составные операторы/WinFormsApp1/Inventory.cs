public class Inventory
{
    static int globalCount;

    public int Id { get; }
    public int TotalUnits { get; set; }

    public Inventory() => Id = ++globalCount;

    public virtual void operator +=(int units) => TotalUnits += units;

    public void operator +=(Inventory other) => TotalUnits += other.TotalUnits;

    public void operator ++() => TotalUnits++;

    public static Inventory operator ++(Inventory inv) =>
        new Inventory { TotalUnits = inv.TotalUnits + 1 };
}
