using System;
using System.Linq;
using System.Windows.Forms;

namespace WarehouseApp;

public partial class FormMain : Form
{
    private readonly ReportService reportService = new ReportService();
    private bool isLoaded;

    public FormMain()
    {
        InitializeComponent();
    }

    private void FormMain_Load(object sender, EventArgs e)
    {
        cmbCategories.DisplayMember = "Name";
        cmbCategories.ValueMember = "Id";
        cmbCategories.DataSource = reportService.GetCategories();
        cmbCategories.SelectedIndex = 0;

        isLoaded = true;
        RefreshReport();
    }

    private void RefreshReport()
    {
        if (!isLoaded)
            return;

        int categoryId = cmbCategories.SelectedValue is int id ? id : 0;
        string searchText = txtSearch.Text.Trim();

        var report = reportService.GetProductReport(categoryId, searchText);

        dgvReport.DataSource = report;
        ConfigureColumns();

        decimal total = report.Sum(r => r.TotalValue);
        lblTotalSum.Text = $"Общая стоимость запаса: {total:N2} руб.";
    }

    private void ConfigureColumns()
    {
        dgvReport.Columns[nameof(ProductReportItem.ProductName)].HeaderText = "Товар";
        dgvReport.Columns[nameof(ProductReportItem.CategoryName)].HeaderText = "Категория";
        dgvReport.Columns[nameof(ProductReportItem.Price)].HeaderText = "Цена";
        dgvReport.Columns[nameof(ProductReportItem.Quantity)].HeaderText = "Остаток";
        dgvReport.Columns[nameof(ProductReportItem.TotalValue)].HeaderText = "Стоимость запаса";

        dgvReport.Columns[nameof(ProductReportItem.Price)].DefaultCellStyle.Format = "N2";
        dgvReport.Columns[nameof(ProductReportItem.TotalValue)].DefaultCellStyle.Format = "N2";
    }

    private void cmbCategories_SelectedIndexChanged(object sender, EventArgs e) => RefreshReport();

    private void txtSearch_TextChanged(object sender, EventArgs e) => RefreshReport();

    private void btnFilter_Click(object sender, EventArgs e) => RefreshReport();
}
