using System.Drawing;
using System.Windows.Forms;

namespace CourseApp;

partial class FormMain
{
    private System.ComponentModel.IContainer components = null;

    private Label lblTitle;
    private Label lblPrice;
    private Label lblCertNumber;
    private Label lblCertDays;
    private TextBox txtTitle;
    private NumericUpDown numPrice;
    private TextBox txtCertNumber;
    private NumericUpDown numCertDays;
    private Button btnAdd;
    private ListBox lstCourses;
    private Button btnSort;
    private Button btnClone;
    private Button btnExport;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblTitle = new Label();
        lblPrice = new Label();
        lblCertNumber = new Label();
        lblCertDays = new Label();
        txtTitle = new TextBox();
        numPrice = new NumericUpDown();
        txtCertNumber = new TextBox();
        numCertDays = new NumericUpDown();
        btnAdd = new Button();
        lstCourses = new ListBox();
        btnSort = new Button();
        btnClone = new Button();
        btnExport = new Button();
        ((System.ComponentModel.ISupportInitialize)numPrice).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numCertDays).BeginInit();
        SuspendLayout();

        lblTitle.AutoSize = true;
        lblTitle.Location = new Point(12, 15);
        lblTitle.Name = "lblTitle";
        lblTitle.Text = "Название курса:";

        txtTitle.Location = new Point(160, 12);
        txtTitle.Name = "txtTitle";
        txtTitle.Size = new Size(250, 23);

        lblPrice.AutoSize = true;
        lblPrice.Location = new Point(12, 48);
        lblPrice.Name = "lblPrice";
        lblPrice.Text = "Стоимость, руб.:";

        numPrice.DecimalPlaces = 2;
        numPrice.Location = new Point(160, 45);
        numPrice.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        numPrice.Name = "numPrice";
        numPrice.Size = new Size(250, 23);
        numPrice.Value = new decimal(new int[] { 1000, 0, 0, 0 });

        lblCertNumber.AutoSize = true;
        lblCertNumber.Location = new Point(12, 81);
        lblCertNumber.Name = "lblCertNumber";
        lblCertNumber.Text = "Номер сертификата:";

        txtCertNumber.Location = new Point(160, 78);
        txtCertNumber.Name = "txtCertNumber";
        txtCertNumber.Size = new Size(250, 23);

        lblCertDays.AutoSize = true;
        lblCertDays.Location = new Point(12, 114);
        lblCertDays.Name = "lblCertDays";
        lblCertDays.Text = "Срок действия, дн.:";

        numCertDays.Location = new Point(160, 111);
        numCertDays.Maximum = new decimal(new int[] { 3650, 0, 0, 0 });
        numCertDays.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numCertDays.Name = "numCertDays";
        numCertDays.Size = new Size(250, 23);
        numCertDays.Value = new decimal(new int[] { 30, 0, 0, 0 });

        btnAdd.Location = new Point(12, 150);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(398, 30);
        btnAdd.Text = "Добавить курс";
        btnAdd.UseVisualStyleBackColor = true;
        btnAdd.Click += btnAdd_Click;

        lstCourses.FormattingEnabled = true;
        lstCourses.HorizontalScrollbar = true;
        lstCourses.ItemHeight = 15;
        lstCourses.Location = new Point(12, 195);
        lstCourses.Name = "lstCourses";
        lstCourses.Size = new Size(636, 199);

        btnSort.Location = new Point(12, 405);
        btnSort.Name = "btnSort";
        btnSort.Size = new Size(204, 30);
        btnSort.Text = "Сортировать по цене";
        btnSort.UseVisualStyleBackColor = true;
        btnSort.Click += btnSort_Click;

        btnClone.Location = new Point(228, 405);
        btnClone.Name = "btnClone";
        btnClone.Size = new Size(204, 30);
        btnClone.Text = "Клонировать выбранный";
        btnClone.UseVisualStyleBackColor = true;
        btnClone.Click += btnClone_Click;

        btnExport.Location = new Point(444, 405);
        btnExport.Name = "btnExport";
        btnExport.Size = new Size(204, 30);
        btnExport.Text = "Явный экспорт данных";
        btnExport.UseVisualStyleBackColor = true;
        btnExport.Click += btnExport_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(660, 450);
        Controls.Add(lblTitle);
        Controls.Add(txtTitle);
        Controls.Add(lblPrice);
        Controls.Add(numPrice);
        Controls.Add(lblCertNumber);
        Controls.Add(txtCertNumber);
        Controls.Add(lblCertDays);
        Controls.Add(numCertDays);
        Controls.Add(btnAdd);
        Controls.Add(lstCourses);
        Controls.Add(btnSort);
        Controls.Add(btnClone);
        Controls.Add(btnExport);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "FormMain";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Каталог онлайн-курсов";
        ((System.ComponentModel.ISupportInitialize)numPrice).EndInit();
        ((System.ComponentModel.ISupportInitialize)numCertDays).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
