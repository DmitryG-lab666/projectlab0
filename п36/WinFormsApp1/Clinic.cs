using System.ComponentModel;

namespace ClinicCrm;

public class AppointmentEventArgs : EventArgs
{
    public string DoctorName { get; }
    public string PatientName { get; }
    public decimal Cost { get; }
    public string Message { get; }

    public AppointmentEventArgs(string doctorName, string patientName, decimal cost, string message)
    {
        DoctorName = doctorName;
        PatientName = patientName;
        Cost = cost;
        Message = message;
    }
}

public class ClinicManager
{
    public event EventHandler<AppointmentEventArgs>? AppointmentRegistered;

    private readonly IRepository<Appointment> _appointments;

    public Action<string> Log { get; set; } = _ => { };

    public Predicate<Appointment> IsCompleted { get; } = a => a.Status == "Completed";

    public Func<Appointment, decimal> DiscountedCost { get; } = a => a.Cost * 0.9m;

    public ClinicManager(IRepository<Appointment> appointments)
    {
        _appointments = appointments;
    }

    public Func<Appointment, bool> CreateBudgetFilter(decimal minCost) =>
        a => a.Cost >= minCost;

    public void Register(Doctor doctor, string patient, decimal cost, DateTime date)
    {
        var appointment = new Appointment
        {
            DoctorId = doctor.Id,
            PatientName = patient,
            Cost = cost,
            Date = date,
            Status = "Planned"
        };
        _appointments.Add(appointment);
        Log($"Запись сохранена: {patient} → {doctor.FullName}");
        AppointmentRegistered?.Invoke(this, new AppointmentEventArgs(
            doctor.FullName, patient, cost, $"Зарегистрирован приём на {date:dd.MM.yyyy}"));
    }
}

public class AppointmentViewDto
{
    [DisplayName("Пациент")] public string Patient { get; set; } = "";
    [DisplayName("Врач")] public string DoctorName { get; set; } = "";
    [DisplayName("Отделение")] public string Department { get; set; } = "";
    [DisplayName("Стоимость")] public decimal Cost { get; set; }
    [DisplayName("Со скидкой 10%")] public decimal Discounted { get; set; }
    [DisplayName("Дата")] public string Date { get; set; } = "";
    [DisplayName("Статус")] public string Status { get; set; } = "";
}

public class DoctorStatDto
{
    [DisplayName("Врач")] public string DoctorName { get; set; } = "";
    [DisplayName("Приемов")] public int Count { get; set; }
    [DisplayName("Суммарный доход")] public decimal Revenue { get; set; }
    [DisplayName("Средний чек")] public decimal Average { get; set; }
}

public class PatientDto
{
    [DisplayName("Пациент")] public string Name { get; set; } = "";
}

public static class ClinicAnalytics
{
    public static List<AppointmentViewDto> BuildDto(
        List<Appointment> appointments, List<Doctor> doctors, List<Department> departments,
        Func<Appointment, decimal> discount) =>
        appointments
            .Join(doctors, a => a.DoctorId, d => d.Id, (a, d) => new { a, d })
            .Join(departments, x => x.d.DepartmentId, dep => dep.Id, (x, dep) => new AppointmentViewDto
            {
                Patient = x.a.PatientName,
                DoctorName = x.d.FullName,
                Department = dep.Title,
                Cost = x.a.Cost,
                Discounted = discount(x.a),
                Date = x.a.Date.ToString("dd.MM.yyyy"),
                Status = x.a.Status
            })
            .ToList();

    public static List<AppointmentViewDto> Sort(IEnumerable<AppointmentViewDto> dtos) =>
        dtos.OrderBy(d => d.Department).ThenByDescending(d => d.Cost).ToList();

    public static List<AppointmentViewDto> CompletedSorted(IEnumerable<AppointmentViewDto> dtos) =>
        Sort(dtos.Where(d => d.Status == "Completed"));

    public static List<AppointmentViewDto> Page(List<AppointmentViewDto> sorted, int page, int pageSize) =>
        sorted.Skip((page - 1) * pageSize).Take(pageSize).ToList();

    public static int PageCount(int total, int pageSize) =>
        Math.Max(1, (int)Math.Ceiling(total / (double)pageSize));

    public static List<DoctorStatDto> RevenueByDoctor(List<Appointment> appointments, List<Doctor> doctors) =>
        appointments
            .GroupBy(a => a.DoctorId)
            .Select(g => new DoctorStatDto
            {
                DoctorName = doctors.FirstOrDefault(d => d.Id == g.Key)?.FullName ?? $"ID {g.Key}",
                Count = g.Count(),
                Revenue = g.Sum(a => a.Cost),
                Average = g.Average(a => a.Cost)
            })
            .OrderByDescending(s => s.Revenue)
            .ToList();

    private static IEnumerable<string> PatientsOf(List<AppointmentViewDto> dtos, string department) =>
        dtos.Where(d => d.Department == department).Select(d => d.Patient).Distinct();

    public static List<PatientDto> TherapyAndSurgery(List<AppointmentViewDto> dtos) =>
        PatientsOf(dtos, "Терапия").Intersect(PatientsOf(dtos, "Хирургия"))
            .Select(n => new PatientDto { Name = n }).ToList();

    public static List<PatientDto> TherapyOnly(List<AppointmentViewDto> dtos) =>
        PatientsOf(dtos, "Терапия").Except(PatientsOf(dtos, "Хирургия"))
            .Select(n => new PatientDto { Name = n }).ToList();
}
