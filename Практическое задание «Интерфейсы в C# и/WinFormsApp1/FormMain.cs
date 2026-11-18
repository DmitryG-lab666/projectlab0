using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CourseApp;

public partial class FormMain : Form
{
    private List<OnlineCourse> courses = new List<OnlineCourse>();
    private int nextId = 1;

    public FormMain()
    {
        InitializeComponent();
    }

    private void RefreshList()
    {
        lstCourses.Items.Clear();

        foreach (OnlineCourse course in courses)
            course.Display(lstCourses);
    }

    private OnlineCourse GetSelectedCourse()
    {
        int index = lstCourses.SelectedIndex;

        if (index < 0 || index >= courses.Count)
        {
            MessageBox.Show("Выберите курс в списке.", "Нет выбора", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;
        }

        return courses[index];
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        string title = txtTitle.Text.Trim();
        string certNumber = txtCertNumber.Text.Trim();

        if (title.Length == 0 || certNumber.Length == 0)
        {
            MessageBox.Show("Введите название курса и номер сертификата.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var certificate = new AccessCertificate(certNumber, (int)numCertDays.Value);
        var course = new OnlineCourse(nextId++, title, numPrice.Value, certificate);

        courses.Add(course);
        RefreshList();

        txtTitle.Clear();
        txtCertNumber.Clear();
        txtTitle.Focus();
    }

    private void btnSort_Click(object sender, EventArgs e)
    {
        courses.Sort();
        RefreshList();
    }

    private void btnClone_Click(object sender, EventArgs e)
    {
        OnlineCourse selected = GetSelectedCourse();
        if (selected == null)
            return;

        var clone = (OnlineCourse)selected.Clone();
        clone.Certificate.CertNumber = "_CLONE" + clone.Certificate.CertNumber;

        courses.Add(clone);
        RefreshList();

        string report = $"Оригинал: №{selected.Certificate.CertNumber}\nКлон: №{clone.Certificate.CertNumber}";
        Console.WriteLine(report);
        MessageBox.Show(report, "Глубокое копирование");
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
        OnlineCourse selected = GetSelectedCourse();
        if (selected == null)
            return;

        if (selected is IExportable exportable)
            MessageBox.Show(exportable.GetExportData(), "Экспорт данных");
    }
}
