using SmartHomeMasterConsole.Models;

namespace SmartHomeMasterConsole.Services;

public static class LinqAnalytics
{
    public static List<DeviceReportDto> BuildReport(IEnumerable<Device> devices, IEnumerable<Room> rooms) =>
        devices
            .Join(rooms, d => d.RoomId, r => r.Id,
                (d, r) => new DeviceReportDto(d.Id, d.Model, d.DeviceType, d.Power, r.Title, r.Floor))
            .ToList();

    public static List<RoomSummaryDto> RoomSummaries(IEnumerable<Device> devices, IEnumerable<Room> rooms) =>
        rooms
            .GroupJoin(devices, r => r.Id, d => d.RoomId,
                (r, ds) => new RoomSummaryDto(r.Title, r.Floor, ds.Count()))
            .OrderBy(x => x.Floor)
            .ToList();

    public static List<DeviceReportDto> FilterSorted(IEnumerable<DeviceReportDto> report, double minPower, string? room) =>
        report
            .Where(d => d.Power >= minPower && (room == null || d.RoomTitle == room))
            .OrderBy(d => d.Floor)
            .ThenByDescending(d => d.Power)
            .ToList();

    public static List<DeviceReportDto> Page(IEnumerable<DeviceReportDto> sorted, int page, int pageSize) =>
        sorted.Skip((page - 1) * pageSize).Take(pageSize).ToList();

    public static List<GroupStatDto> GroupStats(IEnumerable<DeviceReportDto> report) =>
        report
            .GroupBy(d => d.RoomTitle)
            .Select(g => new GroupStatDto(
                g.Key,
                g.Count(),
                Math.Round(g.Sum(x => x.Power), 1),
                Math.Round(g.Average(x => x.Power), 1),
                g.Select(x => x.Model).Aggregate((a, b) => a + ", " + b)))
            .OrderBy(s => s.Room)
            .ToList();

    public static string CompareRooms(IEnumerable<DeviceReportDto> report, string roomA, string roomB)
    {
        var data = report.ToList();
        var a = data.Where(d => d.RoomTitle == roomA).Select(d => d.Model).Distinct().ToList();
        var b = data.Where(d => d.RoomTitle == roomB).Select(d => d.Model).Distinct().ToList();

        var common = a.Intersect(b).ToList();
        var onlyA = a.Except(b).ToList();
        var onlyB = b.Except(a).ToList();
        var all = a.Union(b).ToList();

        return string.Join(Environment.NewLine, new[]
        {
            $"Комната A: {roomA} — {string.Join(", ", a)}",
            $"Комната B: {roomB} — {string.Join(", ", b)}",
            $"Intersect (общие): {string.Join(", ", common)}",
            $"Except (только в A): {string.Join(", ", onlyA)}",
            $"Except (только в B): {string.Join(", ", onlyB)}",
            $"Union (все модели): {string.Join(", ", all)}"
        });
    }

    public static string SafetyChecks(IEnumerable<DeviceReportDto> report, double limit)
    {
        var data = report.ToList();
        bool anyOver = data.Any(d => d.Power > limit);
        bool allPositive = data.All(d => d.Power > 0);
        var first = data.FirstOrDefault(d => d.Power > limit);

        return string.Join(Environment.NewLine, new[]
        {
            $"Any(Power > {limit:F0}): {anyOver}",
            $"All(Power > 0): {allPositive}",
            $"FirstOrDefault(Power > {limit:F0}): {(first is null ? "null" : $"{first.Model} в комнате {first.RoomTitle}")}"
        });
    }

    public static string DeferredDemo(IEnumerable<Device> devices)
    {
        var source = devices.ToList();
        IEnumerable<Device> deferred = source.Where(d => d.Power > 100);
        List<Device> materialized = source.Where(d => d.Power > 100).ToList();

        int deferredBefore = deferred.Count();
        int materializedBefore = materialized.Count;

        source.Add(DeviceFactory.Create(9999, 1, "Heater X", 2000, "Actuator"));

        int deferredAfter = deferred.Count();
        int materializedAfter = materialized.Count;

        return string.Join(Environment.NewLine, new[]
        {
            $"IEnumerable (отложенный): было {deferredBefore}, после добавления в источник стало {deferredAfter}",
            $"ToList() (материализация): было {materializedBefore}, после добавления осталось {materializedAfter}"
        });
    }
}
