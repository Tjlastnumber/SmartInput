using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartIME
{
    class Program
    {
        [DllImport("User32.dll")]
        public static extern IntPtr GetForegroundWindow();     //获取活动窗口句柄

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);   //获取线程ID

        static void Main(string[] args)
        {
            int currentProcessId = 0;

            while(true)
            {
                IntPtr hWnd = GetForegroundWindow();    //获取活动窗口句柄
                GetWindowThreadProcessId(hWnd, out int calcID);
                Process myProcess = Process.GetProcessById(calcID);
                if (calcID != currentProcessId)
                {
                    currentProcessId = calcID;
                    Console.WriteLine("进程名：" + myProcess.ProcessName + "\n" + "进程ID：" + calcID);
                }
                Thread.Sleep(1000);
            }
        }
    }
}
