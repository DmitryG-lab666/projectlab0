using System;

namespace CourseApp;

public class AccessCertificate : ICloneable
{
    public string CertNumber { get; set; }
    public int ValidityDays { get; set; }

    public AccessCertificate(string certNumber, int validityDays)
    {
        CertNumber = certNumber;
        ValidityDays = validityDays;
    }

    public AccessCertificate ShallowCopy() => (AccessCertificate)MemberwiseClone();

    public object Clone() => new AccessCertificate(CertNumber, ValidityDays);
}
