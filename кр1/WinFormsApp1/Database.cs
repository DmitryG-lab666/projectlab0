using Microsoft.Data.Sqlite;

namespace CorpFleet;

public record VehicleRow(int Id, string Model, double BasePrice, int Power, int CategoryId);
public record LogRow(int Id, int VehicleId, string ServiceName, double Cost, string LogDate, string Channel);

public class Database
{
    private readonly string _cs;

    public Database(string? path = null)
    {
        path ??= System.IO.Path.Combine(AppContext.BaseDirectory, "corp_fleet.db");
        _cs = $"Data Source={path}";
        EnsureCreated();
    }

    private SqliteConnection Open()
    {
        var c = new SqliteConnection(_cs);
        c.Open();
        return c;
    }

    private static void Exec(SqliteConnection c, string sql)
    {
        using var cmd = c.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }

    private void EnsureCreated()
    {
        using var c = Open();
        Exec(c, @"
CREATE TABLE IF NOT EXISTS Categories(Id INTEGER PRIMARY KEY, CategoryName TEXT NOT NULL, TaxRate REAL NOT NULL);
CREATE TABLE IF NOT EXISTS Vehicles(Id INTEGER PRIMARY KEY, Model TEXT NOT NULL, BasePrice REAL NOT NULL, Power INT NOT NULL,
    CategoryId INTEGER NOT NULL REFERENCES Categories(Id));
CREATE TABLE IF NOT EXISTS MaintenanceLogs(Id INTEGER PRIMARY KEY, VehicleId INTEGER NOT NULL REFERENCES Vehicles(Id),
    ServiceName TEXT NOT NULL, Cost REAL NOT NULL, LogDate TEXT NOT NULL, Channel TEXT NOT NULL);");

        using var check = c.CreateCommand();
        check.CommandText = "SELECT COUNT(*) FROM Categories";
        if (Convert.ToInt32(check.ExecuteScalar()) > 0) return;

        Exec(c, @"
INSERT INTO Categories(Id,CategoryName,TaxRate) VALUES (1,'Легковые',0.05),(2,'Грузовые',0.12),(3,'Автобусы',0.08);
INSERT INTO Vehicles(Id,Model,BasePrice,Power,CategoryId) VALUES
 (1,'Toyota Camry',3200000,181,1),(2,'Skoda Octavia',2700000,150,1),(3,'BMW 5 Series',5900000,249,1),
 (4,'KAMAZ 5490',7800000,400,2),(5,'MAN TGX',9400000,460,2),(6,'GAZelle Next',2400000,149,2),
 (7,'Yutong ZK6122',8600000,300,3),(8,'Hyundai Solaris',1700000,123,1);
INSERT INTO MaintenanceLogs(VehicleId,ServiceName,Cost,LogDate,Channel) VALUES
 (1,'Замена масла',25000,'2026-01-10','Online'),(1,'Диагностика',15000,'2026-03-02','Offline'),
 (2,'Замена масла',22000,'2026-01-15','Offline'),(3,'Замена тормозов',90000,'2026-02-20','Offline'),
 (3,'Диагностика',18000,'2026-04-11','Online'),(4,'Ремонт КПП',350000,'2026-02-05','Offline'),
 (4,'Замена масла',60000,'2026-05-01','Online'),(5,'Шиномонтаж',120000,'2026-03-18','Offline'),
 (5,'Замена фильтров',45000,'2026-06-07','Online'),(6,'Замена масла',20000,'2026-04-22','Online'),
 (7,'Ремонт подвески',210000,'2026-05-30','Offline'),(7,'Диагностика',30000,'2026-06-15','Online'),
 (8,'Шиномонтаж',12000,'2026-02-28','Offline'),(8,'Замена масла',19000,'2026-05-12','Online');");
    }

    public List<Category> GetCategories()
    {
        var list = new List<Category>();
        using var c = Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "SELECT Id, CategoryName, TaxRate FROM Categories";
        using var r = cmd.ExecuteReader();
        while (r.Read())
            list.Add(new Category { Id = r.GetInt32(0), CategoryName = r.GetString(1), TaxRate = r.GetDouble(2) });
        return list;
    }

    public List<VehicleRow> GetVehicles()
    {
        var list = new List<VehicleRow>();
        using var c = Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "SELECT Id, Model, BasePrice, Power, CategoryId FROM Vehicles";
        using var r = cmd.ExecuteReader();
        while (r.Read())
            list.Add(new VehicleRow(r.GetInt32(0), r.GetString(1), r.GetDouble(2), r.GetInt32(3), r.GetInt32(4)));
        return list;
    }

    public List<LogRow> GetLogs()
    {
        var list = new List<LogRow>();
        using var c = Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "SELECT Id, VehicleId, ServiceName, Cost, LogDate, Channel FROM MaintenanceLogs";
        using var r = cmd.ExecuteReader();
        while (r.Read())
            list.Add(new LogRow(r.GetInt32(0), r.GetInt32(1), r.GetString(2), r.GetDouble(3), r.GetString(4), r.GetString(5)));
        return list;
    }

    public int InsertVehicle(string model, double price, int power, int categoryId)
    {
        using var c = Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "INSERT INTO Vehicles(Model,BasePrice,Power,CategoryId) VALUES($m,$p,$w,$c); SELECT last_insert_rowid();";
        cmd.Parameters.AddWithValue("$m", model);
        cmd.Parameters.AddWithValue("$p", price);
        cmd.Parameters.AddWithValue("$w", power);
        cmd.Parameters.AddWithValue("$c", categoryId);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }
}
