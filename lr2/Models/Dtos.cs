namespace SmartHomeMasterConsole.Models;

public record Room(int Id, string Title, int Floor);

public record TelemetryLog(int Id, int DeviceId, double Reading, string Timestamp);

public record DeviceReportDto(int DeviceId, string Model, string Type, double Power, string RoomTitle, int Floor);

public record RoomSummaryDto(string Room, int Floor, int DeviceCount);

public record GroupStatDto(string Room, int Count, double TotalPower, double AveragePower, string Models);

public record DeviceRow(int Id, string Model, string Type, double Power, int RoomId, string Status, double DailyKWh, string Passport);
