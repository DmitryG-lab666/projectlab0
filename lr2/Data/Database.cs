using Microsoft.Data.Sqlite;
using SmartHomeMasterConsole.Models;

namespace SmartHomeMasterConsole.Data;

public class Database
{
    private readonly string _connectionString;

    public Database(string? path = null)
    {
        string file = path ?? Path.Combine(AppContext.BaseDirectory, "smart_home.db");
        _connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = file,
            ForeignKeys = true
        }.ToString();
    }

    public void EnsureCreated()
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        using (var create = conn.CreateCommand())
        {
            create.CommandText = """
                CREATE TABLE IF NOT EXISTS Rooms (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL UNIQUE,
                    Floor INTEGER NOT NULL
                );
                CREATE TABLE IF NOT EXISTS Devices (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    RoomId INTEGER NOT NULL,
                    Model TEXT NOT NULL,
                    Power REAL NOT NULL,
                    Type TEXT NOT NULL,
                    FOREIGN KEY (RoomId) REFERENCES Rooms(Id)
                );
                CREATE TABLE IF NOT EXISTS TelemetryLogs (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    DeviceId INTEGER NOT NULL,
                    Reading REAL NOT NULL,
                    Timestamp TEXT NOT NULL,
                    FOREIGN KEY (DeviceId) REFERENCES Devices(Id)
                );
                """;
            create.ExecuteNonQuery();
        }

        using (var check = conn.CreateCommand())
        {
            check.CommandText = "SELECT COUNT(*) FROM Rooms";
            if (Convert.ToInt32(check.ExecuteScalar()) > 0)
            {
                return;
            }
        }

        var rooms = new (int Id, string Title, int Floor)[]
        {
            (1, "Гостиная", 1),
            (2, "Кухня", 1),
            (3, "Спальня", 2),
            (4, "Кабинет", 2),
            (5, "Гараж", 0)
        };

        var devices = new (int Id, int RoomId, string Model, double Power, string Type)[]
        {
            (1, 1, "TempSense T1", 2.5, "Sensor"),
            (2, 1, "MotionEye M2", 3.0, "Sensor"),
            (3, 1, "LightDim L1", 60, "Actuator"),
            (4, 1, "SmartPlug P3", 1800, "Actuator"),
            (5, 2, "TempSense T1", 2.5, "Sensor"),
            (6, 2, "AirQ A3", 4.5, "Sensor"),
            (7, 2, "SmartPlug P3", 2200, "Actuator"),
            (8, 3, "TempSense T1", 2.5, "Sensor"),
            (9, 3, "LightDim L1", 60, "Actuator"),
            (10, 3, "BlindMotor B4", 120, "Actuator"),
            (11, 4, "MotionEye M2", 3.0, "Sensor"),
            (12, 4, "AirQ A3", 4.5, "Sensor"),
            (13, 4, "LightDim L1", 60, "Actuator"),
            (14, 5, "MotionEye M2", 3.0, "Sensor"),
            (15, 5, "ThermoValve V2", 15, "Actuator"),
            (16, 5, "BlindMotor B4", 120, "Actuator")
        };

        using var tx = conn.BeginTransaction();

        foreach (var r in rooms)
        {
            Exec(conn, tx, "INSERT INTO Rooms (Id, Title, Floor) VALUES ($id, $t, $f)",
                ("$id", r.Id), ("$t", r.Title), ("$f", r.Floor));
        }

        foreach (var d in devices)
        {
            Exec(conn, tx, "INSERT INTO Devices (Id, RoomId, Model, Power, Type) VALUES ($id, $r, $m, $p, $t)",
                ("$id", d.Id), ("$r", d.RoomId), ("$m", d.Model), ("$p", d.Power), ("$t", d.Type));
        }

        int logId = 1;
        foreach (var d in devices.Where(x => x.Type == "Sensor"))
        {
            for (int k = 0; k < 3; k++)
            {
                Exec(conn, tx, "INSERT INTO TelemetryLogs (Id, DeviceId, Reading, Timestamp) VALUES ($id, $d, $r, $t)",
                    ("$id", logId++),
                    ("$d", d.Id),
                    ("$r", Math.Round(18 + d.Id * 0.7 + k * 0.4, 2)),
                    ("$t", DateTime.Now.AddHours(-6 * (2 - k)).ToString("s")));
            }
        }

        tx.Commit();
    }

    private static void Exec(SqliteConnection conn, SqliteTransaction tx, string sql, params (string Name, object Value)[] args)
    {
        using var cmd = conn.CreateCommand();
        cmd.Transaction = tx;
        cmd.CommandText = sql;
        foreach (var (name, value) in args)
        {
            cmd.Parameters.AddWithValue(name, value);
        }

        cmd.ExecuteNonQuery();
    }

    public async Task<List<Room>> LoadRoomsAsync()
    {
        var list = new List<Room>();
        await using var conn = new SqliteConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Title, Floor FROM Rooms ORDER BY Id";
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new Room(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));
        }

        return list;
    }

    public async Task<List<Device>> LoadDevicesAsync()
    {
        var list = new List<Device>();
        await using var conn = new SqliteConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, RoomId, Model, Power, Type FROM Devices ORDER BY Id";
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(DeviceFactory.Create(
                reader.GetInt32(0),
                reader.GetInt32(1),
                reader.GetString(2),
                reader.GetDouble(3),
                reader.GetString(4)));
        }

        return list;
    }

    public async Task<List<TelemetryLog>> LoadTelemetryAsync()
    {
        var list = new List<TelemetryLog>();
        await using var conn = new SqliteConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, DeviceId, Reading, Timestamp FROM TelemetryLogs ORDER BY Id";
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new TelemetryLog(reader.GetInt32(0), reader.GetInt32(1), reader.GetDouble(2), reader.GetString(3)));
        }

        return list;
    }

    public async Task<int> InsertDeviceAsync(int roomId, string model, double power, string type)
    {
        await using var conn = new SqliteConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO Devices (RoomId, Model, Power, Type) VALUES ($r, $m, $p, $t) RETURNING Id";
        cmd.Parameters.AddWithValue("$r", roomId);
        cmd.Parameters.AddWithValue("$m", model);
        cmd.Parameters.AddWithValue("$p", power);
        cmd.Parameters.AddWithValue("$t", type);
        object? result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<double?> GetLastReadingAsync(int deviceId)
    {
        await using var conn = new SqliteConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Reading FROM TelemetryLogs WHERE DeviceId = $d ORDER BY Timestamp DESC, Id DESC LIMIT 1";
        cmd.Parameters.AddWithValue("$d", deviceId);
        object? result = await cmd.ExecuteScalarAsync();
        if (result is null || result is DBNull)
        {
            return null;
        }

        return Convert.ToDouble(result);
    }
}
