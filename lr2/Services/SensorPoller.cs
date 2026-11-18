using System.Collections.Concurrent;
using SmartHomeMasterConsole.Data;
using SmartHomeMasterConsole.Models;

namespace SmartHomeMasterConsole.Services;

public class SensorPoller
{
    private readonly Database _db;
    private readonly ConcurrentDictionary<int, string> _cache = new();

    public SensorPoller(Database db)
    {
        _db = db;
    }

    public async Task<string> PollOneAsync(SmartSensor sensor)
    {
        sensor.SetActive(true);
        await Task.Delay(Random.Shared.Next(200, 900));

        double? reading = await _db.GetLastReadingAsync(sensor.Id);
        string text = reading.HasValue
            ? $"{sensor.Model} (#{sensor.Id}): {reading.Value:F2}"
            : $"{sensor.Model} (#{sensor.Id}): нет данных";

        _cache[sensor.Id] = text;
        sensor.NotifyStatus($"Получено значение — {text}");
        return text;
    }

    public async Task<string[]> PollAllAsync(IEnumerable<SmartSensor> sensors)
    {
        var tasks = sensors.Select(s => PollOneAsync(s)).ToList();
        return await Task.WhenAll(tasks);
    }

    public async ValueTask<string> GetCachedReadingAsync(SmartSensor sensor)
    {
        if (_cache.TryGetValue(sensor.Id, out var cached))
        {
            return "[кэш] " + cached;
        }

        double? reading = await _db.GetLastReadingAsync(sensor.Id);
        string text = reading.HasValue
            ? $"{sensor.Model} (#{sensor.Id}): {reading.Value:F2}"
            : $"{sensor.Model} (#{sensor.Id}): нет данных";

        _cache[sensor.Id] = text;
        return "[БД] " + text;
    }
}
