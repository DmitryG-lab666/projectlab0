public static class Helper
{
    public static void Swap<TValue>(ref TValue a, ref TValue b)
    {
        TValue temp = a;
        a = b;
        b = temp;
    }
}
