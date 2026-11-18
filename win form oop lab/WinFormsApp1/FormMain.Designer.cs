using System.Drawing;
using System.Windows.Forms;

namespace VehicleApp;

partial class FormMain
{
    private System.ComponentModel.IContainer components = null;

    private Label lblType;
    private Label lblModel;
    private Label lblPower;
    private Label lblCapacity;
    private ComboBox cmbVehicleType;
    private TextBox txtModel;
    private NumericUpDown numPower;
    private CheckBox chkIsElectric;
    private NumericUpDown numCapacity;
    private Button btnAdd;
    private ListBox lstFleet;
    private Button btnCalculateTax;
    private Button btnSaveStorable;
    private Label lblTotalTax;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblType = new Label();
        lblModel = new Label();
        lblPower = new Label();
        lblCapacity = new Label();
        cmbVehicleType = new ComboBox();
        txtModel = new TextBox();
        numPower = new NumericUpDown();
        chkIsElectric = new CheckBox();
        numCapacity = new NumericUpDown();
        btnAdd = new Button();
        lstFleet = new ListBox();
        btnCalculateTax = new Button();
        btnSaveStorable = new Button();
        lblTotalTax = new Label();
        ((System.ComponentModel.ISupportInitialize)numPower).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numCapacity).BeginInit();
        SuspendLayout();

        lblType.AutoSize = true;
        lblType.Location = new Point(12, 15);
        lblType.Name = "lblType";
        lblType.Text = "Тип:";

        cmbVehicleType.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbVehicleType.Items.AddRange(new object[] { "Легковой", "Грузовой" });
        cmbVehicleType.Location = new Point(150, 12);
        cmbVehicleType.Name = "cmbVehicleType";
        cmbVehicleType.Size = new Size(200, 23);
        cmbVehicleType.SelectedIndexChanged += cmbVehicleType_SelectedIndexChanged;

        lblModel.AutoSize = true;
        lblModel.Location = new Point(12, 48);
        lblModel.Name = "lblModel";
        lblModel.Text = "Модель:";

        txtModel.Location = new Point(150, 45);
        txtModel.Name = "txtModel";
        txtModel.Size = new Size(200, 23);

        lblPower.AutoSize = true;
        lblPower.Location = new Point(12, 81);
        lblPower.Name = "lblPower";
        lblPower.Text = "Мощность, л.с.:";

        numPower.Location = new Point(150, 78);
        numPower.Maximum = new decimal(new int[] { 2000, 0, 0, 0 });
        numPower.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numPower.Name = "numPower";
        numPower.Size = new Size(200, 23);
        numPower.Value = new decimal(new int[] { 100, 0, 0, 0 });

        chkIsElectric.AutoSize = true;
        chkIsElectric.Location = new Point(150, 110);
        chkIsElectric.Name = "chkIsElectric";
        chkIsElectric.Text = "Электромобиль";

        lblCapacity.AutoSize = true;
        lblCapacity.Location = new Point(12, 142);
        lblCapacity.Name = "lblCapacity";
        lblCapacity.Text = "Грузоподъёмность, т:";

        numCapacity.DecimalPlaces = 1;
        numCapacity.Location = new Point(150, 139);
        numCapacity.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
        numCapacity.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
        numCapacity.Name = "numCapacity";
        numCapacity.Size = new Size(200, 23);
        numCapacity.Value = new decimal(new int[] { 10, 0, 0, 65536 });

        btnAdd.Location = new Point(12, 175);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(338, 30);
        btnAdd.Text = "Добавить в автопарк";
        btnAdd.UseVisualStyleBackColor = true;
        btnAdd.Click += btnAdd_Click;

        lstFleet.FormattingEnabled = true;
        lstFleet.ItemHeight = 15;
        lstFleet.Location = new Point(12, 215);
        lstFleet.Name = "lstFleet";
        lstFleet.Size = new Size(496, 154);

        btnCalculateTax.Location = new Point(12, 380);
        btnCalculateTax.Name = "btnCalculateTax";
        btnCalculateTax.Size = new Size(240, 30);
        btnCalculateTax.Text = "Рассчитать налог";
        btnCalculateTax.UseVisualStyleBackColor = true;
        btnCalculateTax.Click += btnCalculateTax_Click;

        btnSaveStorable.Location = new Point(268, 380);
        btnSaveStorable.Name = "btnSaveStorable";
        btnSaveStorable.Size = new Size(240, 30);
        btnSaveStorable.Text = "Сохранить IStorable";
        btnSaveStorable.UseVisualStyleBackColor = true;
        btnSaveStorable.Click += btnSaveStorable_Click;

        lblTotalTax.AutoSize = true;
        lblTotalTax.Location = new Point(12, 425);
        lblTotalTax.Name = "lblTotalTax";
        lblTotalTax.Text = "Суммарный налог: 0,00 руб.";

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(520, 460);
        Controls.Add(lblType);
        Controls.Add(cmbVehicleType);
        Controls.Add(lblModel);
        Controls.Add(txtModel);
        Controls.Add(lblPower);
        Controls.Add(numPower);
        Controls.Add(chkIsElectric);
        Controls.Add(lblCapacity);
        Controls.Add(numCapacity);
        Controls.Add(btnAdd);
        Controls.Add(lstFleet);
        Controls.Add(btnCalculateTax);
        Controls.Add(btnSaveStorable);
        Controls.Add(lblTotalTax);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "FormMain";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Автопарк";
        ((System.ComponentModel.ISupportInitialize)numPower).EndInit();
        ((System.ComponentModel.ISupportInitialize)numCapacity).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
