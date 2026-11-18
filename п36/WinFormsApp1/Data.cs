using Microsoft.Data.Sqlite;

namespace ClinicCrm;

public class SqliteRepository<T> : IRepository<T> where T : class
{
    private readonly string _connectionString;
    private readonly string _table;
    private readonly string _insertSql;
    private readonly Func<SqliteDataReader, T> _map;
    private readonly Action<SqliteCommand, T> _bind;

    public SqliteRepository(string connectionString, string table, string insertSql,
        Func<SqliteDataReader, T> map, Action<SqliteCommand, T> bind)
    {
        _connectionString = connectionString;
        _table = table;
        _insertSql = insertSql;
        _map = map;
        _bind = bind;
    }

    private SqliteConnection Open()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }

    public List<T> GetAll()
    {
        var list = new List<T>();
        using var connection = Open();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"SELECT * FROM {_table}";
        using var reader = cmd.ExecuteReader();
        while (reader.Read()) list.Add(_map(reader));
        return list;
    }

    public T? GetById(int id)
    {
        using var connection = Open();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"SELECT * FROM {_table} WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        using var reader = cmd.ExecuteReader();
        return reader.Read() ? _map(reader) : null;
    }

    public void Add(T item)
    {
        using var connection = Open();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = _insertSql;
        _bind(cmd, item);
        cmd.ExecuteNonQuery();
    }
}

public static class DbInit
{
    public static void Ensure(string connectionString)
    {
        SQLitePCL.Batteries.Init();
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        Exec(connection, @"
CREATE TABLE IF NOT EXISTS Departments(
    Id INTEGER PRIMARY KEY AUTOINCREMENT, Title TEXT NOT NULL, BaseRate REAL NOT NULL);
CREATE TABLE IF NOT EXISTS Doctors(
    Id INTEGER PRIMARY KEY AUTOINCREMENT, FullName TEXT NOT NULL,
    DepartmentId INTEGER REFERENCES Departments(Id), Category TEXT NOT NULL, BaseSalary REAL NOT NULL);
CREATE TABLE IF NOT EXISTS Appointments(
    Id INTEGER PRIMARY KEY AUTOINCREMENT, DoctorId INTEGER REFERENCES Doctors(Id),
    PatientName TEXT NOT NULL, Cost REAL NOT NULL, Date TEXT NOT NULL, Status TEXT NOT NULL);");

        using var check = connection.CreateCommand();
        check.CommandText = "SELECT COUNT(*) FROM Departments";
        if (Convert.ToInt32(check.ExecuteScalar()) > 0) return;

        Exec(connection, @"
INSERT INTO Departments(Title,BaseRate) VALUES ('Терапия',1500),('Хирургия',3000),('Кардиология',2200);
INSERT INTO Doctors(FullName,DepartmentId,Category,BaseSalary) VALUES
 ('Иванов Иван Иванович',1,'Высшая',80000),
 ('Петрова Анна Сергеевна',1,'Первая',65000),
 ('Сидоров Пётр Павлович',2,'Высшая',120000),
 ('Каримов Нурлан Бекович',2,'Первая',105000),
 ('Ли Елена Викторовна',3,'Высшая',95000);
INSERT INTO Appointments(DoctorId,PatientName,Cost,Date,Status) VALUES
 (1,'Смирнов А.',3000,'2026-10-01','Completed'),
 (1,'Кузнецова М.',3500,'2026-10-02','Completed'),
 (1,'Орлов Д.',3000,'2026-10-05','Completed'),
 (1,'Попов В.',3200,'2026-11-20','Planned'),
 (2,'Смирнов А.',2800,'2026-10-07','Completed'),
 (2,'Белова О.',2800,'2026-10-09','Cancelled'),
 (2,'Захаров К.',3100,'2026-10-12','Completed'),
 (3,'Смирнов А.',15000,'2026-10-14','Completed'),
 (3,'Кузнецова М.',22000,'2026-10-15','Completed'),
 (3,'Волков Р.',18000,'2026-11-25','Planned'),
 (4,'Орлов Д.',20000,'2026-10-18','Completed'),
 (4,'Морозов Т.',12000,'2026-10-20','Cancelled'),
 (5,'Белова О.',6500,'2026-10-22','Completed'),
 (5,'Попов В.',7000,'2026-10-25','Completed'),
 (5,'Новиков С.',6800,'2026-11-28','Planned'),
 (3,'Захаров К.',17000,'2026-10-28','Completed');");
    }

    private static void Exec(SqliteConnection connection, string sql)
    {
        using var cmd = connection.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }
}
