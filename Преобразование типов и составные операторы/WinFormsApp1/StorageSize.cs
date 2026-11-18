public class StorageSize
{
    public long Bytes { get; set; }

    public static implicit operator StorageSize(long bytes) => new StorageSize { Bytes = bytes };

    public static explicit operator long(StorageSize size) => size.Bytes;

    public static implicit operator MegaByte(StorageSize size) =>
        new MegaByte { Value = size.Bytes / (double)MegaByte.BytesPerMegaByte };
}
