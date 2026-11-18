namespace VehicleApp;

public interface IStorable
{
    string FilePath { get; }

    void Save(string path);
}
