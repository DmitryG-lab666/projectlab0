using System;
using System.Windows.Forms;

namespace CourseApp;

public class OnlineCourse : IIdentifiable<int>, IExportable, IComparable, ICloneable
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public decimal Price { get; private set; }
    public AccessCertificate Certificate { get; private set; }

    public OnlineCourse(int id, string title, decimal price, AccessCertificate certificate)
    {
        Id = id;
        Title = title;
        Price = price;
        Certificate = certificate;
    }

    string IExportable.GetExportData() =>
        $"[{Id}] Курс: {Title} | Цена: {Price} руб. | Сертификат: №{Certificate.CertNumber} ({Certificate.ValidityDays} дн.)";

    public void Display(ListBox listBox)
    {
        listBox.Items.Add(((IExportable)this).GetExportData());
    }

    public int CompareTo(object obj)
    {
        if (obj == null)
            return 1;

        if (obj is OnlineCourse other)
            return Price.CompareTo(other.Price);

        throw new ArgumentException("Объект должен быть типа OnlineCourse", nameof(obj));
    }

    public object Clone()
    {
        return new OnlineCourse(Id, Title, Price, (AccessCertificate)Certificate.Clone());
    }
}
