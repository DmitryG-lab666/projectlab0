using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace VehicleApp;

public partial class FormMain : Form
{
    private readonly List<Vehicle> fleet = new List<Vehicle>();

    public FormMain()
    {
        InitializeComponent();
        cmbVehicleType.SelectedIndex = 0;
    }

    private void cmbVehicleType_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool isCar = cmbVehicleType.SelectedIndex == 0;
        chkIsElectric.Enabled = isCar;
        numCapacity.Enabled = !isCar;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string model = txtModel.Text.Trim();
            int power = (int)numPower.Value;

            Vehicle vehicle = cmbVehicleType.SelectedIndex == 0
                ? new Car(model, power, chkIsElectric.Checked)
                : new Truck(model, power, (double)numCapacity.Value);

            fleet.Add(vehicle);
            lstFleet.Items.Add(vehicle.GetInfo());
            txtModel.Clear();
            txtModel.Focus();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void btnCalculateTax_Click(object sender, EventArgs e)
    {
        double total = 0;

        foreach (Vehicle vehicle in fleet)
            total += vehicle.CalculateTax();

        lblTotalTax.Text = $"Суммарный налог: {total:N2} руб.";
    }

    private void btnSaveStorable_Click(object sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog();
        if (dialog.ShowDialog() != DialogResult.OK)
            return;

        int saved = 0;

        foreach (Vehicle v in fleet)
        {
            if (v is IStorable storable)
            {
                saved++;
                storable.Save(Path.Combine(dialog.SelectedPath, $"truck_{saved}.txt"));
            }
        }

        MessageBox.Show($"Сохранено объектов: {saved}", "Сохранение");
    }
}
