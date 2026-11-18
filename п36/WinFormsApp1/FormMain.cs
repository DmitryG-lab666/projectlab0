using Microsoft.Data.Sqlite;

namespace ClinicCrm;

public class FormMain : Form
{
    private const int PageSize = 5;

    private readonly IRepository<Department> _departmentRepo;
    private readonly IRepository<Doctor> _doctorRepo;
    private readonly IRepository<Appointment> _appointmentRepo;
    private readonly ClinicManager _clinic;

    private List<Department> _departments = new();
    private List<Doctor> _doctors = new();
    private List<MedicalEmployee> _employees = new();
    private List<Appointment> _appointments = new();
    private int _page = 1;

    private readonly DataGridView gridDoctors = NewGrid();
    private readonly TextBox txtDoctorName = new() { Width = 200, PlaceholderText = "ФИО врача" };
    private readonly ComboBox cmbCategory = new() { Width = 90, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox cmbDepartment = new() { Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly NumericUpDown numSalary = new() { Width = 100, Maximum = 10_000_000, Value = 70000, ThousandsSeparator = true };

    private readonly ComboBox cmbDoctorPick = new() { Width = 260, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TextBox txtPatient = new() { Width = 180, PlaceholderText = "Пациент" };
    private readonly NumericUpDown numCost = new() { Width = 100, Maximum = 10_000_000, Value = 3000, ThousandsSeparator = true };
    private readonly DateTimePicker dtpDate = new() { Width = 110, Format = DateTimePickerFormat.Short };
    private readonly ListBox lstLog = new() { Dock = DockStyle.Fill };

    private readonly DataGridView gridDto = NewGrid();
    private readonly ComboBox cmbDeptFilter = new() { Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TextBox txtSearch = new() { Width = 150, PlaceholderText = "Поиск пациента" };
    private readonly CheckBox chkCompleted = new() { Text = "Только завершенные", AutoSize = true, Padding = new Padding(0, 4, 0, 0) };
    private readonly NumericUpDown numBudget = new() { Width = 90, Maximum = 10_000_000, ThousandsSeparator = true };
    private readonly Label lblTotals = new() { AutoSize = true, Padding = new Padding(8, 6, 0, 0) };
    private readonly Label lblPage = new() { AutoSize = true, Padding = new Padding(8, 6, 8, 0) };

    private readonly DataGridView gridGroups = NewGrid();

    private readonly ToolStripStatusLabel lblStatus = new() { Text = "Готово" };

    public FormMain()
    {
        Text = "Clinic CRM";
        Width = 1050;
        Height = 680;
        StartPosition = FormStartPosition.CenterScreen;

        string cs = $"Data Source={Path.Combine(AppContext.BaseDirectory, "clinic.db")}";
        DbInit.Ensure(cs);

        _departmentRepo = new SqliteRepository<Department>(cs, "Departments",
            "INSERT INTO Departments(Title,BaseRate) VALUES($t,$r)",
            r => new Department { Id = r.GetInt32(0), Title = r.GetString(1), BaseRate = Convert.ToDecimal(r.GetDouble(2)) },
            (cmd, d) =>
            {
                cmd.Parameters.AddWithValue("$t", d.Title);
                cmd.Parameters.AddWithValue("$r", (double)d.BaseRate);
            });

        _doctorRepo = new SqliteRepository<Doctor>(cs, "Doctors",
            "INSERT INTO Doctors(FullName,DepartmentId,Category,BaseSalary) VALUES($n,$d,$c,$s)",
            MapDoctor,
            (cmd, d) =>
            {
                cmd.Parameters.AddWithValue("$n", d.FullName);
                cmd.Parameters.AddWithValue("$d", d.DepartmentId);
                cmd.Parameters.AddWithValue("$c", d.Category);
                cmd.Parameters.AddWithValue("$s", (double)d.BaseSalary);
            });

        _appointmentRepo = new SqliteRepository<Appointment>(cs, "Appointments",
            "INSERT INTO Appointments(DoctorId,PatientName,Cost,Date,Status) VALUES($d,$p,$c,$dt,$s)",
            r => new Appointment
            {
                Id = r.GetInt32(0),
                DoctorId = r.GetInt32(1),
                PatientName = r.GetString(2),
                Cost = Convert.ToDecimal(r.GetDouble(3)),
                Date = DateTime.Parse(r.GetString(4), System.Globalization.CultureInfo.InvariantCulture),
                Status = r.GetString(5)
            },
            (cmd, a) =>
            {
                cmd.Parameters.AddWithValue("$d", a.DoctorId);
                cmd.Parameters.AddWithValue("$p", a.PatientName);
                cmd.Parameters.AddWithValue("$c", (double)a.Cost);
                cmd.Parameters.AddWithValue("$dt", a.Date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("$s", a.Status);
            });

        _clinic = new ClinicManager(_appointmentRepo);
        _clinic.Log = msg => lblStatus.Text = msg;
        _clinic.AppointmentRegistered += (_, e) =>
            lstLog.Items.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {e.Message}: {e.PatientName} → {e.DoctorName}, {e.Cost:N0}");

        var tabs = new TabControl { Dock = DockStyle.Fill };
        tabs.TabPages.Add(BuildDoctorsTab());
        tabs.TabPages.Add(BuildAppointmentTab());
        tabs.TabPages.Add(BuildAnalyticsTab());
        tabs.TabPages.Add(BuildGroupsTab());
        Controls.Add(tabs);

        var strip = new StatusStrip();
        strip.Items.Add(lblStatus);
        Controls.Add(strip);

        cmbCategory.Items.AddRange(new object[] { "Высшая", "Первая" });
        cmbCategory.SelectedIndex = 0;

        Reload();
        cmbDepartment.DataSource = _departments.Select(d => d.Title).ToList();
        cmbDeptFilter.DataSource = new[] { "Все" }.Concat(_departments.Select(d => d.Title)).ToList();
        cmbDeptFilter.SelectedIndexChanged += (_, _) => { _page = 1; RefreshAnalytics(); };
        txtSearch.TextChanged += (_, _) => { _page = 1; RefreshAnalytics(); };
        chkCompleted.CheckedChanged += (_, _) => { _page = 1; RefreshAnalytics(); };
        numBudget.ValueChanged += (_, _) => { _page = 1; RefreshAnalytics(); };
        RefreshAll();
    }

    private static DataGridView NewGrid() => new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };

    private static FlowLayoutPanel NewBar(params Control[] items)
    {
        var panel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(4) };
        panel.Controls.AddRange(items);
        return panel;
    }

    private static Button Btn(string text, EventHandler onClick, int width = 150)
    {
        var button = new Button { Text = text, Width = width, Height = 28 };
        button.Click += onClick;
        return button;
    }

    private static Label Lbl(string text) => new() { Text = text, AutoSize = true, Padding = new Padding(4, 6, 0, 0) };

    private Doctor MapDoctor(SqliteDataReader r)
    {
        int id = r.GetInt32(0);
        string name = r.GetString(1);
        int departmentId = r.IsDBNull(2) ? 0 : r.GetInt32(2);
        string category = r.GetString(3);
        decimal salary = Convert.ToDecimal(r.GetDouble(4));
        var license = new MedicalLicense($"ЛО-{id:0000}", DateTime.Today.AddYears(3));
        string? title = _departments.FirstOrDefault(d => d.Id == departmentId)?.Title;

        return title == "Хирургия"
            ? new Surgeon(id, name, salary, category, departmentId, license, 50 + id * 10)
            : new Doctor(id, name, salary, category, departmentId, license);
    }

    private void Reload()
    {
        _departments = _departmentRepo.GetAll();
        _doctors = _doctorRepo.GetAll();
        _employees = _doctors.Cast<MedicalEmployee>().ToList();
        _appointments = _appointmentRepo.GetAll();

        int selected = cmbDoctorPick.SelectedIndex;
        cmbDoctorPick.DataSource = _doctors.Select(d => $"{d.FullName} ({d.Category})").ToList();
        if (selected >= 0 && selected < _doctors.Count) cmbDoctorPick.SelectedIndex = selected;
    }

    private void RefreshAll()
    {
        RefreshDoctorsGrid();
        RefreshAnalytics();
        gridGroups.DataSource = ClinicAnalytics.RevenueByDoctor(_appointments, _doctors);
    }

    private TabPage BuildDoctorsTab()
    {
        var tab = new TabPage("Врачи и ООП-модель");
        tab.Controls.Add(gridDoctors);
        tab.Controls.Add(NewBar(
            Btn("Сортировать по окладу (IComparable)", (_, _) =>
            {
                _employees.Sort();
                RefreshDoctorsGrid();
                lblStatus.Text = "Список отсортирован по убыванию оклада";
            }, 250),
            Btn("Клонировать (ICloneable)", OnClone, 190),
            Btn("Экспорт отчета (Явная реализация)", OnExport, 250)));
        tab.Controls.Add(NewBar(
            Lbl("ФИО:"), txtDoctorName, Lbl("Категория:"), cmbCategory,
            Lbl("Отделение:"), cmbDepartment, Lbl("Оклад:"), numSalary,
            Btn("Добавить врача", OnAddDoctor, 130)));
        return tab;
    }

    private void RefreshDoctorsGrid() =>
        gridDoctors.DataSource = _employees.Select(e => new
        {
            e.Id,
            ФИО = e.FullName,
            Тип = e.GetType().Name,
            Категория = (e as Doctor)?.Category,
            Оклад = e.BaseSalary,
            Лицензия = (e as Doctor)?.License.Number,
            Выплата_за_20_приемов = e.CalculateMonthlyPayout(20)
        }).ToList();

    private Doctor? SelectedDoctor() =>
        gridDoctors.CurrentRow?.Cells["Id"].Value is int id
            ? _employees.FirstOrDefault(e => e.Id == id) as Doctor
            : null;

    private void OnAddDoctor(object? sender, EventArgs e)
    {
        try
        {
            var department = _departments.First(d => d.Title == cmbDepartment.Text);
            var doctor = new Doctor(0, txtDoctorName.Text, numSalary.Value, cmbCategory.Text, department.Id,
                new MedicalLicense("НОВАЯ", DateTime.Today.AddYears(3)));
            _doctorRepo.Add(doctor);
            Reload();
            RefreshAll();
            txtDoctorName.Clear();
            lblStatus.Text = $"Добавлен врач: {doctor.FullName}";
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void OnClone(object? sender, EventArgs e)
    {
        if (SelectedDoctor() is not { } original) return;

        var clone = (Doctor)original.Clone();
        clone.License.Number = original.License.Number + "-COPY";

        MessageBox.Show(
            $"Оригинал: {original.License.Number}\nКлон: {clone.License.Number}\n" +
            $"Одна и та же лицензия в памяти: {ReferenceEquals(original.License, clone.License)}",
            "Глубокое клонирование");
    }

    private void OnExport(object? sender, EventArgs e)
    {
        if (SelectedDoctor() is not { } doctor) return;
        MessageBox.Show(((IExportable)doctor).GetSummary(), "Отчет");
    }

    private TabPage BuildAppointmentTab()
    {
        var tab = new TabPage("Запись на прием и Events");
        tab.Controls.Add(lstLog);
        tab.Controls.Add(NewBar(
            Lbl("Врач:"), cmbDoctorPick, Lbl("Пациент:"), txtPatient,
            Lbl("Стоимость:"), numCost, Lbl("Дата:"), dtpDate,
            Btn("Зарегистрировать прием", OnRegister, 190)));
        return tab;
    }

    private void OnRegister(object? sender, EventArgs e)
    {
        if (cmbDoctorPick.SelectedIndex < 0 || cmbDoctorPick.SelectedIndex >= _doctors.Count) return;
        if (string.IsNullOrWhiteSpace(txtPatient.Text))
        {
            MessageBox.Show("Введите имя пациента.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _clinic.Register(_doctors[cmbDoctorPick.SelectedIndex], txtPatient.Text.Trim(), numCost.Value, dtpDate.Value.Date);
        txtPatient.Clear();
        Reload();
        RefreshAll();
    }

    private TabPage BuildAnalyticsTab()
    {
        var tab = new TabPage("LINQ-Аналитика и DTO");
        tab.Controls.Add(gridDto);
        tab.Controls.Add(NewBar(
            Btn("◀", (_, _) => { _page--; RefreshAnalytics(); }, 40), lblPage,
            Btn("▶", (_, _) => { _page++; RefreshAnalytics(); }, 40), lblTotals));
        tab.Controls.Add(NewBar(
            Lbl("Отделение:"), cmbDeptFilter, txtSearch, chkCompleted, Lbl("Мин. стоимость:"), numBudget));
        return tab;
    }

    private void RefreshAnalytics()
    {
        var budget = _clinic.CreateBudgetFilter(numBudget.Value);
        var source = _appointments.Where(budget).ToList();
        if (chkCompleted.Checked) source = source.FindAll(_clinic.IsCompleted);

        var dtos = ClinicAnalytics.BuildDto(source, _doctors, _departments, _clinic.DiscountedCost)
            .Where(d => cmbDeptFilter.SelectedIndex <= 0 || d.Department == cmbDeptFilter.Text)
            .Where(d => d.Patient.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase));

        var sorted = ClinicAnalytics.Sort(dtos);
        int pages = ClinicAnalytics.PageCount(sorted.Count, PageSize);
        _page = Math.Clamp(_page, 1, pages);

        gridDto.DataSource = ClinicAnalytics.Page(sorted, _page, PageSize);
        lblPage.Text = $"Страница {_page} из {pages}";
        lblTotals.Text = $"Приемов (Count): {sorted.Count}   Выручка (Sum): {sorted.Sum(d => d.Cost):N0}   " +
                         $"Со скидкой: {sorted.Sum(d => d.Discounted):N0}";
    }

    private TabPage BuildGroupsTab()
    {
        var tab = new TabPage("Группировка и Множества");
        tab.Controls.Add(gridGroups);
        tab.Controls.Add(NewBar(
            Btn("Выручка по врачам (GroupBy)", (_, _) =>
                gridGroups.DataSource = ClinicAnalytics.RevenueByDoctor(_appointments, _doctors), 220),
            Btn("Пересечение пациентов (Intersect)", (_, _) =>
                gridGroups.DataSource = ClinicAnalytics.TherapyAndSurgery(AllDtos()), 250),
            Btn("Исключение (Except)", (_, _) =>
                gridGroups.DataSource = ClinicAnalytics.TherapyOnly(AllDtos()), 170)));
        return tab;
    }

    private List<AppointmentViewDto> AllDtos() =>
        ClinicAnalytics.BuildDto(_appointments, _doctors, _departments, _clinic.DiscountedCost);
}
