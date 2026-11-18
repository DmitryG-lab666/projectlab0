public static class DataParser
{
    public static int ParseAndDivide(string[] data, int index, int divisor)
    {
        ArgumentNullException.ThrowIfNull(data);

        int value = int.Parse(data[index]);
        return value / divisor;
    }
}
