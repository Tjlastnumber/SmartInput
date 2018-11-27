using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartInput
{
    public class Win32Api
    {
        /// <summary>
        /// 获取活动窗口句柄
        /// </summary>
        /// <returns>活动窗口句柄</returns>
        [DllImport("User32.dll")]
        private static extern IntPtr GetForegroundWindow();     

        /// <summary>
        /// 获取窗口进程线程ID
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="ID">窗口线程</param>
        /// <returns>窗口线程句柄</returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetWindowThreadProcessId(IntPtr hwnd, out int ID);   

        /// <summary>
        /// 推送消息指令
        /// </summary>
        /// <param name="hhwnd">目标窗口句柄</param>
        /// <param name="msg">发送消息ID</param>
        /// <param name="wparam"></param>
        /// <param name="lparam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hhwnd, uint msg, IntPtr wparam, IntPtr lparam);

        /// <summary>
        /// 加载指定键盘布局
        /// https://baike.baidu.com/item/LoadKeyboardLayout
        /// </summary>
        /// <param name="pwszKLID">键盘id( 16 进制 8 位 )</param>
        /// <param name="Flags">如何装入键盘布局</param>
        /// <returns>键盘布局句柄</returns>
        [DllImport("user32.dll")]
        private static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);

        private const int WS_EX_NOACTIVATE = 0x08000000;
        private const int GWL_EXSTYLE = -20;
        private static uint WM_INPUTLANGCHANGEREQUEST = 0x0050;
        private static uint KLF_ACTIVATE = 1;

        /// <summary>
        /// 获取当前活动窗口进程ID和句柄
        /// </summary>
        /// <param name="processId">当前活动窗口进程ID</param>
        /// <returns>当前活动窗口句柄</returns>
        public static IntPtr GetForegroundWindowProcessId(out int processId)
        {
            IntPtr hWnd = GetForegroundWindow();
            if (hWnd != IntPtr.Zero)
            {
                GetWindowThreadProcessId(hWnd, out processId);
                return hWnd;
            } else
            {
                processId = 0;
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// 给指定句柄发送消息
        /// </summary>
        /// <param name="hWnd">消息接收句柄</param>
        /// <param name="keyboardLayout">键盘布局类型</param>
        /// <returns>消息是否发送成功</returns>
        public static bool PostMessage(IntPtr hWnd, string keyboardLayout)
        {
            return PostMessage(hWnd, WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, LoadKeyboardLayout(keyboardLayout, KLF_ACTIVATE));
        }
    }
}
