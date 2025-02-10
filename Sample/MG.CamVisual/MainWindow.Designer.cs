namespace MG.CamVisual
{
    partial class MainWindow
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menubtn_about = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.BrandCombox = new System.Windows.Forms.ComboBox();
            this.SNCombox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Btn_SearchCam = new System.Windows.Forms.Button();
            this.Btn_SaveImage = new System.Windows.Forms.Button();
            this.Btn_ReadImage = new System.Windows.Forms.Button();
            this.DisplayBox = new System.Windows.Forms.PictureBox();
            this.DisplayPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Rbtn_TriggerModel = new System.Windows.Forms.RadioButton();
            this.Rbtn_ContinueModel = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.Combobox_HardSource_Callback = new System.Windows.Forms.ComboBox();
            this.Combobox_HardSource = new System.Windows.Forms.ComboBox();
            this.Rbtn_Trigger_Hard = new System.Windows.Forms.RadioButton();
            this.Rbtn_Trigger_HardCallback = new System.Windows.Forms.RadioButton();
            this.Rbtn_Trigger_SoftCallback = new System.Windows.Forms.RadioButton();
            this.Rbtn_Trigger_Soft = new System.Windows.Forms.RadioButton();
            this.Btn_SoftTrigger_Callback = new System.Windows.Forms.Button();
            this.Btn_SoftTrigger = new System.Windows.Forms.Button();
            this.Btn_Startup = new System.Windows.Forms.Button();
            this.Btn_Destroy = new System.Windows.Forms.Button();
            this.Btn_AutoWhiteBlance = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Tbox_Gain = new System.Windows.Forms.TextBox();
            this.Tbox_ExpouseTime = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Tbox_TriggerFliter = new System.Windows.Forms.TextBox();
            this.cmbox_TriggerPolarity = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Btn_CamInit = new System.Windows.Forms.Button();
            this.Tbox_Log = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayBox)).BeginInit();
            this.DisplayPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menubtn_about});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1364, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menubtn_about
            // 
            this.menubtn_about.Name = "menubtn_about";
            this.menubtn_about.Size = new System.Drawing.Size(44, 21);
            this.menubtn_about.Text = "关于";
            this.menubtn_about.Click += new System.EventHandler(this.menubtn_about_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "品牌:";
            // 
            // BrandCombox
            // 
            this.BrandCombox.FormattingEnabled = true;
            this.BrandCombox.Items.AddRange(new object[] {
            "HIK",
            "Basler",
            "DaHeng"});
            this.BrandCombox.Location = new System.Drawing.Point(38, 14);
            this.BrandCombox.Name = "BrandCombox";
            this.BrandCombox.Size = new System.Drawing.Size(121, 20);
            this.BrandCombox.TabIndex = 3;
            // 
            // SNCombox
            // 
            this.SNCombox.FormattingEnabled = true;
            this.SNCombox.Location = new System.Drawing.Point(194, 15);
            this.SNCombox.Name = "SNCombox";
            this.SNCombox.Size = new System.Drawing.Size(121, 20);
            this.SNCombox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(168, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "SN:";
            // 
            // Btn_SearchCam
            // 
            this.Btn_SearchCam.Location = new System.Drawing.Point(327, 14);
            this.Btn_SearchCam.Name = "Btn_SearchCam";
            this.Btn_SearchCam.Size = new System.Drawing.Size(59, 23);
            this.Btn_SearchCam.TabIndex = 4;
            this.Btn_SearchCam.Text = "搜索";
            this.Btn_SearchCam.UseVisualStyleBackColor = true;
            this.Btn_SearchCam.Click += new System.EventHandler(this.Btn_SearchCam_Click);
            // 
            // Btn_SaveImage
            // 
            this.Btn_SaveImage.Location = new System.Drawing.Point(853, 622);
            this.Btn_SaveImage.Name = "Btn_SaveImage";
            this.Btn_SaveImage.Size = new System.Drawing.Size(75, 23);
            this.Btn_SaveImage.TabIndex = 5;
            this.Btn_SaveImage.Text = "保存";
            this.Btn_SaveImage.UseVisualStyleBackColor = true;
            this.Btn_SaveImage.Click += new System.EventHandler(this.Btn_SaveImage_Click);
            // 
            // Btn_ReadImage
            // 
            this.Btn_ReadImage.Location = new System.Drawing.Point(772, 622);
            this.Btn_ReadImage.Name = "Btn_ReadImage";
            this.Btn_ReadImage.Size = new System.Drawing.Size(75, 23);
            this.Btn_ReadImage.TabIndex = 5;
            this.Btn_ReadImage.Text = "读图";
            this.Btn_ReadImage.UseVisualStyleBackColor = true;
            this.Btn_ReadImage.Click += new System.EventHandler(this.Btn_ReadImage_Click);
            // 
            // DisplayBox
            // 
            this.DisplayBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DisplayBox.BackColor = System.Drawing.Color.Black;
            this.DisplayBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DisplayBox.Location = new System.Drawing.Point(6, 6);
            this.DisplayBox.Margin = new System.Windows.Forms.Padding(5);
            this.DisplayBox.Name = "DisplayBox";
            this.DisplayBox.Size = new System.Drawing.Size(935, 572);
            this.DisplayBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.DisplayBox.TabIndex = 1;
            this.DisplayBox.TabStop = false;
            this.DisplayBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DisplayBox_MouseDown);
            this.DisplayBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DisplayBox_MouseMove);
            this.DisplayBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DisplayBox_MouseUp);
            // 
            // DisplayPanel
            // 
            this.DisplayPanel.BackColor = System.Drawing.Color.Black;
            this.DisplayPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DisplayPanel.Controls.Add(this.DisplayBox);
            this.DisplayPanel.Location = new System.Drawing.Point(12, 32);
            this.DisplayPanel.Name = "DisplayPanel";
            this.DisplayPanel.Size = new System.Drawing.Size(948, 585);
            this.DisplayPanel.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.BrandCombox);
            this.panel1.Controls.Add(this.SNCombox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.Btn_SearchCam);
            this.panel1.Location = new System.Drawing.Point(966, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(386, 41);
            this.panel1.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.Rbtn_TriggerModel);
            this.panel2.Controls.Add(this.Rbtn_ContinueModel);
            this.panel2.Location = new System.Drawing.Point(966, 90);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(386, 29);
            this.panel2.TabIndex = 8;
            // 
            // Rbtn_TriggerModel
            // 
            this.Rbtn_TriggerModel.AutoSize = true;
            this.Rbtn_TriggerModel.Location = new System.Drawing.Point(247, 6);
            this.Rbtn_TriggerModel.Name = "Rbtn_TriggerModel";
            this.Rbtn_TriggerModel.Size = new System.Drawing.Size(71, 16);
            this.Rbtn_TriggerModel.TabIndex = 0;
            this.Rbtn_TriggerModel.Text = "触发模式";
            this.Rbtn_TriggerModel.UseVisualStyleBackColor = true;
            this.Rbtn_TriggerModel.Click += new System.EventHandler(this.Rbtn_ModelChanged);
            // 
            // Rbtn_ContinueModel
            // 
            this.Rbtn_ContinueModel.AutoSize = true;
            this.Rbtn_ContinueModel.Checked = true;
            this.Rbtn_ContinueModel.Location = new System.Drawing.Point(67, 6);
            this.Rbtn_ContinueModel.Name = "Rbtn_ContinueModel";
            this.Rbtn_ContinueModel.Size = new System.Drawing.Size(71, 16);
            this.Rbtn_ContinueModel.TabIndex = 0;
            this.Rbtn_ContinueModel.TabStop = true;
            this.Rbtn_ContinueModel.Text = "连续模式";
            this.Rbtn_ContinueModel.UseVisualStyleBackColor = true;
            this.Rbtn_ContinueModel.Click += new System.EventHandler(this.Rbtn_ModelChanged);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.Combobox_HardSource_Callback);
            this.panel3.Controls.Add(this.Combobox_HardSource);
            this.panel3.Controls.Add(this.Rbtn_Trigger_Hard);
            this.panel3.Controls.Add(this.Rbtn_Trigger_HardCallback);
            this.panel3.Controls.Add(this.Rbtn_Trigger_SoftCallback);
            this.panel3.Controls.Add(this.Rbtn_Trigger_Soft);
            this.panel3.Controls.Add(this.Btn_SoftTrigger_Callback);
            this.panel3.Controls.Add(this.Btn_SoftTrigger);
            this.panel3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel3.Location = new System.Drawing.Point(966, 118);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(386, 131);
            this.panel3.TabIndex = 9;
            // 
            // Combobox_HardSource_Callback
            // 
            this.Combobox_HardSource_Callback.FormattingEnabled = true;
            this.Combobox_HardSource_Callback.Items.AddRange(new object[] {
            " Line0",
            " Line1",
            " Line2",
            " Line3",
            " Line4",
            " Line5"});
            this.Combobox_HardSource_Callback.Location = new System.Drawing.Point(243, 98);
            this.Combobox_HardSource_Callback.Name = "Combobox_HardSource_Callback";
            this.Combobox_HardSource_Callback.Size = new System.Drawing.Size(75, 20);
            this.Combobox_HardSource_Callback.TabIndex = 5;
            // 
            // Combobox_HardSource
            // 
            this.Combobox_HardSource.FormattingEnabled = true;
            this.Combobox_HardSource.Items.AddRange(new object[] {
            " Line0",
            " Line1",
            " Line2",
            " Line3",
            " Line4",
            " Line5"});
            this.Combobox_HardSource.Location = new System.Drawing.Point(243, 41);
            this.Combobox_HardSource.Name = "Combobox_HardSource";
            this.Combobox_HardSource.Size = new System.Drawing.Size(75, 20);
            this.Combobox_HardSource.TabIndex = 5;
            // 
            // Rbtn_Trigger_Hard
            // 
            this.Rbtn_Trigger_Hard.AutoSize = true;
            this.Rbtn_Trigger_Hard.Location = new System.Drawing.Point(67, 43);
            this.Rbtn_Trigger_Hard.Name = "Rbtn_Trigger_Hard";
            this.Rbtn_Trigger_Hard.Size = new System.Drawing.Size(59, 16);
            this.Rbtn_Trigger_Hard.TabIndex = 0;
            this.Rbtn_Trigger_Hard.TabStop = true;
            this.Rbtn_Trigger_Hard.Text = "硬触发";
            this.Rbtn_Trigger_Hard.UseVisualStyleBackColor = true;
            this.Rbtn_Trigger_Hard.CheckedChanged += new System.EventHandler(this.Rbtn_TriggerStyleChanged);
            // 
            // Rbtn_Trigger_HardCallback
            // 
            this.Rbtn_Trigger_HardCallback.AutoSize = true;
            this.Rbtn_Trigger_HardCallback.Location = new System.Drawing.Point(67, 100);
            this.Rbtn_Trigger_HardCallback.Name = "Rbtn_Trigger_HardCallback";
            this.Rbtn_Trigger_HardCallback.Size = new System.Drawing.Size(101, 16);
            this.Rbtn_Trigger_HardCallback.TabIndex = 0;
            this.Rbtn_Trigger_HardCallback.TabStop = true;
            this.Rbtn_Trigger_HardCallback.Text = "硬触发 + 回调";
            this.Rbtn_Trigger_HardCallback.UseVisualStyleBackColor = true;
            this.Rbtn_Trigger_HardCallback.CheckedChanged += new System.EventHandler(this.Rbtn_TriggerStyleChanged);
            // 
            // Rbtn_Trigger_SoftCallback
            // 
            this.Rbtn_Trigger_SoftCallback.AutoSize = true;
            this.Rbtn_Trigger_SoftCallback.Location = new System.Drawing.Point(67, 71);
            this.Rbtn_Trigger_SoftCallback.Name = "Rbtn_Trigger_SoftCallback";
            this.Rbtn_Trigger_SoftCallback.Size = new System.Drawing.Size(101, 16);
            this.Rbtn_Trigger_SoftCallback.TabIndex = 0;
            this.Rbtn_Trigger_SoftCallback.TabStop = true;
            this.Rbtn_Trigger_SoftCallback.Text = "软触发 + 回调";
            this.Rbtn_Trigger_SoftCallback.UseVisualStyleBackColor = true;
            this.Rbtn_Trigger_SoftCallback.CheckedChanged += new System.EventHandler(this.Rbtn_TriggerStyleChanged);
            // 
            // Rbtn_Trigger_Soft
            // 
            this.Rbtn_Trigger_Soft.AutoSize = true;
            this.Rbtn_Trigger_Soft.Location = new System.Drawing.Point(67, 14);
            this.Rbtn_Trigger_Soft.Name = "Rbtn_Trigger_Soft";
            this.Rbtn_Trigger_Soft.Size = new System.Drawing.Size(59, 16);
            this.Rbtn_Trigger_Soft.TabIndex = 0;
            this.Rbtn_Trigger_Soft.TabStop = true;
            this.Rbtn_Trigger_Soft.Text = "软触发";
            this.Rbtn_Trigger_Soft.UseVisualStyleBackColor = true;
            this.Rbtn_Trigger_Soft.CheckedChanged += new System.EventHandler(this.Rbtn_TriggerStyleChanged);
            // 
            // Btn_SoftTrigger_Callback
            // 
            this.Btn_SoftTrigger_Callback.Location = new System.Drawing.Point(243, 68);
            this.Btn_SoftTrigger_Callback.Name = "Btn_SoftTrigger_Callback";
            this.Btn_SoftTrigger_Callback.Size = new System.Drawing.Size(75, 23);
            this.Btn_SoftTrigger_Callback.TabIndex = 4;
            this.Btn_SoftTrigger_Callback.Text = "触发一次";
            this.Btn_SoftTrigger_Callback.UseVisualStyleBackColor = true;
            this.Btn_SoftTrigger_Callback.Click += new System.EventHandler(this.Btn_SoftTrigger_Callback_Click);
            // 
            // Btn_SoftTrigger
            // 
            this.Btn_SoftTrigger.Location = new System.Drawing.Point(243, 11);
            this.Btn_SoftTrigger.Name = "Btn_SoftTrigger";
            this.Btn_SoftTrigger.Size = new System.Drawing.Size(75, 23);
            this.Btn_SoftTrigger.TabIndex = 4;
            this.Btn_SoftTrigger.Text = "触发一次";
            this.Btn_SoftTrigger.UseVisualStyleBackColor = true;
            this.Btn_SoftTrigger.Click += new System.EventHandler(this.Btn_SoftTrigger_Click);
            // 
            // Btn_Startup
            // 
            this.Btn_Startup.Location = new System.Drawing.Point(1108, 279);
            this.Btn_Startup.Name = "Btn_Startup";
            this.Btn_Startup.Size = new System.Drawing.Size(75, 23);
            this.Btn_Startup.TabIndex = 4;
            this.Btn_Startup.Text = "启动";
            this.Btn_Startup.UseVisualStyleBackColor = true;
            this.Btn_Startup.Click += new System.EventHandler(this.Btn_Startup_Click);
            // 
            // Btn_Destroy
            // 
            this.Btn_Destroy.Location = new System.Drawing.Point(1222, 279);
            this.Btn_Destroy.Name = "Btn_Destroy";
            this.Btn_Destroy.Size = new System.Drawing.Size(75, 23);
            this.Btn_Destroy.TabIndex = 4;
            this.Btn_Destroy.Text = "注销";
            this.Btn_Destroy.UseVisualStyleBackColor = true;
            this.Btn_Destroy.Click += new System.EventHandler(this.Btn_Destroy_Click);
            // 
            // Btn_AutoWhiteBlance
            // 
            this.Btn_AutoWhiteBlance.Location = new System.Drawing.Point(209, 83);
            this.Btn_AutoWhiteBlance.Name = "Btn_AutoWhiteBlance";
            this.Btn_AutoWhiteBlance.Size = new System.Drawing.Size(100, 21);
            this.Btn_AutoWhiteBlance.TabIndex = 11;
            this.Btn_AutoWhiteBlance.Text = "Auto";
            this.Btn_AutoWhiteBlance.UseVisualStyleBackColor = true;
            this.Btn_AutoWhiteBlance.Click += new System.EventHandler(this.Btn_AutoWhiteBlance_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Tbox_Gain);
            this.groupBox1.Controls.Add(this.Tbox_ExpouseTime);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.Tbox_TriggerFliter);
            this.groupBox1.Controls.Add(this.Btn_AutoWhiteBlance);
            this.groupBox1.Controls.Add(this.cmbox_TriggerPolarity);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(966, 321);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 169);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "参数";
            // 
            // Tbox_Gain
            // 
            this.Tbox_Gain.Location = new System.Drawing.Point(209, 53);
            this.Tbox_Gain.Name = "Tbox_Gain";
            this.Tbox_Gain.Size = new System.Drawing.Size(100, 21);
            this.Tbox_Gain.TabIndex = 18;
            this.Tbox_Gain.TextChanged += new System.EventHandler(this.Tbox_Gain_TextChanged);
            // 
            // Tbox_ExpouseTime
            // 
            this.Tbox_ExpouseTime.Location = new System.Drawing.Point(209, 25);
            this.Tbox_ExpouseTime.Name = "Tbox_ExpouseTime";
            this.Tbox_ExpouseTime.Size = new System.Drawing.Size(100, 21);
            this.Tbox_ExpouseTime.TabIndex = 19;
            this.Tbox_ExpouseTime.TextChanged += new System.EventHandler(this.Tbox_ExpouseTime_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(82, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "白平衡:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(82, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "增益:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(82, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "曝光:";
            // 
            // Tbox_TriggerFliter
            // 
            this.Tbox_TriggerFliter.Location = new System.Drawing.Point(209, 137);
            this.Tbox_TriggerFliter.Name = "Tbox_TriggerFliter";
            this.Tbox_TriggerFliter.Size = new System.Drawing.Size(100, 21);
            this.Tbox_TriggerFliter.TabIndex = 14;
            this.Tbox_TriggerFliter.TextChanged += new System.EventHandler(this.Tbox_TriggerFliter_TextChanged);
            // 
            // cmbox_TriggerPolarity
            // 
            this.cmbox_TriggerPolarity.FormattingEnabled = true;
            this.cmbox_TriggerPolarity.Items.AddRange(new object[] {
            "RisingEdge",
            "FallingEdge"});
            this.cmbox_TriggerPolarity.Location = new System.Drawing.Point(209, 111);
            this.cmbox_TriggerPolarity.Name = "cmbox_TriggerPolarity";
            this.cmbox_TriggerPolarity.Size = new System.Drawing.Size(100, 20);
            this.cmbox_TriggerPolarity.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(82, 114);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "触发极性:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(82, 140);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "触发滤波:";
            // 
            // Btn_CamInit
            // 
            this.Btn_CamInit.Location = new System.Drawing.Point(989, 279);
            this.Btn_CamInit.Name = "Btn_CamInit";
            this.Btn_CamInit.Size = new System.Drawing.Size(75, 23);
            this.Btn_CamInit.TabIndex = 4;
            this.Btn_CamInit.Text = "初始化";
            this.Btn_CamInit.UseVisualStyleBackColor = true;
            this.Btn_CamInit.Click += new System.EventHandler(this.Btn_CamInit_Click);
            // 
            // Tbox_Log
            // 
            this.Tbox_Log.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Tbox_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tbox_Log.Location = new System.Drawing.Point(3, 17);
            this.Tbox_Log.Multiline = true;
            this.Tbox_Log.Name = "Tbox_Log";
            this.Tbox_Log.ReadOnly = true;
            this.Tbox_Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Tbox_Log.Size = new System.Drawing.Size(936, 85);
            this.Tbox_Log.TabIndex = 13;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Tbox_Log);
            this.groupBox2.Location = new System.Drawing.Point(12, 644);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(942, 105);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "日志";
            // 
            // MainWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1364, 761);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.DisplayPanel);
            this.Controls.Add(this.Btn_ReadImage);
            this.Controls.Add(this.Btn_Destroy);
            this.Controls.Add(this.Btn_CamInit);
            this.Controls.Add(this.Btn_Startup);
            this.Controls.Add(this.Btn_SaveImage);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.HelpButton = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MG.CamVisual";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayBox)).EndInit();
            this.DisplayPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menubtn_about;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox BrandCombox;
        private System.Windows.Forms.ComboBox SNCombox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Btn_SearchCam;
        private System.Windows.Forms.Button Btn_SaveImage;
        private System.Windows.Forms.Button Btn_ReadImage;
        private System.Windows.Forms.PictureBox DisplayBox;
        private System.Windows.Forms.Panel DisplayPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton Rbtn_Trigger_Hard;
        private System.Windows.Forms.RadioButton Rbtn_Trigger_HardCallback;
        private System.Windows.Forms.RadioButton Rbtn_Trigger_SoftCallback;
        private System.Windows.Forms.RadioButton Rbtn_Trigger_Soft;
        public System.Windows.Forms.RadioButton Rbtn_ContinueModel;
        public System.Windows.Forms.RadioButton Rbtn_TriggerModel;
        private System.Windows.Forms.Button Btn_SoftTrigger_Callback;
        private System.Windows.Forms.Button Btn_SoftTrigger;
        private System.Windows.Forms.Button Btn_Startup;
        private System.Windows.Forms.Button Btn_Destroy;
        private System.Windows.Forms.ComboBox Combobox_HardSource_Callback;
        private System.Windows.Forms.ComboBox Combobox_HardSource;
        private System.Windows.Forms.Button Btn_AutoWhiteBlance;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbox_TriggerPolarity;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Tbox_TriggerFliter;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox Tbox_Gain;
        private System.Windows.Forms.TextBox Tbox_ExpouseTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Btn_CamInit;
        private System.Windows.Forms.TextBox Tbox_Log;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}


