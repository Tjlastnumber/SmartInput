using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IME_Test
{
    public partial class Form2 : Form
    {
        private string _CurrentImeHandleStr = "";
        public delegate bool EnumResNameProc(IntPtr hModule, IntPtr nType, StringBuilder sName, IntPtr lParam);
        System.ComponentModel.ComponentResourceManager resources = new ComponentResourceManager(typeof(Form2));
        public Form2()
        {
            InitializeComponent();
        }
        #region API定义
        
       
        private static readonly int BTN_HEIGHT = 21;
        private static readonly int IMAGE_ICON = 1;
        private const int DONT_RESOLVE_DLL_REFERENCES = 0x1;
        private const int LOAD_LIBRARY_AS_DATAFILE = 0x2;
        private const int LOAD_WITH_ALTERED_SEARCH_PATH = 0x8;
        private const int RT_ICON = 0x3;
        private const int RT_BITMAP = 0x2;
        private const int RT_GROUP_ICON = (RT_ICON + 11);

      //API定义
        [DllImport("Kernel32.dll")]
        public extern static bool FreeLibrary(IntPtr hModule);

        [DllImport("user32.dll")]
        public extern static IntPtr LoadIcon(IntPtr hInstance, string iID);
        /// <summary>
        /// 得到输入法说明
        /// </summary>
        /// <param name="Hkl"></param>
        /// <param name="sName"></param>
        /// <param name="nBuffer"></param>
        /// <returns></returns>
        [DllImport("Imm32.dll")]
        public extern static int ImmGetDescription(IntPtr Hkl, StringBuilder sName, int nBuffer);
        /// <summary>
        /// 得到输入法的文件名
        /// </summary>
        /// <param name="Hkl"></param>
        /// <param name="sFileName"></param>
        /// <param name="nBuffer"></param>
        /// <returns></returns>
        [DllImport("Imm32.dll")]
        public extern static int ImmGetIMEFileName(IntPtr Hkl, StringBuilder sFileName, int nBuffer);

        [DllImport("Kernel32.dll")]
        public extern static IntPtr LoadLibraryEx(string sFileName, IntPtr hFile, int dwFlags);

        [DllImport("Kernel32.dll")]
        public extern static bool EnumResourceNames(IntPtr hModule, IntPtr nType, EnumResNameProc lpEnumFunc, int lParam);

        [DllImport("shell32.dll")]
        public extern static IntPtr ExtractIcon(IntPtr hInstance, string sExeFileName, int nIconIndex);

        [DllImport("user32.dll")]
        public extern static IntPtr LoadImage(IntPtr hInstance, string sID, int nType, int cx, int cy, int fuLoad);
        #endregion

        private void Form17_Load(object sender, EventArgs e)
        {
            //初始化菜单
            InitMenus();
        }

        void Application_Idle(object sender, EventArgs e)
        {
            if (this._CurrentImeHandleStr == Application.CurrentInputLanguage.Handle.ToString())
                return;

            //显示新的输入法
            ChangeIme(Application.CurrentInputLanguage.Handle);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        { 
          this.contextMenuStrip1.Show(this.toolStrip1, new Point(0, 0), ToolStripDropDownDirection.AboveRight);
        }
        /// <summary>
        /// 初始化菜单
        /// </summary>
        private void InitMenus()
        {
            this.contextMenuStrip1.Items.Clear();
            string sLayoutName = "";

            foreach (InputLanguage item in InputLanguage.InstalledInputLanguages)
            {
                sLayoutName = GetImmDescription(item);//item.LayoutName; //
                if (string.IsNullOrEmpty(sLayoutName))
                {
                    continue;
                }
                ToolStripMenuItem oMenuItem = new ToolStripMenuItem();
                oMenuItem.Checked = (item.Handle.ToString() == InputLanguage.CurrentInputLanguage.Handle.ToString());


                oMenuItem.Text = sLayoutName;
                oMenuItem.ToolTipText = sLayoutName;
                oMenuItem.Click += new EventHandler(oMenuItem_Click);
                oMenuItem.Tag = item;
                oMenuItem.Image = GetImeBitmap(item);
                this.contextMenuStrip1.Items.Add(oMenuItem);
            }
        }

        /// <summary>
        /// 得到指定输入法的说明
        /// </summary>
        /// <param name="hKl"></param>
        /// <returns></returns>
        private string GetImmDescription(InputLanguage inpt)
        {
            int nBuffer = 0;

            StringBuilder sName = new StringBuilder();
            string sDesc = "";

            nBuffer = ImmGetDescription(inpt.Handle, null, nBuffer);
            sName = new StringBuilder(nBuffer);
            ImmGetDescription(inpt.Handle, sName, nBuffer);
            sDesc = sName.ToString();
            if (string.IsNullOrEmpty(sDesc))
            {
                sDesc = inpt.LayoutName;
            }
      
            return sDesc;
        }

        /// <summary>
        /// 单击输入法事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void oMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem oItem = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem item in this.contextMenuStrip1.Items)
            {
                item.CheckState = CheckState.Unchecked;
            }
            oItem.CheckState = CheckState.Checked;

            Application.CurrentInputLanguage = ((InputLanguage)oItem.Tag);
            InputLanauageChangedUI();
        }


        /// <summary>
        /// 得到指定输入法的图标
        /// </summary>
        /// <param name="ime"></param>
        /// <returns></returns>
        private Image GetImeBitmap(InputLanguage ime)
        {
            int nBuffer = 0;
            StringBuilder sName;
            Image oBitmap = null;

            //得到IME文件
            nBuffer = ImmGetIMEFileName(ime.Handle, null, nBuffer);
            sName = new StringBuilder(nBuffer);
            ImmGetIMEFileName(ime.Handle, sName, nBuffer);

            if (string.IsNullOrEmpty(sName.ToString()))
            {
                return Properties.Resources.input;
   
            }
            else
            {
                //从资源文件中得到图标
                string sFileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), sName.ToString());
                if (File.Exists(sFileName))
                {
                    oBitmap = GetBitmapFromResource(sFileName, "");
                }
                if (oBitmap == null)
                {
                    oBitmap = Properties.Resources.input;
                }
                return oBitmap;
            }
        }
        private Image GetBitmapFromResource(string sFileName, string sBitmapFlag)
        {
            Bitmap oBitmap = null;
            IntPtr hModule = LoadLibraryEx(sFileName, IntPtr.Zero, LOAD_LIBRARY_AS_DATAFILE);
            if (hModule == IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine("未能成功加载" + sFileName);
                return null;
            }
            string sName = "IMEICO";
            IntPtr hIcon = IntPtr.Zero;

            System.Diagnostics.Debug.WriteLine("正在获取" + sFileName + "中所有图标。");

         
            hIcon = ExtractIcon(this.Handle, sFileName, 0);
 
            if (hIcon == IntPtr.Zero)
            {
                sName = "#101";
                hIcon = LoadImage(hModule, sName, IMAGE_ICON, 16, 16, 0);
            }

            if (hIcon != IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Hicon:{0}", hIcon.ToString()));
                oBitmap = Icon.FromHandle(hIcon).ToBitmap();
    
            }
         
            EnumResourceNames(hModule, this.MAKEINTRESOURCE(RT_GROUP_ICON), this.EnumIconResourceProc, 0);
            //释放
            FreeLibrary(hModule);
            return oBitmap;
        }
        private IntPtr MAKEINTRESOURCE(int nID)
        {
            return new IntPtr((long)((short)nID));
        }
        private bool EnumIconResourceProc(IntPtr hModule, IntPtr nType, StringBuilder sName, IntPtr lParam)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("得到的资源名称：{0}", sName));
            //得到图标
            IntPtr hIcon = LoadIcon(hModule, sName.ToString());
            Icon icon = Icon.FromHandle(hIcon);

            return true;
        }
        private void Form17_InputLanguageChanged(object sender, InputLanguageChangedEventArgs e)
        {
            Application.CurrentInputLanguage = e.InputLanguage;
            this.ChangeIme(e.InputLanguage.Handle);

        }
        /// <summary>
        /// 改变输入法函数
        /// </summary>
        /// <param name="handle"></param>
        private void ChangeIme(IntPtr handle)
        {
            this._CurrentImeHandleStr = handle.ToString();
          
            //改变输入法的状态
            foreach (ToolStripMenuItem item in this.contextMenuStrip1.Items)
            {
                if (((InputLanguage)item.Tag).Handle.ToString() == handle.ToString())
                {
                    item.CheckState = CheckState.Checked;
                }
                else
                {
                    item.CheckState = CheckState.Unchecked;
                }
            }
            InputLanauageChangedUI();
        }

        /// <summary>
        /// 输入法改变时界面的变化
        /// </summary>
        private void InputLanauageChangedUI()
        {
            //改变相应的图标
            foreach (ToolStripMenuItem item in this.contextMenuStrip1.Items)
            {
                if (item.CheckState == CheckState.Checked)
                {
                    this.ToolBtn.Image = item.Image;
                    this.ToolBtn.ToolTipText = item.Text;
                  
                }
            }

            //重新设置组件的大小
            this.toolStrip1.Height = BTN_HEIGHT;
       
        }
    }
}
