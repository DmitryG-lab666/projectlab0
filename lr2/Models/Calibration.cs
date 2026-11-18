namespace SmartHomeMasterConsole.Models;

public class Calibration
{
    public string Serial { get; set; }
    public DateTime CalibratedOn { get; set; }
    public double Offset { get; set; }

    public Calibration(string serial, DateTime calibratedOn, double offset)
    {
        Serial = serial;
        CalibratedOn = calibratedOn;
        Offset = offset;
    }

    public Calibration Clone() => new Calibration(Serial, CalibratedOn, Offset);

    public override string ToString() => $"{Serial} от {CalibratedOn:dd.MM.yyyy}, смещение {Offset:F2}";
}
