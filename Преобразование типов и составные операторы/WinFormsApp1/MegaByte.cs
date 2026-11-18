public class MegaByte
{
    public const long BytesPerMegaByte = 1_048_576;

    public double Value { get; set; }

    public static explicit operator StorageSize(MegaByte mb) =>
        new StorageSize { Bytes = (long)(mb.Value * BytesPerMegaByte) };
}
