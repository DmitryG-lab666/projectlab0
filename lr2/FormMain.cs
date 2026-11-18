using SmartHomeMasterConsole.Data;
using SmartHomeMasterConsole.Models;
using SmartHomeMasterConsole.Services;

namespace SmartHomeMasterConsole;

public class FormMain : Form
{
    private readonly Database _db = new();
    private readonly DeviceRepository _repo = new();
    private readonly SensorPoller _poller;
    private List<Room> _rooms = new();
    private List<TelemetryLog> _logs = new();
    private int _page = 1;

    private readonly TabControl _tabs = new() { Dock = DockStyle.Fill };

    private readonly TextBox _txtModel = new() { Width = 140, Margin = new Padding(3, 5, 3, 3) };
    private readonly NumericUpDown _numPower = new() { Minimum = 0, Maximum = 10000, DecimalPlaces = 1, Value = 50, Width = 80, Margin = new Padding(3, 5, 3, 3) };
    private readonly ComboBox _cmbType = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 100, Margin = new Padding(3, 5, 3, 3) };
    private readonly ComboBox _cmbRoom = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130, Margin = new Padding(3, 5, 3, 3) };
    private readonly DataGridView _dgvDevices = Grid();
    private readonly TextBox _txtLog1 = Out();

    private Button _btnScan = null!;
    private readonly ListBox _lstEvents = new() { Dock = DockStyle.Fill, Font = new Font("Consolas", 9.5f), HorizontalScrollbar = true };
    private readonly NumericUpDown _numThreshold = new() { Minimum = 0, Maximum = 1000, DecimalPlaces = 2, Value = 1, Width = 80, Margin = new Padding(3, 5, 3, 3) };

    private readonly NumericUpDown _numMinPower = new() { Minimum = 0, Maximum = 10000, DecimalPlaces = 1, Value = 0, Width = 80, Margin = new Padding(3, 5, 3, 3) };
    private readonly ComboBox _cmbFilterRoom = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130, Margin = new Padding(3, 5, 3, 3) };
    private readonly DataGridView _dgvReport = Grid();
    private readonly TextBox _txtLog3 = Out();

    private readonly ComboBox _cmbRoomA = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130, Margin = new Padding(3, 5, 3, 3) };
    private readonly ComboBox _cmbRoomB = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130, Margin = new Padding(3, 5, 3, 3) };
    private readonly NumericUpDown _numPageSize = new() { Minimum = 1, Maximum = 100, Value = 5, Width = 60, Margin = new Padding(3, 5, 3, 3) };
    private readonly Label _lblPage = Lbl("Стр. 1 из 1");
    private readonly DataGridView _dgvPage = Grid();
    private readonly TextBox _txtSets = Out();

    public FormMain()
    {
        _poller = new SensorPoller(_db);

        Text = "SmartHome Master Console";
        Size = new Size(1180, 760);
        StartPosition = FormStartPosition.CenterScreen;

        _cmbType.Items.AddRange(new object[] { "Sensor", "Actuator" });
        _cmbType.SelectedIndex = 0;

        BuildTabs();
        Load += FormMain_Load;
    }

    private static Label Lbl(string text) => new()
    {
        Text = text,
        AutoSize = true,
        Margin = new Padding(8, 8, 3, 0)
    };

    private static Button Btn(string text, EventHandler handler)
    {
        var button = new Button
        {
            Text = text,
            AutoSize = true,
            Margin = new Padding(4),
            Padding = new Padding(6, 2, 6, 2)
        };
        button.Click += handler;
        return button;
    }

    private static FlowLayoutPanel Bar(params Control[] items)
    {
        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            WrapContents = true
        };
        panel.Controls.AddRange(items);
        return panel;
    }

    private static DataGridView Grid() => new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        MultiSelect = false,
        RowHeadersVisible = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };

    private static TextBox Out() => new()
    {
        Dock = DockStyle.Fill,
        Multiline = true,
        ReadOnly = true,
        ScrollBars = ScrollBars.Vertical,
        Font = new Font("Consolas", 9.5f)
    };

    private static TableLayoutPanel Stack(Control top, int topHeight, Control fill, Control? bottom = null, int bottomHeight = 130)
    {
        var table = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = bottom is null ? 2 : 3
        };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, topHeight));
        table.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        table.Controls.Add(top, 0, 0);
        table.Controls.Add(fill, 0, 1);

        if (bottom is not null)
        {
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, bottomHeight));
            table.Controls.Add(bottom, 0, 2);
        }

        return table;
    }

    private void BuildTabs()
    {
        var tab1 = new TabPage("Реестр приборов (ООП && Generics)");
        tab1.Controls.Add(Stack(
            Bar(
                Lbl("Модель"), _txtModel,
                Lbl("Мощность, Вт"), _numPower,
                Lbl("Тип"), _cmbType,
                Lbl("Комната"), _cmbRoom,
                Btn("Добавить", BtnAdd_Click),
                Btn("Сортировка (IComparable)", BtnSort_Click),
                Btn("Сравнить (CompareTo)", BtnCompare_Click),
                Btn("Deep Copy (ICloneable)", BtnDeepCopy_Click),
                Btn("Экспорт (IExportable)", BtnExport_Click),
                Btn("Shadowing (new)", BtnShadow_Click)),
            90, _dgvDevices, _txtLog1, 130));

        _btnScan = Btn("Запустить опрос (Task.WhenAll)", BtnScan_Click);
        var tab2 = new TabPage("Асинхронный опрос && Events");
        tab2.Controls.Add(Stack(
            Bar(
                _btnScan,
                Btn("ValueTask-кэш", BtnCache_Click),
                Lbl("Порог, кВт·ч/сутки"), _numThreshold,
                Btn("Замыкание (Closure)", BtnClosure_Click),
                Btn("Критические (Predicate)", BtnCritical_Click),
                Btn("Очистить", (s, e) => _lstEvents.Items.Clear())),
            90, _lstEvents));

        var tab3 = new TabPage("LINQ-Аналитика && DTO");
        tab3.Controls.Add(Stack(
            Bar(
                Lbl("Мин. мощность, Вт"), _numMinPower,
                Lbl("Комната"), _cmbFilterRoom,
                Btn("Join + фильтр + сортировка", BtnJoin_Click),
                Btn("GroupJoin по комнатам", BtnGroupJoin_Click),
                Btn("GroupBy + агрегаты", BtnGroupBy_Click),
                Btn("Квантификаторы и отложенное выполнение", BtnQuantifiers_Click)),
            90, _dgvReport, _txtLog3, 130));

        _numPageSize.ValueChanged += (s, e) =>
        {
            _page = 1;
            RefreshPage();
        };

        var tab4 = new TabPage("Множества && Пагинация");
        tab4.Controls.Add(Stack(
            Bar(
                Lbl("Комната A"), _cmbRoomA,
                Lbl("Комната B"), _cmbRoomB,
                Btn("Сравнить (Intersect/Except/Union)", BtnSets_Click),
                Lbl("Размер страницы"), _numPageSize,
                Btn("◄ Назад", BtnPrev_Click),
                _lblPage,
                Btn("Вперёд ►", BtnNext_Click)),
            90, _dgvPage, _txtSets, 150));

        _tabs.TabPages.AddRange(new[] { tab1, tab2, tab3, tab4 });
        Controls.Add(_tabs);
    }

    private async void FormMain_Load(object? sender, EventArgs e)
    {
        try
        {
            _db.EnsureCreated();
            _rooms = await _db.LoadRoomsAsync();
            var devices = await _db.LoadDevicesAsync();
            _logs = await _db.LoadTelemetryAsync();

            foreach (var device in devices)
            {
                device.StatusChanged += OnDeviceStatusChanged;
                _repo.Add(device);
            }

            BindRooms();
            RefreshDevices();
            RefreshPage();
            Log1($"Загружено: комнат {_rooms.Count}, устройств {devices.Count}, записей телеметрии {_logs.Count}");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка загрузки", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BindRooms()
    {
        _cmbRoom.DataSource = _rooms.ToList();
        _cmbRoom.DisplayMember = nameof(Room.Title);
        _cmbRoom.ValueMember = nameof(Room.Id);

        _cmbFilterRoom.Items.Clear();
        _cmbFilterRoom.Items.Add("Все");
        foreach (var room in _rooms)
        {
            _cmbFilterRoom.Items.Add(room.Title);
            _cmbRoomA.Items.Add(room.Title);
            _cmbRoomB.Items.Add(room.Title);
        }

        _cmbFilterRoom.SelectedIndex = 0;
        _cmbRoomA.SelectedIndex = 0;
        _cmbRoomB.SelectedIndex = _rooms.Count > 1 ? 1 : 0;
    }

    private static DeviceRow ToRow(Device d) => new(
        d.Id,
        d.Model,
        d.DeviceType,
        d.Power,
        d.RoomId,
        d.GetStatus(),
        Math.Round(d.CalculatePowerConsumption(), 3),
        d.Calibration.ToString());

    private void ShowDevices(List<Device> devices)
    {
        _dgvDevices.DataSource = devices.Select(ToRow).ToList();
    }

    private void RefreshDevices() => ShowDevices(_repo.GetAll());

    private Device? GetSelectedDevice()
    {
        if (_dgvDevices.CurrentRow?.DataBoundItem is DeviceRow row)
        {
            return _repo.GetById(row.Id);
        }

        MessageBox.Show("Выберите устройство в таблице.", "Нет выбора", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return null;
    }

    private void Log1(string message) => _txtLog1.AppendText(message + Environment.NewLine);

    private void AddEvent(string message)
    {
        _lstEvents.Items.Add(message);
        _lstEvents.TopIndex = _lstEvents.Items.Count - 1;
    }

    private void OnDeviceStatusChanged(object? sender, DeviceEventArgs e)
    {
        if (InvokeRequired)
        {
            BeginInvoke(new Action(() => OnDeviceStatusChanged(sender, e)));
            return;
        }

        AddEvent($"[{e.Timestamp:HH:mm:ss}] {e.Message}");
    }

    private async void BtnAdd_Click(object? sender, EventArgs e)
    {
        string model = _txtModel.Text.Trim();
        if (model.Length == 0 || _cmbRoom.SelectedValue is not int roomId || _cmbType.SelectedItem is not string type)
        {
            MessageBox.Show("Заполните модель, комнату и тип.", "Проверка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            double power = (double)_numPower.Value;
            int id = await _db.InsertDeviceAsync(roomId, model, power, type);
            var device = DeviceFactory.Create(id, roomId, model, power, type);
            device.StatusChanged += OnDeviceStatusChanged;
            _repo.Add(device);

            RefreshDevices();
            RefreshPage();
            Log1($"Добавлено: {device}");
            _txtModel.Clear();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка добавления", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnSort_Click(object? sender, EventArgs e)
    {
        var list = _repo.GetAll();
        list.Sort();
        ShowDevices(list);
        Log1("Сортировка по мощности через IComparable<Device> выполнена");
    }

    private void BtnCompare_Click(object? sender, EventArgs e)
    {
        var selected = GetSelectedDevice();
        var first = _repo.GetAll().FirstOrDefault();
        if (selected is null || first is null)
        {
            return;
        }

        int result = selected.CompareTo(first);
        Log1($"CompareTo: {selected.Model} ({selected.Power:F1} Вт) и {first.Model} ({first.Power:F1} Вт) = {result}");
        Log1($"Equals: {selected.Equals(first)}, HashCode равны: {selected.GetHashCode() == first.GetHashCode()}");
    }

    private void BtnDeepCopy_Click(object? sender, EventArgs e)
    {
        var selected = GetSelectedDevice();
        if (selected is null)
        {
            return;
        }

        var copy = (Device)((ICloneable)selected).Clone();
        copy.Calibration.Offset += 1.0;

        Log1($"Оригинал: {selected.Calibration}");
        Log1($"Копия после изменения: {copy.Calibration}");
        Log1($"Один и тот же объект паспорта: {ReferenceEquals(selected.Calibration, copy.Calibration)}");
        Log1($"Equals(оригинал, копия): {selected.Equals(copy)}");
    }

    private void BtnExport_Click(object? sender, EventArgs e)
    {
        var selected = GetSelectedDevice();
        if (selected is null)
        {
            return;
        }

        Log1("Экспорт: " + ((IExportable)selected).GetExportData());
    }

    private void BtnShadow_Click(object? sender, EventArgs e)
    {
        var selected = GetSelectedDevice();
        if (selected is null)
        {
            return;
        }

        Log1($"Через ссылку Device: {selected.Describe()}");

        if (selected is SmartSensor sensor)
        {
            Log1($"Через ссылку SmartSensor: {sensor.Describe()}");
        }
        else if (selected is SmartActuator actuator)
        {
            Log1($"Через ссылку SmartActuator: {actuator.Describe()}");
        }
    }

    private async void BtnScan_Click(object? sender, EventArgs e)
    {
        _btnScan.Enabled = false;
        try
        {
            var sensors = _repo.GetAll().OfType<SmartSensor>().ToList();
            foreach (var sensor in sensors)
            {
                sensor.SetActive(false);
            }

            AddEvent($"Старт опроса датчиков: {sensors.Count}");
            string[] results = await _poller.PollAllAsync(sensors);
            AddEvent($"Task.WhenAll завершён, получено ответов: {results.Length}");
            RefreshDevices();
        }
        catch (Exception ex)
        {
            AddEvent("Ошибка: " + ex.Message);
        }
        finally
        {
            _btnScan.Enabled = true;
        }
    }

    private async void BtnCache_Click(object? sender, EventArgs e)
    {
        var sensor = _repo.GetAll().OfType<SmartSensor>().FirstOrDefault();
        if (sensor is null)
        {
            return;
        }

        try
        {
            string first = await _poller.GetCachedReadingAsync(sensor);
            string second = await _poller.GetCachedReadingAsync(sensor);
            AddEvent("ValueTask, вызов 1: " + first);
            AddEvent("ValueTask, вызов 2: " + second);
        }
        catch (Exception ex)
        {
            AddEvent("Ошибка: " + ex.Message);
        }
    }

    private void BtnClosure_Click(object? sender, EventArgs e)
    {
        var check = DelegateTools.CreateThresholdChecker((double)_numThreshold.Value);
        var over = _repo.GetAll().Where(d => check(DelegateTools.DailyConsumption(d))).ToList();

        AddEvent($"Замыкание: устройств выше порога {_numThreshold.Value:F2} кВт·ч/сутки — {over.Count}");
        DelegateTools.ForEachDevice(over, d => AddEvent($"  {d.Model} #{d.Id}: {DelegateTools.DailyConsumption(d):F3}"));
    }

    private void BtnCritical_Click(object? sender, EventArgs e)
    {
        var critical = DelegateTools.FindCritical(_repo.GetAll());
        AddEvent($"Predicate: критических устройств — {critical.Count}");
        DelegateTools.ForEachDevice(critical, d => AddEvent($"  {d}"));
    }

    private List<DeviceReportDto> CurrentReport() => LinqAnalytics.BuildReport(_repo.GetAll(), _rooms);

    private void BtnJoin_Click(object? sender, EventArgs e)
    {
        var report = CurrentReport();
        string? room = _cmbFilterRoom.SelectedIndex > 0 ? _cmbFilterRoom.SelectedItem as string : null;
        var data = LinqAnalytics.FilterSorted(report, (double)_numMinPower.Value, room);

        _dgvReport.DataSource = data;
        _txtLog3.Text = $"Join: {report.Count} записей, после Where и OrderBy.ThenByDescending: {data.Count}";
    }

    private void BtnGroupJoin_Click(object? sender, EventArgs e)
    {
        var data = LinqAnalytics.RoomSummaries(_repo.GetAll(), _rooms);
        _dgvReport.DataSource = data;
        _txtLog3.Text = $"GroupJoin: комнат {data.Count}, всего устройств {data.Sum(x => x.DeviceCount)}";
    }

    private void BtnGroupBy_Click(object? sender, EventArgs e)
    {
        var data = LinqAnalytics.GroupStats(CurrentReport());
        _dgvReport.DataSource = data;
        _txtLog3.Text = $"GroupBy: групп {data.Count}, суммарная мощность {data.Sum(x => x.TotalPower):F1} Вт";
    }

    private void BtnQuantifiers_Click(object? sender, EventArgs e)
    {
        _txtLog3.Text =
            LinqAnalytics.SafetyChecks(CurrentReport(), 2000) + Environment.NewLine +
            LinqAnalytics.DeferredDemo(_repo.GetAll());
    }

    private void BtnSets_Click(object? sender, EventArgs e)
    {
        string a = _cmbRoomA.SelectedItem as string ?? string.Empty;
        string b = _cmbRoomB.SelectedItem as string ?? string.Empty;
        _txtSets.Text = LinqAnalytics.CompareRooms(CurrentReport(), a, b);
    }

    private void BtnPrev_Click(object? sender, EventArgs e)
    {
        _page--;
        RefreshPage();
    }

    private void BtnNext_Click(object? sender, EventArgs e)
    {
        _page++;
        RefreshPage();
    }

    private void RefreshPage()
    {
        int pageSize = (int)_numPageSize.Value;
        var sorted = LinqAnalytics.FilterSorted(CurrentReport(), 0, null);
        int totalPages = Math.Max(1, (int)Math.Ceiling(sorted.Count / (double)pageSize));

        _page = Math.Clamp(_page, 1, totalPages);
        _dgvPage.DataSource = LinqAnalytics.Page(sorted, _page, pageSize);
        _lblPage.Text = $"Стр. {_page} из {totalPages}";
    }
}
