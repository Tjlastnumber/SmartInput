using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IME_Test
{
    public partial class Form1 : Form
    {
        [DllImport("User32.dll")]
        private static extern IntPtr GetForegroundWindow();     //获取活动窗口句柄

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetWindowThreadProcessId(IntPtr hwnd, out int ID);   //获取线程ID

        [DllImport("User32.dll")]
        private static extern int GetKeyboardLayout(IntPtr threadId);

        [DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
        private static extern int ImmGetCompositionStringW(IntPtr hIMC, int dwIndex, byte[] lpBuf, int dwBufLen);

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hhwnd, uint msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32.dll")]
        private static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);

        private Dictionary<string, string> inputDict = new Dictionary<string, string>();
        private InputLanguage currentLanguage;
        private Process currentProcess;
        private InputLanguage globalInputLanguage;
        private const int WS_EX_NOACTIVATE = 0x08000000;
        private const int GWL_EXSTYLE = -20;
        private static uint WM_INPUTLANGCHANGEREQUEST = 0x0050;
        private static uint KLF_ACTIVATE = 1;

        public Form1()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            bs.DataSource = GetProcesses();

            DataGridViewComboBoxColumn ilColumn = dataGridView1.Columns["data_inputLanguage"] as DataGridViewComboBoxColumn;
            foreach (InputLanguage item in InputLanguage.InstalledInputLanguages)
            {
                ilColumn.Items.Add(item);
            }

            ilColumn.DisplayMember = "LayoutName";
            ilColumn.ValueMember = "Culture";
        }

        private List<Process> GetProcesses()
        {
            Process[] ps = Process.GetProcesses();
            List<Process> processDict = new List<Process>();

            var data = from p in ps
                       where p.MainWindowHandle != IntPtr.Zero && p.MainWindowTitle.Length > 0
                       select p;

            return data.ToList();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Task.Run(() =>
            {
                int currentProcessId = 0;
                for (; ; )
                {
                    IntPtr hWnd = GetForegroundWindow();
                    if (hWnd != IntPtr.Zero)
                    {
                        IntPtr processId = GetWindowThreadProcessId(hWnd, out int calcID);
                        if (calcID != currentProcessId)
                        {
                            currentProcessId = calcID;
                            Process myProcess = Process.GetProcessById(calcID);
                            if (inputDict.TryGetValue(myProcess.ProcessName, out string keyId))
                            {
                                PostMessage(hWnd, WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, LoadKeyboardLayout(keyId, KLF_ACTIVATE));
                            }
                        }
                    }
                    Thread.Sleep(500);
                }
            });

            //Task.Run(() =>
            //{
            //    for (; ; )
            //    {
            //        Thread.Sleep(1000);
            //        dataGridView1.BeginInvoke(new MethodInvoker(() =>
            //        {
            //            bs.DataSource = GetProcesses();
            //        }));
            //    }
            //});
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();
        }


        protected override void OnClosed(EventArgs e)
        {
            Application.Exit();
            base.OnClosed(e);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            dataGridView1.BeginEdit(true);
            ComboBox cb = dataGridView1.EditingControl as ComboBox;
            if (cb != null) cb.DroppedDown = true;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var cb = dataGridView1[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;
            CultureInfo culture = cb.Value as CultureInfo;
            string processName = ((Process)bs[e.RowIndex]).ProcessName;
            inputDict[processName] = culture?.KeyboardLayoutId.ToString("x8");
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
