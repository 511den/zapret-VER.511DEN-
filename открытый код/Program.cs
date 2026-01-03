using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

namespace ZapretApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Перезапуск с правами администратора, если не запущено от admin
            if (!IsRunningAsAdmin())
            {
                var psi = new ProcessStartInfo
                {
                    FileName = Application.ExecutablePath,
                    UseShellExecute = true,
                    Verb = "runas" // повышенные права
                };
                try
                {
                    Process.Start(psi);
                }
                catch
                {
                    return; // пользователь отказался
                }
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static bool IsRunningAsAdmin()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
