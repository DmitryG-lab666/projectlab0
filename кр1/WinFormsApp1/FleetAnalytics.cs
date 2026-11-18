namespace CorpFleet;

public class FleetReportDto
{
    public int Id { get; set; }
    public string Model { get; set; } = "";
    public string CategoryName { get; set; } = "";
    public double BasePrice { get; set; }
    public int Power { get; set; }
    public double Tax { get; set; }
    public double ServiceCost { get; set; }
    public int LogsCount { get; set; }
    public double TotalCost { get; set; }
}

public static class FleetAnalytics
{
    public static List<FleetReportDto> BuildReport(
        List<Category> categories, List<VehicleRow> vehicles, List<LogRow> logs) =>
        vehicles
            .Join(categories, v => v.CategoryId, c => c.Id, (v, c) => new { v, c })
            .GroupJoin(logs, x => x.v.Id, l => l.VehicleId, (x, ls) =>
            {
                double tax = x.v.BasePrice * x.c.TaxRate;
                double service = ls.Sum(l => l.Cost);
                return new FleetReportDto
                {
                    Id = x.v.Id,
                    Model = x.v.Model,
                    CategoryName = x.c.CategoryName,
                    BasePrice = x.v.BasePrice,
                    Power = x.v.Power,
                    Tax = tax,
                    ServiceCost = service,
                    LogsCount = ls.Count(),
                    TotalCost = tax + service
                };
            })
            .ToList();

    public static List<FleetReportDto> Filter(
        IEnumerable<FleetReportDto> report, double minPrice, double maxPrice, string? text) =>
        report.Where(d => d.BasePrice >= minPrice && d.BasePrice <= maxPrice &&
                          (string.IsNullOrWhiteSpace(text) ||
                           d.Model.Contains(text, StringComparison.OrdinalIgnoreCase) ||
                           d.CategoryName.Contains(text, StringComparison.OrdinalIgnoreCase)))
              .ToList();

    public static List<FleetReportDto> Sort(IEnumerable<FleetReportDto> report) =>
        report.OrderBy(v => v.CategoryName).ThenByDescending(v => v.BasePrice).ToList();

    public static List<FleetReportDto> Page(List<FleetReportDto> sorted, int page, int pageSize) =>
        sorted.Skip((page - 1) * pageSize).Take(pageSize).ToList();

    public static int PageCount(int total, int pageSize) =>
        Math.Max(1, (int)Math.Ceiling(total / (double)pageSize));

    public static string Summary(List<FleetReportDto> report)
    {
        var groups = report
            .GroupBy(d => d.CategoryName)
            .Select(g => new
            {
                Name = g.Key,
                Count = g.Count(),
                Sum = g.Sum(d => d.BasePrice),
                Avg = g.Average(d => d.BasePrice),
                Min = g.Min(d => d.BasePrice),
                Max = g.Max(d => d.BasePrice)
            })
            .OrderBy(g => g.Name)
            .ToList();

        string text = groups.Aggregate(
            "=== Сводка CorpFleet Analytics ===",
            (acc, g) => acc + Environment.NewLine +
                $"{g.Name}: шт={g.Count}, сумма={g.Sum:N0}, средняя={g.Avg:N0}, мин={g.Min:N0}, макс={g.Max:N0}");

        return text + Environment.NewLine +
            $"ИТОГО: ТС={report.Count}, стоимость парка={report.Sum(d => d.BasePrice):N0}, " +
            $"средний налог={(report.Count == 0 ? 0 : report.Average(d => d.Tax)):N0}, " +
            $"обслуживание={report.Sum(d => d.ServiceCost):N0}";
    }

    private static IEnumerable<string> Names(List<LogRow> logs, string channel) =>
        logs.Where(l => l.Channel == channel).Select(l => l.ServiceName);

    public static List<string> OnlineExceptOffline(List<LogRow> logs) =>
        Names(logs, "Online").Except(Names(logs, "Offline")).ToList();

    public static List<string> Common(List<LogRow> logs) =>
        Names(logs, "Online").Intersect(Names(logs, "Offline")).ToList();

    public static List<string> All(List<LogRow> logs) =>
        Names(logs, "Online").Union(Names(logs, "Offline")).ToList();

    public static string Checks(List<LogRow> logs, List<FleetReportDto> report)
    {
        bool anyBig = logs.Any(l => l.Cost > 300_000);
        bool allPositive = logs.All(l => l.Cost > 0);
        var top = report.OrderByDescending(d => d.ServiceCost).FirstOrDefault();
        var noLogs = report.FirstOrDefault(d => d.LogsCount == 0);

        return $"Any(обслуживание > 300 000): {anyBig}{Environment.NewLine}" +
               $"All(стоимость > 0): {allPositive}{Environment.NewLine}" +
               $"Больше всего на обслуживание: {(top == null ? "—" : $"{top.Model} ({top.ServiceCost:N0})")}{Environment.NewLine}" +
               $"ТС без обслуживания: {(noLogs == null ? "нет" : noLogs.Model)}";
    }
}
