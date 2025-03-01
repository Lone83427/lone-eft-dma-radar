namespace arena_dma_radar.UI.Radar
{
    sealed partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));
            colorDialog1 = new ColorDialog();
            toolTip1 = new ToolTip(components);
            tabPage2 = new TabPage();
            flowLayoutPanel_Settings = new FlowLayoutPanel();
            flowLayoutPanel_RadarSettings = new FlowLayoutPanel();
            label2 = new Label();
            label8 = new Label();
            linkLabel_CheckForUpdates = new LinkLabel();
            label1 = new Label();
            button_Restart = new Button();
            button_HotkeyManager = new Button();
            button_Radar_ColorPicker = new Button();
            button_BackupConfig = new Button();
            label_AimlineLength = new Label();
            trackBar_AimlineLength = new TrackBar();
            label_UIScale = new Label();
            trackBar_UIScale = new TrackBar();
            checkBox_MapSetup = new CheckBox();
            checkBox_Aimview = new CheckBox();
            checkBox_GrpConnect = new CheckBox();
            checkBox_HideNames = new CheckBox();
            flowLayoutPanel_MemWriteCheckbox = new FlowLayoutPanel();
            checkBox_EnableMemWrite = new CheckBox();
            flowLayoutPanel_MemWrites = new FlowLayoutPanel();
            label3 = new Label();
            checkBox_AdvancedMemWrites = new CheckBox();
            checkBox_AimBotEnabled = new CheckBox();
            checkBox_NoRecoilSway = new CheckBox();
            checkBox_Chams = new CheckBox();
            checkBox_NoVisor = new CheckBox();
            checkBox_NoWepMalf = new CheckBox();
            flowLayoutPanel_Aimbot = new FlowLayoutPanel();
            label13 = new Label();
            label6 = new Label();
            checkBox_SA_SafeLock = new CheckBox();
            checkBox_SA_AutoBone = new CheckBox();
            radioButton_AimTarget_FOV = new RadioButton();
            radioButton_AimTarget_CQB = new RadioButton();
            label_AimFOV = new Label();
            trackBar_AimFOV = new TrackBar();
            label10 = new Label();
            comboBox_AimbotTarget = new ComboBox();
            checkBox_AimRandomBone = new CheckBox();
            button_RandomBoneCfg = new Button();
            flowLayoutPanel_NoRecoil = new FlowLayoutPanel();
            label16 = new Label();
            label_Recoil = new Label();
            trackBar_NoRecoil = new TrackBar();
            label_Sway = new Label();
            trackBar_NoSway = new TrackBar();
            flowLayoutPanel_Chams = new FlowLayoutPanel();
            label17 = new Label();
            radioButton_Chams_Basic = new RadioButton();
            radioButton_Chams_Visible = new RadioButton();
            radioButton_Chams_Vischeck = new RadioButton();
            flowLayoutPanel_Vischeck = new FlowLayoutPanel();
            label14 = new Label();
            label15 = new Label();
            textBox_VischeckVisColor = new TextBox();
            button_VischeckVisColorPick = new Button();
            label33 = new Label();
            textBox_VischeckInvisColor = new TextBox();
            button_VischeckInvisColorPick = new Button();
            flowLayoutPanel_MonitorSettings = new FlowLayoutPanel();
            label11 = new Label();
            label_Width = new Label();
            textBox_ResWidth = new TextBox();
            label_Height = new Label();
            textBox_ResHeight = new TextBox();
            button_DetectRes = new Button();
            flowLayoutPanel_ESPSettings = new FlowLayoutPanel();
            label12 = new Label();
            label7 = new Label();
            button_StartESP = new Button();
            button_EspColorPicker = new Button();
            label_ESPFPSCap = new Label();
            textBox_EspFpsCap = new TextBox();
            checkBox_ESP_AutoFS = new CheckBox();
            comboBox_ESPAutoFS = new ComboBox();
            checkBox_ESP_Grenades = new CheckBox();
            checkBox_ESP_ShowMag = new CheckBox();
            checkBox_ESP_HighAlert = new CheckBox();
            checkBox_ESP_FireportAim = new CheckBox();
            checkBox_ESP_AimFov = new CheckBox();
            checkBox_ESP_AimLock = new CheckBox();
            checkBox_ESP_StatusText = new CheckBox();
            checkBox_ESP_FPS = new CheckBox();
            flowLayoutPanel_ESP_PlayerRender = new FlowLayoutPanel();
            label18 = new Label();
            radioButton_ESPRender_None = new RadioButton();
            radioButton_ESPRender_Bones = new RadioButton();
            checkBox_ESPRender_Labels = new CheckBox();
            checkBox_ESPRender_Weapons = new CheckBox();
            checkBox_ESPRender_Dist = new CheckBox();
            flowLayoutPanel4 = new FlowLayoutPanel();
            label_EspFontScale = new Label();
            trackBar_EspFontScale = new TrackBar();
            label_EspLineScale = new Label();
            trackBar_EspLineScale = new TrackBar();
            tabPage1 = new TabPage();
            checkBox_MapFree = new CheckBox();
            groupBox_MapSetup = new GroupBox();
            button_MapSetupApply = new Button();
            textBox_mapScale = new TextBox();
            label5 = new Label();
            textBox_mapY = new TextBox();
            label4 = new Label();
            textBox_mapX = new TextBox();
            label_Pos = new Label();
            skglControl_Radar = new SKGLControl();
            tabControl1 = new TabControl();
            tabPage2.SuspendLayout();
            flowLayoutPanel_Settings.SuspendLayout();
            flowLayoutPanel_RadarSettings.SuspendLayout();
            ((ISupportInitialize)trackBar_AimlineLength).BeginInit();
            ((ISupportInitialize)trackBar_UIScale).BeginInit();
            flowLayoutPanel_MemWriteCheckbox.SuspendLayout();
            flowLayoutPanel_MemWrites.SuspendLayout();
            flowLayoutPanel_Aimbot.SuspendLayout();
            ((ISupportInitialize)trackBar_AimFOV).BeginInit();
            flowLayoutPanel_NoRecoil.SuspendLayout();
            ((ISupportInitialize)trackBar_NoRecoil).BeginInit();
            ((ISupportInitialize)trackBar_NoSway).BeginInit();
            flowLayoutPanel_Chams.SuspendLayout();
            flowLayoutPanel_Vischeck.SuspendLayout();
            flowLayoutPanel_MonitorSettings.SuspendLayout();
            flowLayoutPanel_ESPSettings.SuspendLayout();
            flowLayoutPanel_ESP_PlayerRender.SuspendLayout();
            flowLayoutPanel4.SuspendLayout();
            ((ISupportInitialize)trackBar_EspFontScale).BeginInit();
            ((ISupportInitialize)trackBar_EspLineScale).BeginInit();
            tabPage1.SuspendLayout();
            groupBox_MapSetup.SuspendLayout();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // toolTip1
            // 
            toolTip1.AutoPopDelay = 20000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 100;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(flowLayoutPanel_Settings);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1256, 653);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Settings";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel_Settings
            // 
            flowLayoutPanel_Settings.AutoScroll = true;
            flowLayoutPanel_Settings.Controls.Add(flowLayoutPanel_RadarSettings);
            flowLayoutPanel_Settings.Controls.Add(flowLayoutPanel_MemWriteCheckbox);
            flowLayoutPanel_Settings.Controls.Add(flowLayoutPanel_MemWrites);
            flowLayoutPanel_Settings.Controls.Add(flowLayoutPanel_MonitorSettings);
            flowLayoutPanel_Settings.Controls.Add(flowLayoutPanel_ESPSettings);
            flowLayoutPanel_Settings.Dock = DockStyle.Fill;
            flowLayoutPanel_Settings.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel_Settings.Location = new Point(3, 3);
            flowLayoutPanel_Settings.Name = "flowLayoutPanel_Settings";
            flowLayoutPanel_Settings.Size = new Size(1250, 647);
            flowLayoutPanel_Settings.TabIndex = 10;
            flowLayoutPanel_Settings.WrapContents = false;
            // 
            // flowLayoutPanel_RadarSettings
            // 
            flowLayoutPanel_RadarSettings.AutoSize = true;
            flowLayoutPanel_RadarSettings.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_RadarSettings.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel_RadarSettings.Controls.Add(label2);
            flowLayoutPanel_RadarSettings.Controls.Add(label8);
            flowLayoutPanel_RadarSettings.Controls.Add(linkLabel_CheckForUpdates);
            flowLayoutPanel_RadarSettings.Controls.Add(label1);
            flowLayoutPanel_RadarSettings.Controls.Add(button_Restart);
            flowLayoutPanel_RadarSettings.Controls.Add(button_HotkeyManager);
            flowLayoutPanel_RadarSettings.Controls.Add(button_Radar_ColorPicker);
            flowLayoutPanel_RadarSettings.Controls.Add(button_BackupConfig);
            flowLayoutPanel_RadarSettings.Controls.Add(label_AimlineLength);
            flowLayoutPanel_RadarSettings.Controls.Add(trackBar_AimlineLength);
            flowLayoutPanel_RadarSettings.Controls.Add(label_UIScale);
            flowLayoutPanel_RadarSettings.Controls.Add(trackBar_UIScale);
            flowLayoutPanel_RadarSettings.Controls.Add(checkBox_MapSetup);
            flowLayoutPanel_RadarSettings.Controls.Add(checkBox_Aimview);
            flowLayoutPanel_RadarSettings.Controls.Add(checkBox_GrpConnect);
            flowLayoutPanel_RadarSettings.Controls.Add(checkBox_HideNames);
            flowLayoutPanel_RadarSettings.Dock = DockStyle.Top;
            flowLayoutPanel_Settings.SetFlowBreak(flowLayoutPanel_RadarSettings, true);
            flowLayoutPanel_RadarSettings.Location = new Point(3, 3);
            flowLayoutPanel_RadarSettings.Name = "flowLayoutPanel_RadarSettings";
            flowLayoutPanel_RadarSettings.Size = new Size(1186, 173);
            flowLayoutPanel_RadarSettings.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            flowLayoutPanel_RadarSettings.SetFlowBreak(label2, true);
            label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(3, 0);
            label2.Name = "label2";
            label2.Size = new Size(179, 21);
            label2.TabIndex = 45;
            label2.Text = "Radar/General Settings";
            // 
            // label8
            // 
            label8.Location = new Point(3, 21);
            label8.Name = "label8";
            label8.Size = new Size(0, 0);
            label8.TabIndex = 46;
            label8.Text = "label8";
            // 
            // linkLabel_CheckForUpdates
            // 
            linkLabel_CheckForUpdates.AutoSize = true;
            flowLayoutPanel_RadarSettings.SetFlowBreak(linkLabel_CheckForUpdates, true);
            linkLabel_CheckForUpdates.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            linkLabel_CheckForUpdates.Location = new Point(9, 27);
            linkLabel_CheckForUpdates.Margin = new Padding(3, 6, 3, 6);
            linkLabel_CheckForUpdates.Name = "linkLabel_CheckForUpdates";
            linkLabel_CheckForUpdates.Size = new Size(278, 15);
            linkLabel_CheckForUpdates.TabIndex = 47;
            linkLabel_CheckForUpdates.TabStop = true;
            linkLabel_CheckForUpdates.Text = "Check for updates at lone-eft.com/ongoingsupport";
            linkLabel_CheckForUpdates.LinkClicked += linkLabel_CheckForUpdates_LinkClicked;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(4, 48);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(169, 45);
            label1.TabIndex = 16;
            label1.Text = "Zoom In: F1 / Mouse Whl Up\r\nZoom Out: F2 / Mouse Whl Dn\r\nToggle Fullscreen: F11";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button_Restart
            // 
            button_Restart.Font = new Font("Segoe UI", 9.75F);
            button_Restart.Location = new Point(180, 51);
            button_Restart.Name = "button_Restart";
            button_Restart.Size = new Size(107, 41);
            button_Restart.TabIndex = 18;
            button_Restart.Text = "Restart Radar";
            button_Restart.UseVisualStyleBackColor = true;
            button_Restart.Click += button_Restart_Click;
            // 
            // button_HotkeyManager
            // 
            button_HotkeyManager.Location = new Point(293, 51);
            button_HotkeyManager.Name = "button_HotkeyManager";
            button_HotkeyManager.Size = new Size(107, 41);
            button_HotkeyManager.TabIndex = 34;
            button_HotkeyManager.Text = "Hotkey Manager";
            button_HotkeyManager.UseVisualStyleBackColor = true;
            button_HotkeyManager.Click += button_HotkeyManager_Click;
            // 
            // button_Radar_ColorPicker
            // 
            button_Radar_ColorPicker.Location = new Point(406, 51);
            button_Radar_ColorPicker.Name = "button_Radar_ColorPicker";
            button_Radar_ColorPicker.Size = new Size(107, 41);
            button_Radar_ColorPicker.TabIndex = 21;
            button_Radar_ColorPicker.Text = "Color Picker";
            button_Radar_ColorPicker.UseVisualStyleBackColor = true;
            button_Radar_ColorPicker.Click += button_Radar_ColorPicker_Click;
            // 
            // button_BackupConfig
            // 
            flowLayoutPanel_RadarSettings.SetFlowBreak(button_BackupConfig, true);
            button_BackupConfig.Location = new Point(519, 51);
            button_BackupConfig.Name = "button_BackupConfig";
            button_BackupConfig.Size = new Size(107, 41);
            button_BackupConfig.TabIndex = 20;
            button_BackupConfig.Text = "Backup Config";
            button_BackupConfig.UseVisualStyleBackColor = true;
            button_BackupConfig.Click += button_BackupConfig_Click;
            // 
            // label_AimlineLength
            // 
            label_AimlineLength.Anchor = AnchorStyles.Right;
            label_AimlineLength.AutoSize = true;
            label_AimlineLength.Location = new Point(4, 113);
            label_AimlineLength.Margin = new Padding(4, 0, 4, 0);
            label_AimlineLength.Name = "label_AimlineLength";
            label_AimlineLength.Size = new Size(88, 15);
            label_AimlineLength.TabIndex = 13;
            label_AimlineLength.Text = "Aimline Length";
            label_AimlineLength.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // trackBar_AimlineLength
            // 
            trackBar_AimlineLength.Anchor = AnchorStyles.Right;
            trackBar_AimlineLength.BackColor = SystemColors.Window;
            trackBar_AimlineLength.LargeChange = 50;
            trackBar_AimlineLength.Location = new Point(100, 98);
            trackBar_AimlineLength.Margin = new Padding(4, 3, 4, 3);
            trackBar_AimlineLength.Maximum = 1500;
            trackBar_AimlineLength.Minimum = 10;
            trackBar_AimlineLength.Name = "trackBar_AimlineLength";
            trackBar_AimlineLength.Size = new Size(92, 45);
            trackBar_AimlineLength.SmallChange = 5;
            trackBar_AimlineLength.TabIndex = 11;
            trackBar_AimlineLength.TickStyle = TickStyle.None;
            trackBar_AimlineLength.Value = 1500;
            // 
            // label_UIScale
            // 
            label_UIScale.Anchor = AnchorStyles.Right;
            label_UIScale.AutoSize = true;
            label_UIScale.Location = new Point(199, 113);
            label_UIScale.Name = "label_UIScale";
            label_UIScale.Size = new Size(72, 15);
            label_UIScale.TabIndex = 28;
            label_UIScale.Text = "UI Scale 1.00";
            label_UIScale.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // trackBar_UIScale
            // 
            trackBar_UIScale.Anchor = AnchorStyles.Right;
            trackBar_UIScale.BackColor = SystemColors.Window;
            flowLayoutPanel_RadarSettings.SetFlowBreak(trackBar_UIScale, true);
            trackBar_UIScale.LargeChange = 10;
            trackBar_UIScale.Location = new Point(277, 98);
            trackBar_UIScale.Maximum = 200;
            trackBar_UIScale.Minimum = 50;
            trackBar_UIScale.Name = "trackBar_UIScale";
            trackBar_UIScale.Size = new Size(92, 45);
            trackBar_UIScale.TabIndex = 27;
            trackBar_UIScale.TickStyle = TickStyle.None;
            trackBar_UIScale.Value = 100;
            // 
            // checkBox_MapSetup
            // 
            checkBox_MapSetup.AutoSize = true;
            checkBox_MapSetup.Location = new Point(3, 149);
            checkBox_MapSetup.Name = "checkBox_MapSetup";
            checkBox_MapSetup.Size = new Size(153, 19);
            checkBox_MapSetup.TabIndex = 9;
            checkBox_MapSetup.Text = "Show Map Setup Helper";
            checkBox_MapSetup.UseVisualStyleBackColor = true;
            checkBox_MapSetup.CheckedChanged += checkBox_MapSetup_CheckedChanged;
            // 
            // checkBox_Aimview
            // 
            checkBox_Aimview.AutoSize = true;
            checkBox_Aimview.Location = new Point(162, 149);
            checkBox_Aimview.Name = "checkBox_Aimview";
            checkBox_Aimview.Size = new Size(86, 19);
            checkBox_Aimview.TabIndex = 19;
            checkBox_Aimview.Text = "ESP Widget";
            checkBox_Aimview.UseVisualStyleBackColor = true;
            checkBox_Aimview.CheckedChanged += checkBox_Aimview_CheckedChanged;
            // 
            // checkBox_GrpConnect
            // 
            checkBox_GrpConnect.AutoSize = true;
            checkBox_GrpConnect.Location = new Point(254, 149);
            checkBox_GrpConnect.Name = "checkBox_GrpConnect";
            checkBox_GrpConnect.Size = new Size(112, 19);
            checkBox_GrpConnect.TabIndex = 33;
            checkBox_GrpConnect.Text = "Connect Groups";
            checkBox_GrpConnect.UseVisualStyleBackColor = true;
            // 
            // checkBox_HideNames
            // 
            checkBox_HideNames.AutoSize = true;
            checkBox_HideNames.Location = new Point(372, 149);
            checkBox_HideNames.Name = "checkBox_HideNames";
            checkBox_HideNames.Size = new Size(91, 19);
            checkBox_HideNames.TabIndex = 26;
            checkBox_HideNames.Text = "Hide Names";
            checkBox_HideNames.UseVisualStyleBackColor = true;
            checkBox_HideNames.CheckedChanged += checkBox_HideNames_CheckedChanged;
            // 
            // flowLayoutPanel_MemWriteCheckbox
            // 
            flowLayoutPanel_MemWriteCheckbox.AutoSize = true;
            flowLayoutPanel_MemWriteCheckbox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_MemWriteCheckbox.Controls.Add(checkBox_EnableMemWrite);
            flowLayoutPanel_Settings.SetFlowBreak(flowLayoutPanel_MemWriteCheckbox, true);
            flowLayoutPanel_MemWriteCheckbox.Location = new Point(3, 182);
            flowLayoutPanel_MemWriteCheckbox.Name = "flowLayoutPanel_MemWriteCheckbox";
            flowLayoutPanel_MemWriteCheckbox.Size = new Size(190, 25);
            flowLayoutPanel_MemWriteCheckbox.TabIndex = 45;
            // 
            // checkBox_EnableMemWrite
            // 
            checkBox_EnableMemWrite.AutoSize = true;
            checkBox_EnableMemWrite.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkBox_EnableMemWrite.Location = new Point(3, 3);
            checkBox_EnableMemWrite.Name = "checkBox_EnableMemWrite";
            checkBox_EnableMemWrite.Size = new Size(184, 19);
            checkBox_EnableMemWrite.TabIndex = 44;
            checkBox_EnableMemWrite.Text = "Enable Memory Writes (Risky)";
            checkBox_EnableMemWrite.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel_MemWrites
            // 
            flowLayoutPanel_MemWrites.AutoSize = true;
            flowLayoutPanel_MemWrites.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_MemWrites.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel_MemWrites.Controls.Add(label3);
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_AdvancedMemWrites);
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_AimBotEnabled);
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_NoRecoilSway);
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_Chams);
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_NoVisor);
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_NoWepMalf);
            flowLayoutPanel_MemWrites.Controls.Add(flowLayoutPanel_Aimbot);
            flowLayoutPanel_MemWrites.Controls.Add(flowLayoutPanel_NoRecoil);
            flowLayoutPanel_MemWrites.Controls.Add(flowLayoutPanel_Chams);
            flowLayoutPanel_MemWrites.Dock = DockStyle.Top;
            flowLayoutPanel_Settings.SetFlowBreak(flowLayoutPanel_MemWrites, true);
            flowLayoutPanel_MemWrites.Location = new Point(3, 213);
            flowLayoutPanel_MemWrites.Name = "flowLayoutPanel_MemWrites";
            flowLayoutPanel_MemWrites.Size = new Size(1186, 191);
            flowLayoutPanel_MemWrites.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            flowLayoutPanel_MemWrites.SetFlowBreak(label3, true);
            label3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(3, 0);
            label3.Name = "label3";
            label3.Size = new Size(183, 21);
            label3.TabIndex = 60;
            label3.Text = "Memory Write Features";
            // 
            // checkBox_AdvancedMemWrites
            // 
            checkBox_AdvancedMemWrites.AutoSize = true;
            flowLayoutPanel_MemWrites.SetFlowBreak(checkBox_AdvancedMemWrites, true);
            checkBox_AdvancedMemWrites.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkBox_AdvancedMemWrites.Location = new Point(3, 28);
            checkBox_AdvancedMemWrites.Name = "checkBox_AdvancedMemWrites";
            checkBox_AdvancedMemWrites.Size = new Size(246, 19);
            checkBox_AdvancedMemWrites.TabIndex = 62;
            checkBox_AdvancedMemWrites.Text = "Enable Advanced MemWrites (Very Risky)";
            checkBox_AdvancedMemWrites.UseVisualStyleBackColor = true;
            // 
            // checkBox_AimBotEnabled
            // 
            checkBox_AimBotEnabled.AutoSize = true;
            checkBox_AimBotEnabled.Location = new Point(3, 53);
            checkBox_AimBotEnabled.Name = "checkBox_AimBotEnabled";
            checkBox_AimBotEnabled.Size = new Size(104, 19);
            checkBox_AimBotEnabled.TabIndex = 41;
            checkBox_AimBotEnabled.Text = "Aimbot (Risky)";
            checkBox_AimBotEnabled.UseVisualStyleBackColor = true;
            checkBox_AimBotEnabled.CheckedChanged += checkBox_AimBotEnabled_CheckedChanged;
            // 
            // checkBox_NoRecoilSway
            // 
            checkBox_NoRecoilSway.AutoSize = true;
            checkBox_NoRecoilSway.Location = new Point(113, 53);
            checkBox_NoRecoilSway.Name = "checkBox_NoRecoilSway";
            checkBox_NoRecoilSway.Size = new Size(147, 19);
            checkBox_NoRecoilSway.TabIndex = 34;
            checkBox_NoRecoilSway.Text = "No Recoil/Sway (Risky)";
            checkBox_NoRecoilSway.UseVisualStyleBackColor = true;
            checkBox_NoRecoilSway.CheckedChanged += checkBox_NoRecoilSway_CheckedChanged;
            // 
            // checkBox_Chams
            // 
            checkBox_Chams.AutoSize = true;
            checkBox_Chams.Location = new Point(266, 53);
            checkBox_Chams.Name = "checkBox_Chams";
            checkBox_Chams.Size = new Size(63, 19);
            checkBox_Chams.TabIndex = 42;
            checkBox_Chams.Text = "Chams";
            checkBox_Chams.UseVisualStyleBackColor = true;
            checkBox_Chams.CheckedChanged += checkBox_Chams_CheckedChanged;
            // 
            // checkBox_NoVisor
            // 
            checkBox_NoVisor.AutoSize = true;
            checkBox_NoVisor.Location = new Point(335, 53);
            checkBox_NoVisor.Name = "checkBox_NoVisor";
            checkBox_NoVisor.Size = new Size(71, 19);
            checkBox_NoVisor.TabIndex = 37;
            checkBox_NoVisor.Text = "No Visor";
            checkBox_NoVisor.UseVisualStyleBackColor = true;
            checkBox_NoVisor.CheckedChanged += checkBox_NoVisor_CheckedChanged;
            // 
            // checkBox_NoWepMalf
            // 
            checkBox_NoWepMalf.AutoSize = true;
            flowLayoutPanel_MemWrites.SetFlowBreak(checkBox_NoWepMalf, true);
            checkBox_NoWepMalf.Location = new Point(412, 53);
            checkBox_NoWepMalf.Name = "checkBox_NoWepMalf";
            checkBox_NoWepMalf.Size = new Size(142, 19);
            checkBox_NoWepMalf.TabIndex = 61;
            checkBox_NoWepMalf.Text = "No Wep Malfunctions";
            checkBox_NoWepMalf.UseVisualStyleBackColor = true;
            checkBox_NoWepMalf.CheckedChanged += checkBox_NoWepMalf_CheckedChanged;
            // 
            // flowLayoutPanel_Aimbot
            // 
            flowLayoutPanel_Aimbot.AutoSize = true;
            flowLayoutPanel_Aimbot.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_Aimbot.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel_Aimbot.Controls.Add(label13);
            flowLayoutPanel_Aimbot.Controls.Add(label6);
            flowLayoutPanel_Aimbot.Controls.Add(checkBox_SA_SafeLock);
            flowLayoutPanel_Aimbot.Controls.Add(checkBox_SA_AutoBone);
            flowLayoutPanel_Aimbot.Controls.Add(radioButton_AimTarget_FOV);
            flowLayoutPanel_Aimbot.Controls.Add(radioButton_AimTarget_CQB);
            flowLayoutPanel_Aimbot.Controls.Add(label_AimFOV);
            flowLayoutPanel_Aimbot.Controls.Add(trackBar_AimFOV);
            flowLayoutPanel_Aimbot.Controls.Add(label10);
            flowLayoutPanel_Aimbot.Controls.Add(comboBox_AimbotTarget);
            flowLayoutPanel_Aimbot.Controls.Add(checkBox_AimRandomBone);
            flowLayoutPanel_Aimbot.Controls.Add(button_RandomBoneCfg);
            flowLayoutPanel_Aimbot.Enabled = false;
            flowLayoutPanel_Aimbot.Location = new Point(3, 78);
            flowLayoutPanel_Aimbot.Name = "flowLayoutPanel_Aimbot";
            flowLayoutPanel_Aimbot.Size = new Size(420, 102);
            flowLayoutPanel_Aimbot.TabIndex = 4;
            // 
            // label13
            // 
            label13.AutoSize = true;
            flowLayoutPanel_Aimbot.SetFlowBreak(label13, true);
            label13.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            label13.Location = new Point(3, 0);
            label13.Name = "label13";
            label13.Size = new Size(113, 15);
            label13.TabIndex = 0;
            label13.Text = "Aimbot (Silent Aim)";
            // 
            // label6
            // 
            label6.Location = new Point(3, 15);
            label6.Name = "label6";
            label6.Size = new Size(0, 0);
            label6.TabIndex = 70;
            label6.Text = "label6";
            // 
            // checkBox_SA_SafeLock
            // 
            checkBox_SA_SafeLock.Anchor = AnchorStyles.Right;
            checkBox_SA_SafeLock.AutoSize = true;
            checkBox_SA_SafeLock.Location = new Point(9, 31);
            checkBox_SA_SafeLock.Name = "checkBox_SA_SafeLock";
            checkBox_SA_SafeLock.Size = new Size(76, 19);
            checkBox_SA_SafeLock.TabIndex = 59;
            checkBox_SA_SafeLock.Text = "Safe Lock";
            checkBox_SA_SafeLock.UseVisualStyleBackColor = true;
            checkBox_SA_SafeLock.CheckedChanged += checkBox_SA_SafeLock_CheckedChanged;
            // 
            // checkBox_SA_AutoBone
            // 
            checkBox_SA_AutoBone.Anchor = AnchorStyles.Right;
            checkBox_SA_AutoBone.AutoSize = true;
            checkBox_SA_AutoBone.Location = new Point(91, 31);
            checkBox_SA_AutoBone.Name = "checkBox_SA_AutoBone";
            checkBox_SA_AutoBone.Size = new Size(82, 19);
            checkBox_SA_AutoBone.TabIndex = 0;
            checkBox_SA_AutoBone.Text = "Auto Bone";
            checkBox_SA_AutoBone.UseVisualStyleBackColor = true;
            checkBox_SA_AutoBone.CheckedChanged += checkBox_SA_AutoBone_CheckedChanged;
            // 
            // radioButton_AimTarget_FOV
            // 
            radioButton_AimTarget_FOV.Anchor = AnchorStyles.Right;
            radioButton_AimTarget_FOV.AutoSize = true;
            radioButton_AimTarget_FOV.Checked = true;
            radioButton_AimTarget_FOV.Location = new Point(179, 31);
            radioButton_AimTarget_FOV.Name = "radioButton_AimTarget_FOV";
            radioButton_AimTarget_FOV.Size = new Size(47, 19);
            radioButton_AimTarget_FOV.TabIndex = 52;
            radioButton_AimTarget_FOV.TabStop = true;
            radioButton_AimTarget_FOV.Text = "FOV";
            radioButton_AimTarget_FOV.UseVisualStyleBackColor = true;
            radioButton_AimTarget_FOV.CheckedChanged += radioButton_AimTarget_FOV_CheckedChanged;
            // 
            // radioButton_AimTarget_CQB
            // 
            radioButton_AimTarget_CQB.Anchor = AnchorStyles.Right;
            radioButton_AimTarget_CQB.AutoSize = true;
            radioButton_AimTarget_CQB.Location = new Point(232, 31);
            radioButton_AimTarget_CQB.Name = "radioButton_AimTarget_CQB";
            radioButton_AimTarget_CQB.Size = new Size(49, 19);
            radioButton_AimTarget_CQB.TabIndex = 53;
            radioButton_AimTarget_CQB.Text = "CQB";
            radioButton_AimTarget_CQB.UseVisualStyleBackColor = true;
            radioButton_AimTarget_CQB.CheckedChanged += radioButton_AimTarget_CQB_CheckedChanged;
            // 
            // label_AimFOV
            // 
            label_AimFOV.Anchor = AnchorStyles.Right;
            label_AimFOV.AutoSize = true;
            label_AimFOV.Location = new Point(287, 33);
            label_AimFOV.Name = "label_AimFOV";
            label_AimFOV.Size = new Size(44, 15);
            label_AimFOV.TabIndex = 57;
            label_AimFOV.Text = "FOV 30";
            // 
            // trackBar_AimFOV
            // 
            trackBar_AimFOV.Anchor = AnchorStyles.Right;
            trackBar_AimFOV.BackColor = SystemColors.Window;
            flowLayoutPanel_Aimbot.SetFlowBreak(trackBar_AimFOV, true);
            trackBar_AimFOV.Location = new Point(337, 18);
            trackBar_AimFOV.Maximum = 500;
            trackBar_AimFOV.Minimum = 5;
            trackBar_AimFOV.Name = "trackBar_AimFOV";
            trackBar_AimFOV.Size = new Size(78, 45);
            trackBar_AimFOV.TabIndex = 56;
            trackBar_AimFOV.TickStyle = TickStyle.None;
            trackBar_AimFOV.Value = 30;
            // 
            // label10
            // 
            label10.Anchor = AnchorStyles.Right;
            label10.AutoSize = true;
            label10.Location = new Point(3, 75);
            label10.Name = "label10";
            label10.Size = new Size(40, 15);
            label10.TabIndex = 51;
            label10.Text = "Target";
            // 
            // comboBox_AimbotTarget
            // 
            comboBox_AimbotTarget.Anchor = AnchorStyles.Right;
            comboBox_AimbotTarget.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_AimbotTarget.FormattingEnabled = true;
            comboBox_AimbotTarget.Location = new Point(49, 71);
            comboBox_AimbotTarget.Name = "comboBox_AimbotTarget";
            comboBox_AimbotTarget.Size = new Size(121, 23);
            comboBox_AimbotTarget.TabIndex = 50;
            // 
            // checkBox_AimRandomBone
            // 
            checkBox_AimRandomBone.Anchor = AnchorStyles.Right;
            checkBox_AimRandomBone.AutoSize = true;
            checkBox_AimRandomBone.Location = new Point(176, 73);
            checkBox_AimRandomBone.Name = "checkBox_AimRandomBone";
            checkBox_AimRandomBone.Size = new Size(101, 19);
            checkBox_AimRandomBone.TabIndex = 68;
            checkBox_AimRandomBone.Text = "Random Bone";
            checkBox_AimRandomBone.UseVisualStyleBackColor = true;
            checkBox_AimRandomBone.CheckedChanged += checkBox_AimRandomBone_CheckedChanged;
            // 
            // button_RandomBoneCfg
            // 
            button_RandomBoneCfg.Anchor = AnchorStyles.Right;
            button_RandomBoneCfg.Enabled = false;
            button_RandomBoneCfg.Location = new Point(283, 69);
            button_RandomBoneCfg.Name = "button_RandomBoneCfg";
            button_RandomBoneCfg.Size = new Size(57, 28);
            button_RandomBoneCfg.TabIndex = 69;
            button_RandomBoneCfg.Text = "Cfg";
            button_RandomBoneCfg.UseVisualStyleBackColor = true;
            button_RandomBoneCfg.Click += button_RandomBoneCfg_Click;
            // 
            // flowLayoutPanel_NoRecoil
            // 
            flowLayoutPanel_NoRecoil.AutoSize = true;
            flowLayoutPanel_NoRecoil.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_NoRecoil.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel_NoRecoil.Controls.Add(label16);
            flowLayoutPanel_NoRecoil.Controls.Add(label_Recoil);
            flowLayoutPanel_NoRecoil.Controls.Add(trackBar_NoRecoil);
            flowLayoutPanel_NoRecoil.Controls.Add(label_Sway);
            flowLayoutPanel_NoRecoil.Controls.Add(trackBar_NoSway);
            flowLayoutPanel_NoRecoil.Enabled = false;
            flowLayoutPanel_NoRecoil.Location = new Point(429, 78);
            flowLayoutPanel_NoRecoil.Name = "flowLayoutPanel_NoRecoil";
            flowLayoutPanel_NoRecoil.Size = new Size(289, 68);
            flowLayoutPanel_NoRecoil.TabIndex = 4;
            // 
            // label16
            // 
            label16.AutoSize = true;
            flowLayoutPanel_NoRecoil.SetFlowBreak(label16, true);
            label16.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label16.Location = new Point(3, 0);
            label16.Name = "label16";
            label16.Size = new Size(58, 15);
            label16.TabIndex = 52;
            label16.Text = "No Recoil";
            // 
            // label_Recoil
            // 
            label_Recoil.Anchor = AnchorStyles.Right;
            label_Recoil.AutoSize = true;
            label_Recoil.Location = new Point(3, 33);
            label_Recoil.Name = "label_Recoil";
            label_Recoil.Size = new Size(54, 15);
            label_Recoil.TabIndex = 50;
            label_Recoil.Text = "Recoil 50";
            // 
            // trackBar_NoRecoil
            // 
            trackBar_NoRecoil.Anchor = AnchorStyles.Right;
            trackBar_NoRecoil.BackColor = SystemColors.Window;
            trackBar_NoRecoil.Location = new Point(63, 18);
            trackBar_NoRecoil.Maximum = 100;
            trackBar_NoRecoil.Name = "trackBar_NoRecoil";
            trackBar_NoRecoil.Size = new Size(80, 45);
            trackBar_NoRecoil.TabIndex = 48;
            trackBar_NoRecoil.TickStyle = TickStyle.None;
            trackBar_NoRecoil.Value = 50;
            // 
            // label_Sway
            // 
            label_Sway.Anchor = AnchorStyles.Right;
            label_Sway.AutoSize = true;
            label_Sway.Location = new Point(149, 33);
            label_Sway.Name = "label_Sway";
            label_Sway.Size = new Size(49, 15);
            label_Sway.TabIndex = 51;
            label_Sway.Text = "Sway 30";
            // 
            // trackBar_NoSway
            // 
            trackBar_NoSway.Anchor = AnchorStyles.Right;
            trackBar_NoSway.BackColor = SystemColors.Window;
            trackBar_NoSway.Location = new Point(204, 18);
            trackBar_NoSway.Maximum = 100;
            trackBar_NoSway.Name = "trackBar_NoSway";
            trackBar_NoSway.Size = new Size(80, 45);
            trackBar_NoSway.TabIndex = 49;
            trackBar_NoSway.TickStyle = TickStyle.None;
            trackBar_NoSway.Value = 30;
            // 
            // flowLayoutPanel_Chams
            // 
            flowLayoutPanel_Chams.AutoSize = true;
            flowLayoutPanel_Chams.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_Chams.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel_Chams.Controls.Add(label17);
            flowLayoutPanel_Chams.Controls.Add(radioButton_Chams_Basic);
            flowLayoutPanel_Chams.Controls.Add(radioButton_Chams_Visible);
            flowLayoutPanel_Chams.Controls.Add(radioButton_Chams_Vischeck);
            flowLayoutPanel_Chams.Controls.Add(flowLayoutPanel_Vischeck);
            flowLayoutPanel_Chams.Enabled = false;
            flowLayoutPanel_Chams.Location = new Point(724, 78);
            flowLayoutPanel_Chams.Name = "flowLayoutPanel_Chams";
            flowLayoutPanel_Chams.Size = new Size(457, 108);
            flowLayoutPanel_Chams.TabIndex = 4;
            // 
            // label17
            // 
            label17.AutoSize = true;
            flowLayoutPanel_Chams.SetFlowBreak(label17, true);
            label17.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label17.Location = new Point(3, 0);
            label17.Name = "label17";
            label17.Size = new Size(43, 15);
            label17.TabIndex = 3;
            label17.Text = "Chams";
            // 
            // radioButton_Chams_Basic
            // 
            radioButton_Chams_Basic.Anchor = AnchorStyles.Right;
            radioButton_Chams_Basic.AutoSize = true;
            radioButton_Chams_Basic.Checked = true;
            radioButton_Chams_Basic.Location = new Point(3, 56);
            radioButton_Chams_Basic.Name = "radioButton_Chams_Basic";
            radioButton_Chams_Basic.Size = new Size(52, 19);
            radioButton_Chams_Basic.TabIndex = 1;
            radioButton_Chams_Basic.TabStop = true;
            radioButton_Chams_Basic.Text = "Basic";
            radioButton_Chams_Basic.UseVisualStyleBackColor = true;
            radioButton_Chams_Basic.CheckedChanged += radioButton_Chams_Basic_CheckedChanged;
            // 
            // radioButton_Chams_Visible
            // 
            radioButton_Chams_Visible.Anchor = AnchorStyles.Right;
            radioButton_Chams_Visible.AutoSize = true;
            radioButton_Chams_Visible.Enabled = false;
            radioButton_Chams_Visible.Location = new Point(61, 56);
            radioButton_Chams_Visible.Name = "radioButton_Chams_Visible";
            radioButton_Chams_Visible.Size = new Size(59, 19);
            radioButton_Chams_Visible.TabIndex = 7;
            radioButton_Chams_Visible.Text = "Visible";
            radioButton_Chams_Visible.UseVisualStyleBackColor = true;
            radioButton_Chams_Visible.CheckedChanged += radioButton_Chams_Visible_CheckedChanged;
            // 
            // radioButton_Chams_Vischeck
            // 
            radioButton_Chams_Vischeck.Anchor = AnchorStyles.Right;
            radioButton_Chams_Vischeck.AutoSize = true;
            radioButton_Chams_Vischeck.Enabled = false;
            radioButton_Chams_Vischeck.Location = new Point(126, 56);
            radioButton_Chams_Vischeck.Name = "radioButton_Chams_Vischeck";
            radioButton_Chams_Vischeck.Size = new Size(71, 19);
            radioButton_Chams_Vischeck.TabIndex = 2;
            radioButton_Chams_Vischeck.Text = "Vischeck";
            radioButton_Chams_Vischeck.UseVisualStyleBackColor = true;
            radioButton_Chams_Vischeck.CheckedChanged += radioButton_Chams_Vischeck_CheckedChanged;
            // 
            // flowLayoutPanel_Vischeck
            // 
            flowLayoutPanel_Vischeck.Anchor = AnchorStyles.Right;
            flowLayoutPanel_Vischeck.AutoSize = true;
            flowLayoutPanel_Vischeck.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_Vischeck.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel_Vischeck.Controls.Add(label14);
            flowLayoutPanel_Vischeck.Controls.Add(label15);
            flowLayoutPanel_Vischeck.Controls.Add(textBox_VischeckVisColor);
            flowLayoutPanel_Vischeck.Controls.Add(button_VischeckVisColorPick);
            flowLayoutPanel_Vischeck.Controls.Add(label33);
            flowLayoutPanel_Vischeck.Controls.Add(textBox_VischeckInvisColor);
            flowLayoutPanel_Vischeck.Controls.Add(button_VischeckInvisColorPick);
            flowLayoutPanel_Vischeck.Enabled = false;
            flowLayoutPanel_Vischeck.Location = new Point(203, 28);
            flowLayoutPanel_Vischeck.Name = "flowLayoutPanel_Vischeck";
            flowLayoutPanel_Vischeck.Size = new Size(249, 75);
            flowLayoutPanel_Vischeck.TabIndex = 6;
            // 
            // label14
            // 
            label14.AutoSize = true;
            flowLayoutPanel_Vischeck.SetFlowBreak(label14, true);
            label14.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label14.Location = new Point(3, 0);
            label14.Name = "label14";
            label14.Size = new Size(145, 15);
            label14.TabIndex = 3;
            label14.Text = "Advanced Chams Settings";
            // 
            // label15
            // 
            label15.Anchor = AnchorStyles.Right;
            label15.AutoSize = true;
            label15.Location = new Point(3, 22);
            label15.Name = "label15";
            label15.Size = new Size(73, 15);
            label15.TabIndex = 5;
            label15.Text = "Visible Color";
            // 
            // textBox_VischeckVisColor
            // 
            textBox_VischeckVisColor.Anchor = AnchorStyles.Right;
            textBox_VischeckVisColor.Location = new Point(82, 18);
            textBox_VischeckVisColor.MaxLength = 9;
            textBox_VischeckVisColor.Name = "textBox_VischeckVisColor";
            textBox_VischeckVisColor.Size = new Size(72, 23);
            textBox_VischeckVisColor.TabIndex = 4;
            textBox_VischeckVisColor.TextChanged += textBox_VischeckVisColor_TextChanged;
            // 
            // button_VischeckVisColorPick
            // 
            flowLayoutPanel_Vischeck.SetFlowBreak(button_VischeckVisColorPick, true);
            button_VischeckVisColorPick.Location = new Point(160, 18);
            button_VischeckVisColorPick.Name = "button_VischeckVisColorPick";
            button_VischeckVisColorPick.Size = new Size(75, 23);
            button_VischeckVisColorPick.TabIndex = 8;
            button_VischeckVisColorPick.Text = "Color";
            button_VischeckVisColorPick.UseVisualStyleBackColor = true;
            button_VischeckVisColorPick.Click += button_VischeckVisColorPick_Click;
            // 
            // label33
            // 
            label33.Anchor = AnchorStyles.Right;
            label33.AutoSize = true;
            label33.Location = new Point(3, 51);
            label33.Name = "label33";
            label33.Size = new Size(82, 15);
            label33.TabIndex = 7;
            label33.Text = "Invisible Color";
            // 
            // textBox_VischeckInvisColor
            // 
            textBox_VischeckInvisColor.Anchor = AnchorStyles.Right;
            textBox_VischeckInvisColor.Location = new Point(91, 47);
            textBox_VischeckInvisColor.MaxLength = 9;
            textBox_VischeckInvisColor.Name = "textBox_VischeckInvisColor";
            textBox_VischeckInvisColor.Size = new Size(72, 23);
            textBox_VischeckInvisColor.TabIndex = 6;
            textBox_VischeckInvisColor.TextChanged += textBox_VischeckInvisColor_TextChanged;
            // 
            // button_VischeckInvisColorPick
            // 
            button_VischeckInvisColorPick.Location = new Point(169, 47);
            button_VischeckInvisColorPick.Name = "button_VischeckInvisColorPick";
            button_VischeckInvisColorPick.Size = new Size(75, 23);
            button_VischeckInvisColorPick.TabIndex = 9;
            button_VischeckInvisColorPick.Text = "Color";
            button_VischeckInvisColorPick.UseVisualStyleBackColor = true;
            button_VischeckInvisColorPick.Click += button_VischeckInvisColorPick_Click;
            // 
            // flowLayoutPanel_MonitorSettings
            // 
            flowLayoutPanel_MonitorSettings.AutoSize = true;
            flowLayoutPanel_MonitorSettings.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_MonitorSettings.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel_MonitorSettings.Controls.Add(label11);
            flowLayoutPanel_MonitorSettings.Controls.Add(label_Width);
            flowLayoutPanel_MonitorSettings.Controls.Add(textBox_ResWidth);
            flowLayoutPanel_MonitorSettings.Controls.Add(label_Height);
            flowLayoutPanel_MonitorSettings.Controls.Add(textBox_ResHeight);
            flowLayoutPanel_MonitorSettings.Controls.Add(button_DetectRes);
            flowLayoutPanel_MonitorSettings.Dock = DockStyle.Top;
            flowLayoutPanel_Settings.SetFlowBreak(flowLayoutPanel_MonitorSettings, true);
            flowLayoutPanel_MonitorSettings.Location = new Point(3, 410);
            flowLayoutPanel_MonitorSettings.Name = "flowLayoutPanel_MonitorSettings";
            flowLayoutPanel_MonitorSettings.Size = new Size(1186, 70);
            flowLayoutPanel_MonitorSettings.TabIndex = 2;
            // 
            // label11
            // 
            label11.AutoSize = true;
            flowLayoutPanel_MonitorSettings.SetFlowBreak(label11, true);
            label11.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label11.Location = new Point(3, 0);
            label11.Name = "label11";
            label11.Size = new Size(206, 21);
            label11.TabIndex = 46;
            label11.Text = "Monitor Info (Aimbot/ESP)";
            // 
            // label_Width
            // 
            label_Width.Anchor = AnchorStyles.Right;
            label_Width.AutoSize = true;
            label_Width.Location = new Point(3, 37);
            label_Width.Name = "label_Width";
            label_Width.Size = new Size(39, 15);
            label_Width.TabIndex = 2;
            label_Width.Text = "Width";
            // 
            // textBox_ResWidth
            // 
            textBox_ResWidth.Anchor = AnchorStyles.Right;
            textBox_ResWidth.Location = new Point(48, 33);
            textBox_ResWidth.MaxLength = 5;
            textBox_ResWidth.Name = "textBox_ResWidth";
            textBox_ResWidth.Size = new Size(51, 23);
            textBox_ResWidth.TabIndex = 0;
            textBox_ResWidth.Text = "1920";
            textBox_ResWidth.TextAlign = HorizontalAlignment.Center;
            textBox_ResWidth.TextChanged += textBox_ResWidth_TextChanged;
            // 
            // label_Height
            // 
            label_Height.Anchor = AnchorStyles.Right;
            label_Height.AutoSize = true;
            label_Height.Location = new Point(105, 37);
            label_Height.Name = "label_Height";
            label_Height.Size = new Size(43, 15);
            label_Height.TabIndex = 3;
            label_Height.Text = "Height";
            // 
            // textBox_ResHeight
            // 
            textBox_ResHeight.Anchor = AnchorStyles.Right;
            textBox_ResHeight.Location = new Point(154, 33);
            textBox_ResHeight.MaxLength = 5;
            textBox_ResHeight.Name = "textBox_ResHeight";
            textBox_ResHeight.Size = new Size(51, 23);
            textBox_ResHeight.TabIndex = 1;
            textBox_ResHeight.Text = "1080";
            textBox_ResHeight.TextAlign = HorizontalAlignment.Center;
            textBox_ResHeight.TextChanged += textBox_ResHeight_TextChanged;
            // 
            // button_DetectRes
            // 
            button_DetectRes.Location = new Point(211, 24);
            button_DetectRes.Name = "button_DetectRes";
            button_DetectRes.Size = new Size(107, 41);
            button_DetectRes.TabIndex = 4;
            button_DetectRes.Text = "Auto-Detect";
            button_DetectRes.UseVisualStyleBackColor = true;
            button_DetectRes.Click += button_DetectRes_Click;
            // 
            // flowLayoutPanel_ESPSettings
            // 
            flowLayoutPanel_ESPSettings.AutoSize = true;
            flowLayoutPanel_ESPSettings.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_ESPSettings.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel_ESPSettings.Controls.Add(label12);
            flowLayoutPanel_ESPSettings.Controls.Add(label7);
            flowLayoutPanel_ESPSettings.Controls.Add(button_StartESP);
            flowLayoutPanel_ESPSettings.Controls.Add(button_EspColorPicker);
            flowLayoutPanel_ESPSettings.Controls.Add(label_ESPFPSCap);
            flowLayoutPanel_ESPSettings.Controls.Add(textBox_EspFpsCap);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_AutoFS);
            flowLayoutPanel_ESPSettings.Controls.Add(comboBox_ESPAutoFS);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_Grenades);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_ShowMag);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_HighAlert);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_FireportAim);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_AimFov);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_AimLock);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_StatusText);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_FPS);
            flowLayoutPanel_ESPSettings.Controls.Add(flowLayoutPanel_ESP_PlayerRender);
            flowLayoutPanel_ESPSettings.Controls.Add(flowLayoutPanel4);
            flowLayoutPanel_ESPSettings.Dock = DockStyle.Top;
            flowLayoutPanel_Settings.SetFlowBreak(flowLayoutPanel_ESPSettings, true);
            flowLayoutPanel_ESPSettings.Location = new Point(3, 486);
            flowLayoutPanel_ESPSettings.Name = "flowLayoutPanel_ESPSettings";
            flowLayoutPanel_ESPSettings.Size = new Size(1186, 178);
            flowLayoutPanel_ESPSettings.TabIndex = 3;
            // 
            // label12
            // 
            label12.AutoSize = true;
            flowLayoutPanel_ESPSettings.SetFlowBreak(label12, true);
            label12.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label12.Location = new Point(3, 0);
            label12.Name = "label12";
            label12.Size = new Size(79, 21);
            label12.TabIndex = 69;
            label12.Text = "Fuser ESP";
            // 
            // label7
            // 
            label7.Location = new Point(3, 21);
            label7.Name = "label7";
            label7.Size = new Size(0, 0);
            label7.TabIndex = 71;
            label7.Text = "label7";
            // 
            // button_StartESP
            // 
            button_StartESP.Anchor = AnchorStyles.Right;
            button_StartESP.Location = new Point(9, 24);
            button_StartESP.Name = "button_StartESP";
            button_StartESP.Size = new Size(107, 41);
            button_StartESP.TabIndex = 0;
            button_StartESP.Text = "Start ESP";
            button_StartESP.UseVisualStyleBackColor = true;
            button_StartESP.Click += button_StartESP_Click;
            // 
            // button_EspColorPicker
            // 
            button_EspColorPicker.Anchor = AnchorStyles.Right;
            button_EspColorPicker.Location = new Point(122, 24);
            button_EspColorPicker.Name = "button_EspColorPicker";
            button_EspColorPicker.Size = new Size(107, 41);
            button_EspColorPicker.TabIndex = 47;
            button_EspColorPicker.Text = "Color Picker";
            button_EspColorPicker.UseVisualStyleBackColor = true;
            button_EspColorPicker.Click += button_EspColorPicker_Click;
            // 
            // label_ESPFPSCap
            // 
            label_ESPFPSCap.Anchor = AnchorStyles.Right;
            label_ESPFPSCap.AutoSize = true;
            label_ESPFPSCap.Location = new Point(235, 37);
            label_ESPFPSCap.Name = "label_ESPFPSCap";
            label_ESPFPSCap.Size = new Size(50, 15);
            label_ESPFPSCap.TabIndex = 46;
            label_ESPFPSCap.Text = "FPS Cap";
            // 
            // textBox_EspFpsCap
            // 
            textBox_EspFpsCap.Anchor = AnchorStyles.Right;
            textBox_EspFpsCap.Location = new Point(291, 33);
            textBox_EspFpsCap.MaxLength = 4;
            textBox_EspFpsCap.Name = "textBox_EspFpsCap";
            textBox_EspFpsCap.Size = new Size(63, 23);
            textBox_EspFpsCap.TabIndex = 45;
            textBox_EspFpsCap.Text = "60";
            textBox_EspFpsCap.TextAlign = HorizontalAlignment.Center;
            textBox_EspFpsCap.TextChanged += textBox_EspFpsCap_TextChanged;
            // 
            // checkBox_ESP_AutoFS
            // 
            checkBox_ESP_AutoFS.Anchor = AnchorStyles.Right;
            checkBox_ESP_AutoFS.AutoSize = true;
            checkBox_ESP_AutoFS.Location = new Point(360, 35);
            checkBox_ESP_AutoFS.Name = "checkBox_ESP_AutoFS";
            checkBox_ESP_AutoFS.Size = new Size(108, 19);
            checkBox_ESP_AutoFS.TabIndex = 55;
            checkBox_ESP_AutoFS.Text = "Auto Fullscreen";
            checkBox_ESP_AutoFS.UseVisualStyleBackColor = true;
            checkBox_ESP_AutoFS.CheckedChanged += checkBox_ESP_AutoFS_CheckedChanged;
            // 
            // comboBox_ESPAutoFS
            // 
            comboBox_ESPAutoFS.Anchor = AnchorStyles.Right;
            comboBox_ESPAutoFS.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_ESPAutoFS.Enabled = false;
            flowLayoutPanel_ESPSettings.SetFlowBreak(comboBox_ESPAutoFS, true);
            comboBox_ESPAutoFS.FormattingEnabled = true;
            comboBox_ESPAutoFS.Location = new Point(474, 33);
            comboBox_ESPAutoFS.Name = "comboBox_ESPAutoFS";
            comboBox_ESPAutoFS.Size = new Size(121, 23);
            comboBox_ESPAutoFS.TabIndex = 56;
            comboBox_ESPAutoFS.SelectedIndexChanged += comboBox_ESPAutoFS_SelectedIndexChanged;
            // 
            // checkBox_ESP_Grenades
            // 
            checkBox_ESP_Grenades.AutoSize = true;
            checkBox_ESP_Grenades.Location = new Point(3, 71);
            checkBox_ESP_Grenades.Name = "checkBox_ESP_Grenades";
            checkBox_ESP_Grenades.Size = new Size(107, 19);
            checkBox_ESP_Grenades.TabIndex = 8;
            checkBox_ESP_Grenades.Text = "Show Grenades";
            checkBox_ESP_Grenades.UseVisualStyleBackColor = true;
            checkBox_ESP_Grenades.CheckedChanged += checkBox_ESP_Grenades_CheckedChanged;
            // 
            // checkBox_ESP_ShowMag
            // 
            checkBox_ESP_ShowMag.AutoSize = true;
            checkBox_ESP_ShowMag.Location = new Point(116, 71);
            checkBox_ESP_ShowMag.Name = "checkBox_ESP_ShowMag";
            checkBox_ESP_ShowMag.Size = new Size(109, 19);
            checkBox_ESP_ShowMag.TabIndex = 60;
            checkBox_ESP_ShowMag.Text = "Show Magazine";
            checkBox_ESP_ShowMag.UseVisualStyleBackColor = true;
            checkBox_ESP_ShowMag.CheckedChanged += checkBox_ESP_ShowMag_CheckedChanged;
            // 
            // checkBox_ESP_HighAlert
            // 
            checkBox_ESP_HighAlert.AutoSize = true;
            checkBox_ESP_HighAlert.Location = new Point(231, 71);
            checkBox_ESP_HighAlert.Name = "checkBox_ESP_HighAlert";
            checkBox_ESP_HighAlert.Size = new Size(80, 19);
            checkBox_ESP_HighAlert.TabIndex = 70;
            checkBox_ESP_HighAlert.Text = "High Alert";
            checkBox_ESP_HighAlert.UseVisualStyleBackColor = true;
            checkBox_ESP_HighAlert.CheckedChanged += checkBox_ESP_HighAlert_CheckedChanged;
            // 
            // checkBox_ESP_FireportAim
            // 
            checkBox_ESP_FireportAim.AutoSize = true;
            checkBox_ESP_FireportAim.Location = new Point(317, 71);
            checkBox_ESP_FireportAim.Name = "checkBox_ESP_FireportAim";
            checkBox_ESP_FireportAim.Size = new Size(124, 19);
            checkBox_ESP_FireportAim.TabIndex = 72;
            checkBox_ESP_FireportAim.Text = "Show Fireport Aim";
            checkBox_ESP_FireportAim.UseVisualStyleBackColor = true;
            checkBox_ESP_FireportAim.CheckedChanged += checkBox_ESP_FireportAim_CheckedChanged;
            // 
            // checkBox_ESP_AimFov
            // 
            checkBox_ESP_AimFov.AutoSize = true;
            checkBox_ESP_AimFov.Location = new Point(447, 71);
            checkBox_ESP_AimFov.Name = "checkBox_ESP_AimFov";
            checkBox_ESP_AimFov.Size = new Size(105, 19);
            checkBox_ESP_AimFov.TabIndex = 43;
            checkBox_ESP_AimFov.Text = "Show Aim FOV";
            checkBox_ESP_AimFov.UseVisualStyleBackColor = true;
            checkBox_ESP_AimFov.CheckedChanged += checkBox_ESP_AimFov_CheckedChanged;
            // 
            // checkBox_ESP_AimLock
            // 
            checkBox_ESP_AimLock.AutoSize = true;
            checkBox_ESP_AimLock.Location = new Point(558, 71);
            checkBox_ESP_AimLock.Name = "checkBox_ESP_AimLock";
            checkBox_ESP_AimLock.Size = new Size(126, 19);
            checkBox_ESP_AimLock.TabIndex = 49;
            checkBox_ESP_AimLock.Text = "Show Aimbot Lock";
            checkBox_ESP_AimLock.UseVisualStyleBackColor = true;
            checkBox_ESP_AimLock.CheckedChanged += checkBox_ESP_AimLock_CheckedChanged;
            // 
            // checkBox_ESP_StatusText
            // 
            checkBox_ESP_StatusText.AutoSize = true;
            checkBox_ESP_StatusText.Location = new Point(690, 71);
            checkBox_ESP_StatusText.Name = "checkBox_ESP_StatusText";
            checkBox_ESP_StatusText.Size = new Size(114, 19);
            checkBox_ESP_StatusText.TabIndex = 73;
            checkBox_ESP_StatusText.Text = "Show Status Text";
            checkBox_ESP_StatusText.UseVisualStyleBackColor = true;
            checkBox_ESP_StatusText.CheckedChanged += checkBox_ESP_StatusText_CheckedChanged;
            // 
            // checkBox_ESP_FPS
            // 
            checkBox_ESP_FPS.AutoSize = true;
            flowLayoutPanel_ESPSettings.SetFlowBreak(checkBox_ESP_FPS, true);
            checkBox_ESP_FPS.Location = new Point(810, 71);
            checkBox_ESP_FPS.Name = "checkBox_ESP_FPS";
            checkBox_ESP_FPS.Size = new Size(86, 19);
            checkBox_ESP_FPS.TabIndex = 5;
            checkBox_ESP_FPS.Text = "Display FPS";
            checkBox_ESP_FPS.UseVisualStyleBackColor = true;
            checkBox_ESP_FPS.CheckedChanged += checkBox_ESP_FPS_CheckedChanged;
            // 
            // flowLayoutPanel_ESP_PlayerRender
            // 
            flowLayoutPanel_ESP_PlayerRender.Anchor = AnchorStyles.Right;
            flowLayoutPanel_ESP_PlayerRender.AutoSize = true;
            flowLayoutPanel_ESP_PlayerRender.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_ESP_PlayerRender.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel_ESP_PlayerRender.Controls.Add(label18);
            flowLayoutPanel_ESP_PlayerRender.Controls.Add(radioButton_ESPRender_None);
            flowLayoutPanel_ESP_PlayerRender.Controls.Add(radioButton_ESPRender_Bones);
            flowLayoutPanel_ESP_PlayerRender.Controls.Add(checkBox_ESPRender_Labels);
            flowLayoutPanel_ESP_PlayerRender.Controls.Add(checkBox_ESPRender_Weapons);
            flowLayoutPanel_ESP_PlayerRender.Controls.Add(checkBox_ESPRender_Dist);
            flowLayoutPanel_ESP_PlayerRender.Location = new Point(3, 96);
            flowLayoutPanel_ESP_PlayerRender.Name = "flowLayoutPanel_ESP_PlayerRender";
            flowLayoutPanel_ESP_PlayerRender.Size = new Size(200, 77);
            flowLayoutPanel_ESP_PlayerRender.TabIndex = 5;
            // 
            // label18
            // 
            label18.AutoSize = true;
            flowLayoutPanel_ESP_PlayerRender.SetFlowBreak(label18, true);
            label18.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label18.Location = new Point(3, 0);
            label18.Name = "label18";
            label18.Size = new Size(113, 15);
            label18.TabIndex = 71;
            label18.Text = "Player Render Mode";
            // 
            // radioButton_ESPRender_None
            // 
            radioButton_ESPRender_None.AutoSize = true;
            radioButton_ESPRender_None.Location = new Point(3, 28);
            radioButton_ESPRender_None.Name = "radioButton_ESPRender_None";
            radioButton_ESPRender_None.Size = new Size(54, 19);
            radioButton_ESPRender_None.TabIndex = 63;
            radioButton_ESPRender_None.Text = "None";
            radioButton_ESPRender_None.UseVisualStyleBackColor = true;
            radioButton_ESPRender_None.CheckedChanged += radioButton_ESPRender_None_CheckedChanged;
            // 
            // radioButton_ESPRender_Bones
            // 
            radioButton_ESPRender_Bones.AutoSize = true;
            radioButton_ESPRender_Bones.Checked = true;
            flowLayoutPanel_ESP_PlayerRender.SetFlowBreak(radioButton_ESPRender_Bones, true);
            radioButton_ESPRender_Bones.Location = new Point(63, 28);
            radioButton_ESPRender_Bones.Name = "radioButton_ESPRender_Bones";
            radioButton_ESPRender_Bones.Size = new Size(57, 19);
            radioButton_ESPRender_Bones.TabIndex = 64;
            radioButton_ESPRender_Bones.TabStop = true;
            radioButton_ESPRender_Bones.Text = "Bones";
            radioButton_ESPRender_Bones.UseVisualStyleBackColor = true;
            radioButton_ESPRender_Bones.CheckedChanged += radioButton_ESPRender_Bones_CheckedChanged;
            // 
            // checkBox_ESPRender_Labels
            // 
            checkBox_ESPRender_Labels.AutoSize = true;
            checkBox_ESPRender_Labels.Location = new Point(3, 53);
            checkBox_ESPRender_Labels.Name = "checkBox_ESPRender_Labels";
            checkBox_ESPRender_Labels.Size = new Size(59, 19);
            checkBox_ESPRender_Labels.TabIndex = 68;
            checkBox_ESPRender_Labels.Text = "Labels";
            checkBox_ESPRender_Labels.UseVisualStyleBackColor = true;
            checkBox_ESPRender_Labels.CheckedChanged += checkBox_ESPRender_Labels_CheckedChanged;
            // 
            // checkBox_ESPRender_Weapons
            // 
            checkBox_ESPRender_Weapons.AutoSize = true;
            checkBox_ESPRender_Weapons.Location = new Point(68, 53);
            checkBox_ESPRender_Weapons.Name = "checkBox_ESPRender_Weapons";
            checkBox_ESPRender_Weapons.Size = new Size(75, 19);
            checkBox_ESPRender_Weapons.TabIndex = 69;
            checkBox_ESPRender_Weapons.Text = "Weapons";
            checkBox_ESPRender_Weapons.UseVisualStyleBackColor = true;
            checkBox_ESPRender_Weapons.CheckedChanged += checkBox_ESPRender_Weapons_CheckedChanged;
            // 
            // checkBox_ESPRender_Dist
            // 
            checkBox_ESPRender_Dist.AutoSize = true;
            checkBox_ESPRender_Dist.Location = new Point(149, 53);
            checkBox_ESPRender_Dist.Name = "checkBox_ESPRender_Dist";
            checkBox_ESPRender_Dist.Size = new Size(46, 19);
            checkBox_ESPRender_Dist.TabIndex = 70;
            checkBox_ESPRender_Dist.Text = "Dist";
            checkBox_ESPRender_Dist.UseVisualStyleBackColor = true;
            checkBox_ESPRender_Dist.CheckedChanged += checkBox_ESPRender_Dist_CheckedChanged;
            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.Anchor = AnchorStyles.Right;
            flowLayoutPanel4.AutoSize = true;
            flowLayoutPanel4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel4.Controls.Add(label_EspFontScale);
            flowLayoutPanel4.Controls.Add(trackBar_EspFontScale);
            flowLayoutPanel4.Controls.Add(label_EspLineScale);
            flowLayoutPanel4.Controls.Add(trackBar_EspLineScale);
            flowLayoutPanel4.Location = new Point(209, 109);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new Size(346, 51);
            flowLayoutPanel4.TabIndex = 4;
            // 
            // label_EspFontScale
            // 
            label_EspFontScale.Anchor = AnchorStyles.Right;
            label_EspFontScale.AutoSize = true;
            label_EspFontScale.Location = new Point(3, 18);
            label_EspFontScale.Name = "label_EspFontScale";
            label_EspFontScale.Size = new Size(85, 15);
            label_EspFontScale.TabIndex = 16;
            label_EspFontScale.Text = "Font Scale 1.00";
            // 
            // trackBar_EspFontScale
            // 
            trackBar_EspFontScale.Anchor = AnchorStyles.Right;
            trackBar_EspFontScale.BackColor = SystemColors.Window;
            trackBar_EspFontScale.Location = new Point(94, 3);
            trackBar_EspFontScale.Maximum = 200;
            trackBar_EspFontScale.Minimum = 50;
            trackBar_EspFontScale.Name = "trackBar_EspFontScale";
            trackBar_EspFontScale.Size = new Size(77, 45);
            trackBar_EspFontScale.TabIndex = 42;
            trackBar_EspFontScale.TickStyle = TickStyle.None;
            trackBar_EspFontScale.Value = 100;
            // 
            // label_EspLineScale
            // 
            label_EspLineScale.Anchor = AnchorStyles.Right;
            label_EspLineScale.AutoSize = true;
            label_EspLineScale.Location = new Point(177, 18);
            label_EspLineScale.Name = "label_EspLineScale";
            label_EspLineScale.Size = new Size(83, 15);
            label_EspLineScale.TabIndex = 43;
            label_EspLineScale.Text = "Line Scale 1.00";
            // 
            // trackBar_EspLineScale
            // 
            trackBar_EspLineScale.Anchor = AnchorStyles.Right;
            trackBar_EspLineScale.BackColor = SystemColors.Window;
            trackBar_EspLineScale.Location = new Point(266, 3);
            trackBar_EspLineScale.Maximum = 200;
            trackBar_EspLineScale.Minimum = 10;
            trackBar_EspLineScale.Name = "trackBar_EspLineScale";
            trackBar_EspLineScale.Size = new Size(77, 45);
            trackBar_EspLineScale.TabIndex = 44;
            trackBar_EspLineScale.TickStyle = TickStyle.None;
            trackBar_EspLineScale.Value = 100;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.Black;
            tabPage1.Controls.Add(checkBox_MapFree);
            tabPage1.Controls.Add(groupBox_MapSetup);
            tabPage1.Controls.Add(skglControl_Radar);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1256, 653);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Radar";
            // 
            // checkBox_MapFree
            // 
            checkBox_MapFree.Appearance = Appearance.Button;
            checkBox_MapFree.AutoSize = true;
            checkBox_MapFree.Location = new Point(6, 6);
            checkBox_MapFree.Name = "checkBox_MapFree";
            checkBox_MapFree.Size = new Size(66, 25);
            checkBox_MapFree.TabIndex = 17;
            checkBox_MapFree.Text = "Map Free";
            checkBox_MapFree.UseVisualStyleBackColor = true;
            checkBox_MapFree.CheckedChanged += checkBox_MapFree_CheckedChanged;
            // 
            // groupBox_MapSetup
            // 
            groupBox_MapSetup.BackColor = Color.WhiteSmoke;
            groupBox_MapSetup.Controls.Add(button_MapSetupApply);
            groupBox_MapSetup.Controls.Add(textBox_mapScale);
            groupBox_MapSetup.Controls.Add(label5);
            groupBox_MapSetup.Controls.Add(textBox_mapY);
            groupBox_MapSetup.Controls.Add(label4);
            groupBox_MapSetup.Controls.Add(textBox_mapX);
            groupBox_MapSetup.Controls.Add(label_Pos);
            groupBox_MapSetup.Location = new Point(6, 36);
            groupBox_MapSetup.Name = "groupBox_MapSetup";
            groupBox_MapSetup.Size = new Size(327, 175);
            groupBox_MapSetup.TabIndex = 11;
            groupBox_MapSetup.TabStop = false;
            groupBox_MapSetup.Text = "Map Setup";
            groupBox_MapSetup.Visible = false;
            // 
            // button_MapSetupApply
            // 
            button_MapSetupApply.Location = new Point(6, 143);
            button_MapSetupApply.Name = "button_MapSetupApply";
            button_MapSetupApply.Size = new Size(75, 23);
            button_MapSetupApply.TabIndex = 16;
            button_MapSetupApply.Text = "Apply";
            button_MapSetupApply.UseVisualStyleBackColor = true;
            button_MapSetupApply.Click += button_MapSetupApply_Click;
            // 
            // textBox_mapScale
            // 
            textBox_mapScale.Location = new Point(46, 101);
            textBox_mapScale.Name = "textBox_mapScale";
            textBox_mapScale.Size = new Size(50, 23);
            textBox_mapScale.TabIndex = 15;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 104);
            label5.Name = "label5";
            label5.Size = new Size(34, 15);
            label5.TabIndex = 14;
            label5.Text = "Scale";
            // 
            // textBox_mapY
            // 
            textBox_mapY.Location = new Point(102, 67);
            textBox_mapY.Name = "textBox_mapY";
            textBox_mapY.Size = new Size(50, 23);
            textBox_mapY.TabIndex = 13;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 70);
            label4.Name = "label4";
            label4.Size = new Size(24, 15);
            label4.TabIndex = 12;
            label4.Text = "X,Y";
            // 
            // textBox_mapX
            // 
            textBox_mapX.Location = new Point(46, 67);
            textBox_mapX.Name = "textBox_mapX";
            textBox_mapX.Size = new Size(50, 23);
            textBox_mapX.TabIndex = 11;
            // 
            // label_Pos
            // 
            label_Pos.AutoSize = true;
            label_Pos.Location = new Point(7, 19);
            label_Pos.Margin = new Padding(4, 0, 4, 0);
            label_Pos.Name = "label_Pos";
            label_Pos.Size = new Size(43, 15);
            label_Pos.TabIndex = 10;
            label_Pos.Text = "coords";
            // 
            // skglControl_Radar
            // 
            skglControl_Radar.BackColor = Color.Black;
            skglControl_Radar.Dock = DockStyle.Fill;
            skglControl_Radar.Location = new Point(3, 3);
            skglControl_Radar.Margin = new Padding(4, 3, 4, 3);
            skglControl_Radar.Name = "skglControl_Radar";
            skglControl_Radar.Size = new Size(1250, 647);
            skglControl_Radar.TabIndex = 18;
            skglControl_Radar.VSync = false;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1264, 681);
            tabControl1.TabIndex = 8;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 681);
            Controls.Add(tabControl1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            Name = "MainForm";
            Text = "Arena DMA Radar";
            tabPage2.ResumeLayout(false);
            flowLayoutPanel_Settings.ResumeLayout(false);
            flowLayoutPanel_Settings.PerformLayout();
            flowLayoutPanel_RadarSettings.ResumeLayout(false);
            flowLayoutPanel_RadarSettings.PerformLayout();
            ((ISupportInitialize)trackBar_AimlineLength).EndInit();
            ((ISupportInitialize)trackBar_UIScale).EndInit();
            flowLayoutPanel_MemWriteCheckbox.ResumeLayout(false);
            flowLayoutPanel_MemWriteCheckbox.PerformLayout();
            flowLayoutPanel_MemWrites.ResumeLayout(false);
            flowLayoutPanel_MemWrites.PerformLayout();
            flowLayoutPanel_Aimbot.ResumeLayout(false);
            flowLayoutPanel_Aimbot.PerformLayout();
            ((ISupportInitialize)trackBar_AimFOV).EndInit();
            flowLayoutPanel_NoRecoil.ResumeLayout(false);
            flowLayoutPanel_NoRecoil.PerformLayout();
            ((ISupportInitialize)trackBar_NoRecoil).EndInit();
            ((ISupportInitialize)trackBar_NoSway).EndInit();
            flowLayoutPanel_Chams.ResumeLayout(false);
            flowLayoutPanel_Chams.PerformLayout();
            flowLayoutPanel_Vischeck.ResumeLayout(false);
            flowLayoutPanel_Vischeck.PerformLayout();
            flowLayoutPanel_MonitorSettings.ResumeLayout(false);
            flowLayoutPanel_MonitorSettings.PerformLayout();
            flowLayoutPanel_ESPSettings.ResumeLayout(false);
            flowLayoutPanel_ESPSettings.PerformLayout();
            flowLayoutPanel_ESP_PlayerRender.ResumeLayout(false);
            flowLayoutPanel_ESP_PlayerRender.PerformLayout();
            flowLayoutPanel4.ResumeLayout(false);
            flowLayoutPanel4.PerformLayout();
            ((ISupportInitialize)trackBar_EspFontScale).EndInit();
            ((ISupportInitialize)trackBar_EspLineScale).EndInit();
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            groupBox_MapSetup.ResumeLayout(false);
            groupBox_MapSetup.PerformLayout();
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private ColorDialog colorDialog1;
        private ToolTip toolTip1;
        private TabPage tabPage2;
        private FlowLayoutPanel flowLayoutPanel_Settings;
        private FlowLayoutPanel flowLayoutPanel_RadarSettings;
        private Label label2;
        private Label label1;
        private Button button_Restart;
        private Button button_HotkeyManager;
        private Button button_Radar_ColorPicker;
        private Button button_BackupConfig;
        private Label label_AimlineLength;
        private TrackBar trackBar_AimlineLength;
        private Label label_UIScale;
        private TrackBar trackBar_UIScale;
        private CheckBox checkBox_MapSetup;
        private CheckBox checkBox_Aimview;
        private CheckBox checkBox_GrpConnect;
        private CheckBox checkBox_HideNames;
        private CheckBox checkBox_EnableMemWrite;
        private FlowLayoutPanel flowLayoutPanel_MemWrites;
        private Label label3;
        private CheckBox checkBox_AimBotEnabled;
        private CheckBox checkBox_NoRecoilSway;
        private CheckBox checkBox_Chams;
        private CheckBox checkBox_NoVisor;
        private FlowLayoutPanel flowLayoutPanel_Aimbot;
        private Label label13;
        private CheckBox checkBox_SA_AutoBone;
        private CheckBox checkBox_SA_SafeLock;
        private RadioButton radioButton_AimTarget_FOV;
        private RadioButton radioButton_AimTarget_CQB;
        private Label label_AimFOV;
        private TrackBar trackBar_AimFOV;
        private Label label10;
        private ComboBox comboBox_AimbotTarget;
        private CheckBox checkBox_AimRandomBone;
        private Button button_RandomBoneCfg;
        private FlowLayoutPanel flowLayoutPanel_NoRecoil;
        private Label label16;
        private Label label_Recoil;
        private TrackBar trackBar_NoRecoil;
        private Label label_Sway;
        private TrackBar trackBar_NoSway;
        private FlowLayoutPanel flowLayoutPanel_Chams;
        private Label label17;
        private RadioButton radioButton_Chams_Basic;
        private RadioButton radioButton_Chams_Vischeck;
        private FlowLayoutPanel flowLayoutPanel_MonitorSettings;
        private Label label11;
        private Label label_Width;
        private TextBox textBox_ResWidth;
        private Label label_Height;
        private TextBox textBox_ResHeight;
        private Button button_DetectRes;
        private FlowLayoutPanel flowLayoutPanel_ESPSettings;
        private Label label12;
        private Button button_StartESP;
        private Button button_EspColorPicker;
        private Label label_ESPFPSCap;
        private TextBox textBox_EspFpsCap;
        private CheckBox checkBox_ESP_AutoFS;
        private ComboBox comboBox_ESPAutoFS;
        private CheckBox checkBox_ESP_ShowMag;
        private CheckBox checkBox_ESP_AimFov;
        private CheckBox checkBox_ESP_AimLock;
        private CheckBox checkBox_ESP_FPS;
        private FlowLayoutPanel flowLayoutPanel_ESP_PlayerRender;
        private Label label18;
        private RadioButton radioButton_ESPRender_None;
        private RadioButton radioButton_ESPRender_Bones;
        private CheckBox checkBox_ESPRender_Labels;
        private CheckBox checkBox_ESPRender_Weapons;
        private CheckBox checkBox_ESPRender_Dist;
        private FlowLayoutPanel flowLayoutPanel4;
        private Label label_EspFontScale;
        private TrackBar trackBar_EspFontScale;
        private TabPage tabPage1;
        private CheckBox checkBox_MapFree;
        private GroupBox groupBox_MapSetup;
        private Button button_MapSetupApply;
        private TextBox textBox_mapScale;
        private Label label5;
        private TextBox textBox_mapY;
        private Label label4;
        private TextBox textBox_mapX;
        private Label label_Pos;
        private SKGLControl skglControl_Radar;
        private TabControl tabControl1;
        private CheckBox checkBox_ESP_Grenades;
        private CheckBox checkBox_ESP_HighAlert;
        private Label label6;
        private Label label7;
        private Label label8;
        private CheckBox checkBox_ESP_FireportAim;
        private Label label_EspLineScale;
        private TrackBar trackBar_EspLineScale;
        private CheckBox checkBox_ESP_StatusText;
        private CheckBox checkBox_NoWepMalf;
        private CheckBox checkBox_AdvancedMemWrites;
        private FlowLayoutPanel flowLayoutPanel_Vischeck;
        private Label label14;
        private Label label15;
        private TextBox textBox_VischeckVisColor;
        private Button button_VischeckVisColorPick;
        private Label label33;
        private TextBox textBox_VischeckInvisColor;
        private Button button_VischeckInvisColorPick;
        private RadioButton radioButton_Chams_Visible;
        private FlowLayoutPanel flowLayoutPanel_MemWriteCheckbox;
        private LinkLabel linkLabel_CheckForUpdates;
    }
}

