namespace ClinicCrm;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        SQLitePCL.Batteries.Init();
        ApplicationConfiguration.Initialize();
        Application.Run(new FormMain());
    }
}
