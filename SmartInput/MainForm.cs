using SmartInput.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartInput
{
    public partial class MainForm : Form
    {
        private readonly string currentProcessName;

        private bool canClose = false;
        private bool canRefresh = false;

        private Dictionary<string, string> languageDict = new Dictionary<string, string>();
        private List<ProcessModel> processCache = new List<ProcessModel>();

        public MainForm()
        {
            InitializeComponent();
            currentProcessName = Process.GetCurrentProcess().ProcessName;

            RunStart.Checked = Program.IsRunStart();
            dgv_Process.AutoGenerateColumns = false;
            dgv_inputLanguage.DisplayMember = nameof(InputLanguage.LayoutName);
            dgv_inputLanguage.ValueMember = nameof(InputLanguage.Culture);

            LoadConfig();
            LoadInputLanguage();
            LoadProcesses();
        }

        private void LoadInputLanguage()
        {
            DataGridViewComboBoxColumn ilColumn = dgv_inputLanguage;
            foreach (InputLanguage item in InputLanguage.InstalledInputLanguages)
            {
                ilColumn.Items.Add(item);
            }
        }

        private void LoadConfig()
        {
            var config = FileHelper.ReadJosn<Dictionary<string, string>>();
            languageDict = config ?? new Dictionary<string, string>();
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

        private Task<List<ProcessModel>> AsyncGetProcess()
        {
            return Task.Factory.StartNew(() =>
            {
                var processes = from p in Process.GetProcesses()
                                where p.MainWindowHandle != IntPtr.Zero && p.MainWindowTitle.Length > 0 && p.ProcessName != currentProcessName
                                select new ProcessModel
                                {
                                    ProcessName = p.ProcessName,
                                    Icon = TryGetProcessIcon(p),
                                    LanguageCode = TryGetInputCode(p)
                                };
                return processes.ToList();
            });
        }

        private async void LoadProcesses()
        {
            var process = await AsyncGetProcess();
            if (!process.SequenceEqual(processCache))
            {
                processCache = process;
                bindingSource.DataSource = processCache;
                process = null;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Task.Factory.StartNew(() =>
            {
                int currentProcessId = 0;
                for (; ; )
                {
                    IntPtr hWnd = Win32Api.GetForegroundWindowProcessId(out int calcID);
                    if (hWnd != IntPtr.Zero)
                    {
                        if (calcID != currentProcessId)
                        {
                            currentProcessId = calcID;
                            Process myProcess = Process.GetProcessById(calcID);
                            if (languageDict.TryGetValue(myProcess.ProcessName, out string keyId))
                            {
                                Win32Api.PostMessage(hWnd, keyId);
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
            string processName = ((ProcessModel)bindingSource[e.RowIndex]).ProcessName;
            if (cb.Value is CultureInfo culture && culture.KeyboardLayoutId != 0)
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

        private void RunStart_CheckedChanged(object sender, EventArgs e)
        {
            if (!(RunStart.Checked ^ Program.IsRunStart())) return;
            if (Program.RunWhenStart(RunStart.Checked))
            {
                Settings.Default.RunStart = RunStart.Checked;
                Settings.Default.Save();
            }
            else
            {
                RunStart.Checked = !RunStart.Checked;
            }
        }
    }

    public class ProcessModel : IEquatable<ProcessModel>, IDisposable
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

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool flag)
        {
            if (flag)
            {
                GC.SuppressFinalize(this);
            }
            else
            {
                icon = null;
                icon.Dispose();
            }
        }
    }
}
