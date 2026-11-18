namespace CorpFleet;

// ───────────── Модуль 5: WinForms (интерфейс строится кодом, без дизайнера) ─────────────
public class FormMain : Form
{
    private const int PageSize = 3;

    private readonly Database _db = new();
    private readonly FleetManager _fleet = new();
    private List<Category> _cats = new();
    private List<VehicleRow> _rows = new();
    private List<LogRow> _logs = new();
    private int _page = 1;

    // вкладка 1
    private readonly DataGridView gridVehicles = NewGrid();
    private readonly TextBox txtModel = new() { Width = 130, PlaceholderText = "Модель" };
    private readonly NumericUpDown numPower = new() { Width = 70, Minimum = 1, Maximum = 2000, Value = 150 };
    private readonly NumericUpDown numPrice = new() { Width = 110, Minimum = 1, Maximum = 1_000_000_000, Value = 3_000_000, ThousandsSeparator = true };
    private readonly NumericUpDown numLoad = new() { Width = 60, Minimum = 1, Maximum = 100, Value = 5, DecimalPlaces = 1 };
    private readonly ComboBox cmbType = new() { Width = 100, DropDownStyle = ComboBoxStyle.DropDownList };
    // вкладка 2
    private readonly DataGridView gridDto = NewGrid();
    private readonly TextBox txtSearch = new() { Width = 150, PlaceholderText = "Поиск (модель/категория)" };
    private readonly NumericUpDown numMin = new() { Width = 110, Maximum = 1_000_000_000, ThousandsSeparator = true };
    private readonly NumericUpDown numMax = new() { Width = 110, Maximum = 1_000_000_000, Value = 1_000_000_000, ThousandsSeparator = true };
    // вкладка 3
    private readonly TextBox txtOut = new() { Multiline = true, ReadOnly = true, Dock = DockStyle.Fill, ScrollBars = ScrollBars.Vertical, Font = new Font("Consolas", 10) };
    // вкладка 4
    private readonly DataGridView gridPaged = NewGrid();
    private readonly Label lblPage = new() { AutoSize = true, Padding = new Padding(8, 6, 8, 0) };
    private readonly ListBox lstEvents = new() { Dock = DockStyle.Bottom, Height = 150 };

    public FormMain()
    {
        Text = "CorpFleet Analytics";
        Width = 1000;
        Height = 650;
        StartPosition = FormStartPosition.CenterScreen;

        // подписка на событие через лямбда-выражение
        _fleet.StatusChanged += (sender, e) =>
            lstEvents.Items.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {e.Message}" +
                                      (e.OperationPrice > 0 ? $" — {e.OperationPrice:N0}" : ""));

        var tabs = new TabControl { Dock = DockStyle.Fill };
        tabs.TabPages.Add(BuildTab1());
        tabs.TabPages.Add(BuildTab2());
        tabs.TabPages.Add(BuildTab3());
        tabs.TabPages.Add(BuildTab4());
        Controls.Add(tabs);

