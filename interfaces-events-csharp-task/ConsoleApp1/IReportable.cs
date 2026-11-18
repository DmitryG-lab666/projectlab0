using System;

public interface IReportable
{
    DateTime CreatedAt { get; }

    string GetReportInfo();
}
