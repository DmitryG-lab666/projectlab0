using SmartHomeMasterConsole.Models;

namespace SmartHomeMasterConsole.Data;

public class DeviceRepository : IRepository<Device>
{
    private readonly List<Device> _items = new();

    public Device GetById(int id) =>
        _items.FirstOrDefault(d => d.Id == id) ?? throw new KeyNotFoundException($"Устройство {id} не найдено");

    public List<Device> GetAll() => new(_items);

    public void Add(Device entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _items.Add(entity);
    }
}