        LoadFromDb();
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
        var p = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(4) };
        p.Controls.AddRange(items);
        return p;
    }

    private static Button Btn(string text, EventHandler onClick, int width = 120)
    {
        var b = new Button { Text = text, Width = width, Height = 28 };
        b.Click += onClick;
        return b;
    }

    private static Label Lbl(string t) => new() { Text = t, AutoSize = true, Padding = new Padding(4, 6, 0, 0) };

    // ───── Загрузка данных (Materialization) ─────
    private void LoadFromDb()
    {
        ReloadDbData();
        cmbType.Items.AddRange(new object[] { "Легковой", "Грузовой" });
        cmbType.SelectedIndex = 0;

        var vehicles = _rows.Select(r =>
        {
            var cat = _cats.First(c => c.Id == r.CategoryId);
            Vehicle v = cat.CategoryName == "Грузовые"
                ? new CargoVehicle(r.Id, r.Model, r.Power, r.BasePrice, Math.Round(r.Power / 40.0, 1))
                : new PassengerVehicle(r.Id, r.Model, r.Power, r.BasePrice, cat.CategoryName == "Автобусы" ? 50 : 5);
            v.ApplyCategory(cat);
            return v;
        });
        _fleet.Load(vehicles);
    }

    private void ReloadDbData()
    {
        _cats = _db.GetCategories();
        _rows = _db.GetVehicles();
        _logs = _db.GetLogs();
    }

    private List<FleetReportDto> Report() => FleetAnalytics.BuildReport(_cats, _rows, _logs);

    private void RefreshAll()
    {
        RefreshVehicles();
        ApplyFilter();
        ShowPage();
    }

    // ───── Вкладка 1: Реестр ТС и ООП-модель ─────
    private TabPage BuildTab1()
    {
        var tab = new TabPage("Реестр ТС и ООП-Модель");
        tab.Controls.Add(gridVehicles);
        tab.Controls.Add(NewBar(
            Btn("Клонировать", (_, _) => { if (SelectedId() is int id) { _fleet.CloneVehicle(id); RefreshVehicles(); } }),
            Btn("Сортировать", (_, _) => { _fleet.SortByPrice(); RefreshVehicles(); }),
            Btn("Экспорт (is)", OnExport),
            Btn("Удалить", (_, _) => { if (SelectedId() is int id) { _fleet.DeleteVehicle(id); RefreshVehicles(); } }),
            Btn("Замыкания", OnClosures)));
        tab.Controls.Add(NewBar(
            Lbl("Тип:"), cmbType, Lbl("Модель:"), txtModel, Lbl("Мощн.:"), numPower,
            Lbl("Цена:"), numPrice, Lbl("Груз, т:"), numLoad, Btn("Добавить", OnAdd, 90)));
        return tab;
    }

    private void RefreshVehicles() =>
        gridVehicles.DataSource = _fleet.Repo.GetAll().Select(v => new
        {
            v.Id,
            Тип = v.GetType().Name,
            v.Model,
            Мощность = v.Power,
            Цена = v.BasePrice,
            Налог = Math.Round(v.CalculateTax())
        }).ToList();

    private int? SelectedId() =>
        gridVehicles.CurrentRow?.Cells["Id"].Value is int id ? id : null;

    private void OnAdd(object? s, EventArgs e)
    {
        try
        {
            bool cargo = cmbType.SelectedItem?.ToString() == "Грузовой";
            var cat = _cats.First(c => c.CategoryName == (cargo ? "Грузовые" : "Легковые"));
            int power = (int)numPower.Value;
            double price = (double)numPrice.Value;

            Vehicle v = cargo
                ? new CargoVehicle(0, txtModel.Text, power, price, (double)numLoad.Value)
                : new PassengerVehicle(0, txtModel.Text, power, price);   // валидация — до записи в БД

            int id = _db.InsertVehicle(v.Model, price, power, cat.Id);
            v.SetId(id);
            v.ApplyCategory(cat);
            _fleet.AddVehicle(v);

            ReloadDbData();
            RefreshAll();
            txtModel.Clear();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void OnExport(object? s, EventArgs e)
    {
        if (SelectedId() is not int id || _fleet.Repo.GetById(id) is not { } v) return;

        // явная реализация доступна только через интерфейс — паттерн-матчинг is
        if (v is IExportable exp)
            MessageBox.Show(exp.GetExportInfo(), "IExportable");
        else
            MessageBox.Show("Объект не поддерживает экспорт.");
    }

    private void OnClosures(object? s, EventArgs e)
    {
        int count = _fleet.CountPowerful(200);
        var discount = _fleet.MakeDiscount(10);
        double total = _fleet.Total(v => discount(v.BasePrice));
        var powerful = _fleet.Find(v => v.Power > 200);
        MessageBox.Show($"ТС мощнее 200 л.с.: {count} ({string.Join(", ", powerful.Select(v => v.Model))})\n" +
                        $"Стоимость парка со скидкой 10%: {total:N0}");
    }

    // ───── Вкладка 2: LINQ-аналитика и DTO ─────
    private TabPage BuildTab2()
    {
        var tab = new TabPage("LINQ-Аналитика и DTO");
        tab.Controls.Add(gridDto);
        tab.Controls.Add(NewBar(Lbl("Цена от:"), numMin, Lbl("до:"), numMax, txtSearch,
            Btn("Применить", (_, _) => { ReloadDbData(); ApplyFilter(); }, 100)));
        return tab;
    }

    private void ApplyFilter()
    {
        var filtered = FleetAnalytics.Filter(Report(), (double)numMin.Value, (double)numMax.Value, txtSearch.Text);
        gridDto.DataSource = FleetAnalytics.Sort(filtered);
    }

    // ───── Вкладка 3: Группировка, агрегаты и множества ─────
    private TabPage BuildTab3()
    {
        var tab = new TabPage("Группировка, Агрегаты и Множества");
        tab.Controls.Add(txtOut);
        tab.Controls.Add(NewBar(
            Btn("Except", (_, _) => ShowSet("Online \\ Offline (Except)", FleetAnalytics.OnlineExceptOffline(_logs)), 90),
            Btn("Intersect", (_, _) => ShowSet("Online ∩ Offline (Intersect)", FleetAnalytics.Common(_logs)), 90),
            Btn("Union", (_, _) => ShowSet("Online ∪ Offline (Union)", FleetAnalytics.All(_logs)), 90),
            Btn("Статистика", (_, _) =>
            {
                ReloadDbData();
                var r = Report();
                txtOut.Text = FleetAnalytics.Summary(r) + Environment.NewLine + Environment.NewLine +
                              FleetAnalytics.Checks(_logs, r);
            }, 110)));
        return tab;
    }

    private void ShowSet(string title, List<string> items) =>
        txtOut.Text = title + Environment.NewLine + (items.Count == 0 ? "(пусто)" : string.Join(Environment.NewLine, items));

    // ───── Вкладка 4: Постраничный просмотр и события ─────
    private TabPage BuildTab4()
    {
        var tab = new TabPage("Постраничный просмотр и События");
        tab.Controls.Add(gridPaged);
        tab.Controls.Add(lstEvents);
        tab.Controls.Add(NewBar(
            Btn("◀ Назад", (_, _) => { _page--; ShowPage(); }, 90),
            lblPage,
            Btn("Вперёд ▶", (_, _) => { _page++; ShowPage(); }, 90)));
        return tab;
    }

    private void ShowPage()
    {
        var sorted = FleetAnalytics.Sort(Report());
        int pages = FleetAnalytics.PageCount(sorted.Count, PageSize);
        _page = Math.Clamp(_page, 1, pages);
        gridPaged.DataSource = FleetAnalytics.Page(sorted, _page, PageSize);
        lblPage.Text = $"Страница {_page} из {pages}";
        _fleet.Notify($"Показана страница {_page}/{pages}");
    }
}
