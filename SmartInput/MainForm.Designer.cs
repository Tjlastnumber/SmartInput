﻿namespace SmartInput
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btn_OK = new System.Windows.Forms.Button();
            this.dgv_Process = new System.Windows.Forms.DataGridView();
            this.ProcessIcon = new System.Windows.Forms.DataGridViewImageColumn();
            this.ProcessName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_inputLanguage = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RunStart = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_btn_Setting = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Tmi_Quit = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Timer_Process = new System.Windows.Forms.Timer(this.components);
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Process)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_OK
            // 
            this.btn_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_OK.Location = new System.Drawing.Point(658, 22);
            this.btn_OK.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(125, 38);
            this.btn_OK.TabIndex = 2;
            this.btn_OK.Text = "确认";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.Btn_OK_Click);
            // 
            // dgv_Process
            // 
            this.dgv_Process.AllowUserToAddRows = false;
            this.dgv_Process.AllowUserToDeleteRows = false;
            this.dgv_Process.AutoGenerateColumns = false;
            this.dgv_Process.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_Process.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Process.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProcessIcon,
            this.ProcessName,
            this.dgv_inputLanguage});
            this.dgv_Process.DataSource = this.bindingSource;
            this.dgv_Process.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_Process.Location = new System.Drawing.Point(0, 0);
            this.dgv_Process.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.dgv_Process.Name = "dgv_Process";
            this.dgv_Process.RowTemplate.Height = 23;
            this.dgv_Process.Size = new System.Drawing.Size(803, 535);
            this.dgv_Process.TabIndex = 3;
            this.dgv_Process.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellClick);
            this.dgv_Process.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellEndEdit);
            // 
            // ProcessIcon
            // 
            this.ProcessIcon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ProcessIcon.DataPropertyName = "Icon";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle2.NullValue")));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(2);
            this.ProcessIcon.DefaultCellStyle = dataGridViewCellStyle2;
            this.ProcessIcon.FillWeight = 38.77094F;
            this.ProcessIcon.HeaderText = "";
            this.ProcessIcon.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.ProcessIcon.Name = "ProcessIcon";
            this.ProcessIcon.ReadOnly = true;
            this.ProcessIcon.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ProcessIcon.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ProcessIcon.Width = 35;
            // 
            // ProcessName
            // 
            this.ProcessName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ProcessName.DataPropertyName = "ProcessName";
            this.ProcessName.HeaderText = "进程名";
            this.ProcessName.Name = "ProcessName";
            this.ProcessName.ReadOnly = true;
            this.ProcessName.Width = 150;
            // 
            // dgv_inputLanguage
            // 
            this.dgv_inputLanguage.DataPropertyName = "LanguageCode";
            this.dgv_inputLanguage.HeaderText = "输入法";
            this.dgv_inputLanguage.Items.AddRange(new object[] {
            ""});
            this.dgv_inputLanguage.Name = "dgv_inputLanguage";
            this.dgv_inputLanguage.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_inputLanguage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "SmartInput";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(27, 27);
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RunStart,
            this.cms_btn_Setting,
            this.toolStripSeparator1,
            this.Tmi_Quit});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(169, 106);
            // 
            // RunStart
            // 
            this.RunStart.CheckOnClick = true;
            this.RunStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.RunStart.Name = "RunStart";
            this.RunStart.Size = new System.Drawing.Size(168, 32);
            this.RunStart.Text = "开机启动";
            this.RunStart.CheckedChanged += new System.EventHandler(this.RunStart_CheckedChanged);
            // 
            // cms_btn_Setting
            // 
            this.cms_btn_Setting.Name = "cms_btn_Setting";
            this.cms_btn_Setting.Size = new System.Drawing.Size(168, 32);
            this.cms_btn_Setting.Text = "设置";
            this.cms_btn_Setting.Click += new System.EventHandler(this.Cms_btn_Setting_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(165, 6);
            // 
            // Tmi_Quit
            // 
            this.Tmi_Quit.Name = "Tmi_Quit";
            this.Tmi_Quit.Size = new System.Drawing.Size(168, 32);
            this.Tmi_Quit.Text = "退出";
            this.Tmi_Quit.Click += new System.EventHandler(this.Tmi_Quit_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_OK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 535);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(803, 80);
            this.panel1.TabIndex = 4;
            // 
            // Timer_Process
            // 
            this.Timer_Process.Enabled = true;
            this.Timer_Process.Interval = 1000;
            this.Timer_Process.Tick += new System.EventHandler(this.Timer_Process_Tick);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btn_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_OK;
            this.ClientSize = new System.Drawing.Size(803, 615);
            this.Controls.Add(this.dgv_Process);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SmartInput";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Process)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.DataGridView dgv_Process;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.BindingSource bindingSource;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem cms_btn_Setting;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem Tmi_Quit;
        private System.Windows.Forms.Timer Timer_Process;
        private System.Windows.Forms.ToolStripMenuItem RunStart;
        private System.Windows.Forms.DataGridViewImageColumn ProcessIcon;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProcessName;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgv_inputLanguage;
    }
}

