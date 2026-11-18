using System.Drawing;
using System.Windows.Forms;

namespace WarehouseApp;

partial class FormMain
{
    private System.ComponentModel.IContainer components = null;

    private Label lblCategory;
    private Label lblSearch;
    private ComboBox cmbCategories;
    private TextBox txtSearch;
    private Button btnFilter;
    private DataGridView dgvReport;
    private Label lblTotalSum;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblCategory = new Label();
        lblSearch = new Label();
        cmbCategories = new ComboBox();
        txtSearch = new TextBox();
        btnFilter = new Button();
        dgvReport = new DataGridView();
        lblTotalSum = new Label();
        ((System.ComponentModel.ISupportInitialize)dgvReport).BeginInit();
        SuspendLayout();

        lblCategory.AutoSize = true;
        lblCategory.Location = new Point(12, 15);
        lblCategory.Name = "lblCategory";
        lblCategory.Text = "Категория:";

        cmbCategories.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCategories.Location = new Point(90, 12);
        cmbCategories.Name = "cmbCategories";
        cmbCategories.Size = new Size(200, 23);
        cmbCategories.SelectedIndexChanged += cmbCategories_SelectedIndexChanged;

        lblSearch.AutoSize = true;
        lblSearch.Location = new Point(310, 15);
        lblSearch.Name = "lblSearch";
        lblSearch.Text = "Поиск:";

        txtSearch.Location = new Point(365, 12);
        txtSearch.Name = "txtSearch";
        txtSearch.Size = new Size(200, 23);
        txtSearch.TextChanged += txtSearch_TextChanged;

        btnFilter.Location = new Point(580, 11);
        btnFilter.Name = "btnFilter";
        btnFilter.Size = new Size(168, 25);
        btnFilter.Text = "Применить фильтр";
        btnFilter.UseVisualStyleBackColor = true;
        btnFilter.Click += btnFilter_Click;

        dgvReport.AllowUserToAddRows = false;
        dgvReport.AllowUserToDeleteRows = false;
        dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvReport.Location = new Point(12, 50);
        dgvReport.Name = "dgvReport";
        dgvReport.ReadOnly = true;
        dgvReport.RowHeadersVisible = false;
        dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvReport.Size = new Size(736, 370);

        lblTotalSum.AutoSize = true;
        lblTotalSum.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblTotalSum.Location = new Point(12, 435);
        lblTotalSum.Name = "lblTotalSum";
        lblTotalSum.Text = "Общая стоимость запаса: 0,00 руб.";

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(760, 470);
        Controls.Add(lblCategory);
        Controls.Add(cmbCategories);
        Controls.Add(lblSearch);
        Controls.Add(txtSearch);
        Controls.Add(btnFilter);
        Controls.Add(dgvReport);
        Controls.Add(lblTotalSum);
        Name = "FormMain";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Складской учет товаров";
        Load += FormMain_Load;
        ((System.ComponentModel.ISupportInitialize)dgvReport).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
