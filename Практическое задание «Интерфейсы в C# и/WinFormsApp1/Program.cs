using System;
using System.Windows.Forms;

namespace CourseApp;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new FormMain());
    }
}
