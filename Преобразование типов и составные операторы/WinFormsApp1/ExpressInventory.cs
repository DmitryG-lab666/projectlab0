public class ExpressInventory : Inventory
{
    public override void operator +=(int units) => TotalUnits += units * 2;
}
