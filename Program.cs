using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TVSharp
{
    // Token: 0x02000007 RID: 7
    internal static class Program
    {
        // Token: 0x06000033 RID: 51 RVA: 0x000023CC File Offset: 0x000005CC
        [STAThread]
        private static void Main()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32Windows || Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                Process currentProcess = Process.GetCurrentProcess();
                currentProcess.PriorityBoostEnabled = true;
                currentProcess.PriorityClass = ProcessPriorityClass.RealTime;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
