using SmartInput;
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
    public partial class MainForm : Form
    {
        [DllImport("User32.dll")]
        private static extern IntPtr GetForegroundWindow();     //获取活动窗口句柄

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetWindowThreadProcessId(IntPtr hwnd, out int ID);   //获取线程ID

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hhwnd, uint msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32.dll")]
        private static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);

        private Dictionary<string, string> languageDict = new Dictionary<string, string>();
        private const int WS_EX_NOACTIVATE = 0x08000000;
        private const int GWL_EXSTYLE = -20;
        private static uint WM_INPUTLANGCHANGEREQUEST = 0x0050;
        private static uint KLF_ACTIVATE = 1;
        private bool canClose = false;
        private bool canRefresh = false;
        private List<ProcessModel> processCache = new List<ProcessModel>();

        public MainForm()
        {
            InitializeComponent();
            dgv_Process.AutoGenerateColumns = false;
            data_inputLanguage.DisplayMember = nameof(InputLanguage.LayoutName);
            data_inputLanguage.ValueMember = nameof(InputLanguage.Culture);

            LoadConfig();
            LoadProcesses();
            LoadInputLanguage();
        }

        private void LoadInputLanguage()
        {
            DataGridViewComboBoxColumn ilColumn = data_inputLanguage;
            foreach (InputLanguage item in InputLanguage.InstalledInputLanguages)
            {
                ilColumn.Items.Add(item);
            }
        }

        private async void LoadConfig()
        {
            languageDict = await FileHelper.AsyncReadJosn<Dictionary<string, string>>();
        }

        private Task<List<ProcessModel>> AsyncGetProcess()
        {
            return Task.Run(() =>
            {
                Process[] ps = Process.GetProcesses();
                var processes = from p in ps
                                where p.MainWindowHandle != IntPtr.Zero && p.MainWindowTitle.Length > 0
                                select new ProcessModel
                                {
                                    ProcessName = p.ProcessName,
                                    Icon = TryGetProcessIcon(p),
                                    LanguageCode = TryGetInputCode(p)
                                };
                return processes.ToList();
            });
        }

        private Icon TryGetProcessIcon(Process p)
        {
            try
            {
                return Icon.ExtractAssociatedIcon(p.MainModule.FileName);
            }
            catch
            {
                return null;
            }
        }

        private CultureInfo TryGetInputCode(Process p)
        {
            if (languageDict.TryGetValue(p.ProcessName, out string inputId))
            {
                int id = Convert.ToInt32(inputId, 16);
                return CultureInfo.GetCultureInfo(id);
            }
            else
            {
                return null;
            }
        }

        private async void LoadProcesses()
        {
            var process = await AsyncGetProcess();
            if (!process.SequenceEqual(processCache))
            {
                processCache = process;
                bindingSource.DataSource = processCache;
            }
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
                            if (languageDict.TryGetValue(myProcess.ProcessName, out string keyId))
                            {
                                PostMessage(hWnd, WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, LoadKeyboardLayout(keyId, KLF_ACTIVATE));
                            }
                        }
                    }
                    Thread.Sleep(500);
                }
            });
        }

        private void Timer_Process_Tick(object sender, EventArgs e)
        {
            if (!canRefresh) return;
            LoadProcesses();
        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            dgv_Process.EndEdit();
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Exit();
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            dgv_Process.BeginEdit(true);
            if (dgv_Process.EditingControl is ComboBox cb) cb.DroppedDown = true;
        }

        private void DataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var cb = dgv_Process[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;
            CultureInfo culture = cb.Value as CultureInfo;
            string processName = ((ProcessModel)bindingSource[e.RowIndex]).ProcessName;
            if (culture != null && culture.KeyboardLayoutId != 0)
            {
                languageDict[processName] = culture.KeyboardLayoutId.ToString("x8");
            }
            FileHelper.SaveJosn(languageDict);
        }

        private void ShowWindow()
        {
            ShowInTaskbar = true;
            canRefresh = true;
            TopMost = true;
            Activate();
            Show();
            TopMost = false;
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
            ShowWindow();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ShowInTaskbar = false;
            canRefresh = false;
            e.Cancel = e.CloseReason == CloseReason.UserClosing && !canClose;
            Hide();
        }

        private void Cms_btn_Setting_Click(object sender, EventArgs e)
        {
            ShowWindow();
        }

        private void Tmi_Quit_Click(object sender, EventArgs e)
        {
            canClose = true;
            Close();
        }

    }

    public class ProcessModel : IEquatable<ProcessModel>
    {
        public string ProcessName { get; set; }
        private Icon icon;
        public Icon Icon
        {
            get
            {
                return icon;
            }
            set
            {
                icon = value != null ? new Icon(value, new Size(32, 32)) : null;
            }
        }

        public CultureInfo LanguageCode { get; set; }

        public bool Equals(ProcessModel other)
        {
            if (ReferenceEquals(other, null)) return false;

            if (ReferenceEquals(this, other)) return true;

            return ProcessName.Equals(other.ProcessName);
        }

        public override int GetHashCode()
        {
            int hashProcessName = ProcessName.GetHashCode();

            return hashProcessName;
        }
    }
}
