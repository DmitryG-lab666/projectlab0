using System;
using System.Windows.Forms;

namespace WarehouseApp;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        DatabaseInitializer.Initialize();
        Application.Run(new FormMain());
    }
}
