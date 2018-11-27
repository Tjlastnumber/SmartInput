using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartInput
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /*
             * 设置管理员启动 
             */
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
#if DEBUG
            Application.Run(new MainForm());
#else
            if (principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                Application.Run(new MainForm());
            }
            else
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    WorkingDirectory = Environment.CurrentDirectory,
                    FileName = Application.ExecutablePath,
                    Verb = "runas" 
                };
                try
                {
                    Process.Start(startInfo);
                }
                catch 
                {
                    return;
                }
            }
#endif
        }
    }
}
