using eft_dma_radar.UI.Misc;

namespace eft_dma_radar.UI.Radar
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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            flowLayoutPanel_Loot = new FlowLayoutPanel();
            label20 = new Label();
            label6 = new Label();
            label7 = new Label();
            textBox_LootRegValue = new TextBox();
            label8 = new Label();
            textBox_LootImpValue = new TextBox();
            checkBox_LootPPS = new CheckBox();
            radioButton_Loot_FleaPrice = new RadioButton();
            radioButton_Loot_VendorPrice = new RadioButton();
            checkBox_HideCorpses = new CheckBox();
            checkBox_ShowMeds = new CheckBox();
            checkBox_ShowFood = new CheckBox();
            checkBox_ShowBackpacks = new CheckBox();
            textBox_LootFilterByName = new TextBox();
            button_Loot = new Button();
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
            tabPage2 = new TabPage();
            flowLayoutPanel_Settings = new FlowLayoutPanel();
            flowLayoutPanel_RadarSettings = new FlowLayoutPanel();
            label2 = new Label();
            label24 = new Label();
            linkLabel_CheckForUpdates = new LinkLabel();
            label1 = new Label();
            button_Restart = new Button();
            button_HotkeyManager = new Button();
            button_Radar_ColorPicker = new Button();
            button_BackupConfig = new Button();
            label_AimlineLength = new Label();
            trackBar_AimlineLength = new TrackBar();
            label_MaxDist = new Label();
            trackBar_MaxDist = new TrackBar();
            label_UIScale = new Label();
            trackBar_UIScale = new TrackBar();
            label_ContainerDist = new Label();
            trackBar_ContainerDist = new TrackBar();
            checkBox_MapSetup = new CheckBox();
            checkBox_Aimview = new CheckBox();
            checkBox_ShowInfoTab = new CheckBox();
            checkBox_GrpConnect = new CheckBox();
            checkBox_HideNames = new CheckBox();
            checkBox_ShowMines = new CheckBox();
            checkBox_TeammateAimlines = new CheckBox();
            checkBox_AIAimlines = new CheckBox();
            flowLayoutPanel5 = new FlowLayoutPanel();
            label29 = new Label();
            checkBox_Loot = new CheckBox();
            checkBox_LootWishlist = new CheckBox();
            checkBox_ShowContainers = new CheckBox();
            flowLayoutPanel_Loot_Containers = new FlowLayoutPanel();
            label30 = new Label();
            checkBox_Containers_SelectAll = new CheckBox();
            label31 = new Label();
            checkBox_Containers_HideSearched = new CheckBox();
            checkedListBox_Containers = new CheckedListBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label34 = new Label();
            checkBox_QuestHelper_Enabled = new CheckBox();
            checkedListBox_QuestHelper = new CheckedListBox();
            flowLayoutPanel_WebRadarSettings = new FlowLayoutPanel();
            label9 = new Label();
            label25 = new Label();
            button_WebRadarStart = new Button();
            checkBox_WebRadarUPNP = new CheckBox();
            label_WebRadarHost = new Label();
            textBox_WebRadarBindIP = new TextBox();
            label_WebRadarPort = new Label();
            textBox_WebRadarPort = new TextBox();
            label_WebRadarTickRate = new Label();
            textBox_WebRadarTickRate = new TextBox();
            label_WebRadarPassword = new Label();
            textBox_WebRadarPassword = new TextBox();
            linkLabel_WebRadarLink = new LinkLabel();
            flowLayoutPanel_MemWriteCheckbox = new FlowLayoutPanel();
            checkBox_EnableMemWrite = new CheckBox();
            flowLayoutPanel_MemWrites = new FlowLayoutPanel();
            label3 = new Label();
            checkBox_AdvancedMemWrites = new CheckBox();
            checkBox_AntiPage = new CheckBox();
            checkBox_AimBotEnabled = new CheckBox();
            checkBox_NoRecoilSway = new CheckBox();
            checkBox_Chams = new CheckBox();
            checkBox_InfStamina = new CheckBox();
            checkBox_MoveSpeed = new CheckBox();
            checkBox_FastWeaponOps = new CheckBox();
            checkBox_FastLoadUnload = new CheckBox();
            checkBox_LTW = new CheckBox();
            checkBox_WideLean = new CheckBox();
            checkBox_NoVisor = new CheckBox();
            checkBox_AlwaysDaySunny = new CheckBox();
            checkBox_NoWepMalf = new CheckBox();
            checkBox_FullBright = new CheckBox();
            checkBox_RageMode = new CheckBox();
            button_AntiAfk = new Button();
            button_GymHack = new Button();
            label22 = new Label();
            flowLayoutPanel_Aimbot = new FlowLayoutPanel();
            label13 = new Label();
            label23 = new Label();
            checkBox_SA_SafeLock = new CheckBox();
            checkBox_SA_AutoBone = new CheckBox();
            radioButton_AimTarget_FOV = new RadioButton();
            radioButton_AimTarget_CQB = new RadioButton();
            checkBox_AimbotDisableReLock = new CheckBox();
            label_AimFOV = new Label();
            trackBar_AimFOV = new TrackBar();
            label10 = new Label();
            comboBox_AimbotTarget = new ComboBox();
            checkBox_AimHeadAI = new CheckBox();
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
            flowLayoutPanel_AdvancedChams = new FlowLayoutPanel();
            label14 = new Label();
            label15 = new Label();
            textBox_VischeckVisColor = new TextBox();
            button_VischeckVisColorPick = new Button();
            label33 = new Label();
            textBox_VischeckInvisColor = new TextBox();
            button_VischeckInvisColorPick = new Button();
            flowLayoutPanel_LTW = new FlowLayoutPanel();
            label28 = new Label();
            label_LTWAmount = new Label();
            trackBar_LTWAmount = new TrackBar();
            flowLayoutPanel_WideLean = new FlowLayoutPanel();
            label_WideLean = new Label();
            label21 = new Label();
            comboBox_WideLeanMode = new ComboBox();
            label_WideLeanAmt = new Label();
            trackBar_WideLeanAmt = new TrackBar();
            flowLayoutPanel_MonitorSettings = new FlowLayoutPanel();
            label11 = new Label();
            label_Width = new Label();
            textBox_ResWidth = new TextBox();
            label_Height = new Label();
            textBox_ResHeight = new TextBox();
            button_DetectRes = new Button();
            flowLayoutPanel_ESPSettings = new FlowLayoutPanel();
            label12 = new Label();
            label26 = new Label();
            button_StartESP = new Button();
            button_EspColorPicker = new Button();
            label_ESPFPSCap = new Label();
            textBox_EspFpsCap = new TextBox();
            checkBox_ESP_AutoFS = new CheckBox();
            comboBox_ESPAutoFS = new ComboBox();
            label_ESP_HighAlert = new Label();
            comboBox_ESP_HighAlert = new ComboBox();
            checkBox_ESP_Loot = new CheckBox();
            checkBox_ESP_Exfils = new CheckBox();
            checkBox_ESP_Explosives = new CheckBox();
            checkBox_ESP_ShowMag = new CheckBox();
            checkBox_ESP_Dist = new CheckBox();
            checkBox_ESP_ShowMines = new CheckBox();
            checkBox_ESP_FireportAim = new CheckBox();
            checkBox_ESP_AimFov = new CheckBox();
            checkBox_ESP_AimLock = new CheckBox();
            checkBox_ESP_RaidStats = new CheckBox();
            checkBox_ESP_StatusText = new CheckBox();
            checkBox_ESP_FPS = new CheckBox();
            label27 = new Label();
            flowLayoutPanel_ESP_PlayerRender = new FlowLayoutPanel();
            label18 = new Label();
            radioButton_ESPRender_None = new RadioButton();
            radioButton_ESPRender_Bones = new RadioButton();
            radioButton_ESPRender_Box = new RadioButton();
            radioButton_ESPRender_Presence = new RadioButton();
            checkBox_ESPRender_Labels = new CheckBox();
            checkBox_ESPRender_Weapons = new CheckBox();
            checkBox_ESPRender_Dist = new CheckBox();
            flowLayoutPanel_ESP_AIRender = new FlowLayoutPanel();
            label19 = new Label();
            radioButton_ESPAIRender_None = new RadioButton();
            radioButton_ESPAIRender_Bones = new RadioButton();
            radioButton_ESPAIRender_Box = new RadioButton();
            radioButton_ESPAIRender_Presence = new RadioButton();
            checkBox_ESPAIRender_Labels = new CheckBox();
            checkBox_ESPAIRender_Weapons = new CheckBox();
            checkBox_ESPAIRender_Dist = new CheckBox();
            flowLayoutPanel4 = new FlowLayoutPanel();
            label_EspLootDist = new Label();
            trackBar_EspLootDist = new TrackBar();
            label_EspImpLootDist = new Label();
            trackBar_EspImpLootDist = new TrackBar();
            label_ESPContainerDist = new Label();
            trackBar_ESPContainerDist = new TrackBar();
            label_EspQuestHelperDist = new Label();
            trackBar_EspQuestHelperDist = new TrackBar();
            label_EspExplosivesDist = new Label();
            trackBar_EspGrenadeDist = new TrackBar();
            label_EspFontScale = new Label();
            trackBar_EspFontScale = new TrackBar();
            label_EspLineScale = new Label();
            trackBar_EspLineScale = new TrackBar();
            tabPage3 = new TabPage();
            richTextBox_PlayersInfo = new RichTextBox();
            tabPage4 = new TabPage();
            dataGridView_PlayerHistory = new DataGridView();
            PHistory_Name = new DataGridViewTextBoxColumn();
            PHistory_AccountID = new DataGridViewTextBoxColumn();
            PHistory_Acct = new DataGridViewTextBoxColumn();
            PHistory_Type = new DataGridViewTextBoxColumn();
            PHistory_KD = new DataGridViewTextBoxColumn();
            PHistory_Hours = new DataGridViewTextBoxColumn();
            PHistory_Raids = new DataGridViewTextBoxColumn();
            PHistory_SR = new DataGridViewTextBoxColumn();
            PHistory_Group = new DataGridViewTextBoxColumn();
            PHistory_Alerts = new DataGridViewTextBoxColumn();
            tabPage5 = new TabPage();
            dataGridView_Watchlist = new DataGridView();
            Column_PWAcctID = new DataGridViewTextBoxColumn();
            Column_PWReason = new DataGridViewTextBoxColumn();
            Column_PWTimestamp = new DataGridViewTextBoxColumn();
            tabPage6 = new TabPage();
            tableLayoutPanel_ImportantLoot = new TableLayoutPanel();
            flowLayoutPanel2 = new FlowLayoutPanel();
            checkBox_CurrentLootFilter_Enabled = new CheckBox();
            comboBox_LootFilters = new ComboBox();
            button_LootFilters_Add = new Button();
            button_LootFilters_Rename = new Button();
            button_LootFilters_Delete = new Button();
            comboBox_LootFiltersItem_Items = new ComboBox();
            textBox_LootFiltersItem_Search = new TextBox();
            button_LootFiltersItem_Add = new Button();
            dataGridView_Loot = new DataGridView();
            Column_LootEnabled = new DataGridViewCheckBoxColumn();
            Column_LootItemID = new DataGridViewTextBoxColumn();
            Column_LootName = new DataGridViewTextBoxColumn();
            Column_LootType = new DataGridViewComboBoxColumn();
            Column_LootComment = new DataGridViewTextBoxColumn();
            Column_ItemColor = new DataGridViewTextBoxColumn();
            Column_LootColorPicker = new DataGridViewButtonColumn();
            colorPicker1 = new ColorDialog();
            toolTip1 = new ToolTip(components);
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            flowLayoutPanel_Loot.SuspendLayout();
            groupBox_MapSetup.SuspendLayout();
            tabPage2.SuspendLayout();
            flowLayoutPanel_Settings.SuspendLayout();
            flowLayoutPanel_RadarSettings.SuspendLayout();
            ((ISupportInitialize)trackBar_AimlineLength).BeginInit();
            ((ISupportInitialize)trackBar_MaxDist).BeginInit();
            ((ISupportInitialize)trackBar_UIScale).BeginInit();
            ((ISupportInitialize)trackBar_ContainerDist).BeginInit();
            flowLayoutPanel5.SuspendLayout();
            flowLayoutPanel_Loot_Containers.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel_WebRadarSettings.SuspendLayout();
            flowLayoutPanel_MemWriteCheckbox.SuspendLayout();
            flowLayoutPanel_MemWrites.SuspendLayout();
            flowLayoutPanel_Aimbot.SuspendLayout();
            ((ISupportInitialize)trackBar_AimFOV).BeginInit();
            flowLayoutPanel_NoRecoil.SuspendLayout();
            ((ISupportInitialize)trackBar_NoRecoil).BeginInit();
            ((ISupportInitialize)trackBar_NoSway).BeginInit();
            flowLayoutPanel_Chams.SuspendLayout();
            flowLayoutPanel_AdvancedChams.SuspendLayout();
            flowLayoutPanel_LTW.SuspendLayout();
            ((ISupportInitialize)trackBar_LTWAmount).BeginInit();
            flowLayoutPanel_WideLean.SuspendLayout();
            ((ISupportInitialize)trackBar_WideLeanAmt).BeginInit();
            flowLayoutPanel_MonitorSettings.SuspendLayout();
            flowLayoutPanel_ESPSettings.SuspendLayout();
            flowLayoutPanel_ESP_PlayerRender.SuspendLayout();
            flowLayoutPanel_ESP_AIRender.SuspendLayout();
            flowLayoutPanel4.SuspendLayout();
            ((ISupportInitialize)trackBar_EspLootDist).BeginInit();
            ((ISupportInitialize)trackBar_EspImpLootDist).BeginInit();
            ((ISupportInitialize)trackBar_ESPContainerDist).BeginInit();
            ((ISupportInitialize)trackBar_EspQuestHelperDist).BeginInit();
            ((ISupportInitialize)trackBar_EspGrenadeDist).BeginInit();
            ((ISupportInitialize)trackBar_EspFontScale).BeginInit();
            ((ISupportInitialize)trackBar_EspLineScale).BeginInit();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            ((ISupportInitialize)dataGridView_PlayerHistory).BeginInit();
            tabPage5.SuspendLayout();
            ((ISupportInitialize)dataGridView_Watchlist).BeginInit();
            tabPage6.SuspendLayout();
            tableLayoutPanel_ImportantLoot.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            ((ISupportInitialize)dataGridView_Loot).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Controls.Add(tabPage6);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1264, 681);
            tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.Black;
            tabPage1.Controls.Add(flowLayoutPanel_Loot);
            tabPage1.Controls.Add(button_Loot);
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
            // flowLayoutPanel_Loot
            // 
            flowLayoutPanel_Loot.AutoSize = true;
            flowLayoutPanel_Loot.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_Loot.BackColor = Color.WhiteSmoke;
            flowLayoutPanel_Loot.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel_Loot.Controls.Add(label20);
            flowLayoutPanel_Loot.Controls.Add(label6);
            flowLayoutPanel_Loot.Controls.Add(label7);
            flowLayoutPanel_Loot.Controls.Add(textBox_LootRegValue);
            flowLayoutPanel_Loot.Controls.Add(label8);
            flowLayoutPanel_Loot.Controls.Add(textBox_LootImpValue);
            flowLayoutPanel_Loot.Controls.Add(checkBox_LootPPS);
            flowLayoutPanel_Loot.Controls.Add(radioButton_Loot_FleaPrice);
            flowLayoutPanel_Loot.Controls.Add(radioButton_Loot_VendorPrice);
            flowLayoutPanel_Loot.Controls.Add(checkBox_HideCorpses);
            flowLayoutPanel_Loot.Controls.Add(checkBox_ShowMeds);
            flowLayoutPanel_Loot.Controls.Add(checkBox_ShowFood);
            flowLayoutPanel_Loot.Controls.Add(checkBox_ShowBackpacks);
            flowLayoutPanel_Loot.Controls.Add(textBox_LootFilterByName);
            flowLayoutPanel_Loot.Location = new Point(6, 36);
            flowLayoutPanel_Loot.Name = "flowLayoutPanel_Loot";
            flowLayoutPanel_Loot.Size = new Size(407, 146);
            flowLayoutPanel_Loot.TabIndex = 19;
            flowLayoutPanel_Loot.Visible = false;
            // 
            // label20
            // 
            label20.AutoSize = true;
            flowLayoutPanel_Loot.SetFlowBreak(label20, true);
            label20.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label20.Location = new Point(3, 0);
            label20.Name = "label20";
            label20.Size = new Size(44, 21);
            label20.TabIndex = 0;
            label20.Text = "Loot";
            // 
            // label6
            // 
            label6.AutoSize = true;
            flowLayoutPanel_Loot.SetFlowBreak(label6, true);
            label6.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            label6.Location = new Point(3, 21);
            label6.Name = "label6";
            label6.Size = new Size(110, 15);
            label6.TabIndex = 0;
            label6.Text = "Sort by Ruble Value";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Right;
            label7.AutoSize = true;
            label7.Location = new Point(3, 43);
            label7.Name = "label7";
            label7.Size = new Size(47, 15);
            label7.TabIndex = 3;
            label7.Text = "Regular";
            // 
            // textBox_LootRegValue
            // 
            textBox_LootRegValue.Anchor = AnchorStyles.Right;
            textBox_LootRegValue.Location = new Point(56, 39);
            textBox_LootRegValue.MaxLength = 7;
            textBox_LootRegValue.Name = "textBox_LootRegValue";
            textBox_LootRegValue.Size = new Size(50, 23);
            textBox_LootRegValue.TabIndex = 1;
            textBox_LootRegValue.Text = "50000";
            textBox_LootRegValue.TextAlign = HorizontalAlignment.Center;
            textBox_LootRegValue.TextChanged += textBox_LootRegValue_TextChanged;
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Right;
            label8.AutoSize = true;
            label8.Location = new Point(112, 43);
            label8.Name = "label8";
            label8.Size = new Size(51, 15);
            label8.TabIndex = 4;
            label8.Text = "Valuable";
            // 
            // textBox_LootImpValue
            // 
            textBox_LootImpValue.Anchor = AnchorStyles.Right;
            textBox_LootImpValue.Location = new Point(169, 39);
            textBox_LootImpValue.MaxLength = 8;
            textBox_LootImpValue.Name = "textBox_LootImpValue";
            textBox_LootImpValue.Size = new Size(57, 23);
            textBox_LootImpValue.TabIndex = 2;
            textBox_LootImpValue.Text = "200000";
            textBox_LootImpValue.TextAlign = HorizontalAlignment.Center;
            textBox_LootImpValue.TextChanged += textBox_LootImpValue_TextChanged;
            // 
            // checkBox_LootPPS
            // 
            checkBox_LootPPS.Anchor = AnchorStyles.Right;
            checkBox_LootPPS.AutoSize = true;
            flowLayoutPanel_Loot.SetFlowBreak(checkBox_LootPPS, true);
            checkBox_LootPPS.Location = new Point(232, 41);
            checkBox_LootPPS.Name = "checkBox_LootPPS";
            checkBox_LootPPS.Size = new Size(95, 19);
            checkBox_LootPPS.TabIndex = 25;
            checkBox_LootPPS.Text = "Price Per Slot";
            checkBox_LootPPS.UseVisualStyleBackColor = true;
            checkBox_LootPPS.CheckedChanged += checkBox_LootPPS_CheckedChanged;
            // 
            // radioButton_Loot_FleaPrice
            // 
            radioButton_Loot_FleaPrice.AutoSize = true;
            radioButton_Loot_FleaPrice.Checked = true;
            radioButton_Loot_FleaPrice.Location = new Point(3, 68);
            radioButton_Loot_FleaPrice.Name = "radioButton_Loot_FleaPrice";
            radioButton_Loot_FleaPrice.Size = new Size(80, 19);
            radioButton_Loot_FleaPrice.TabIndex = 27;
            radioButton_Loot_FleaPrice.TabStop = true;
            radioButton_Loot_FleaPrice.Text = "Flea Prices";
            radioButton_Loot_FleaPrice.UseVisualStyleBackColor = true;
            radioButton_Loot_FleaPrice.CheckedChanged += radioButton_Loot_FleaPrice_CheckedChanged;
            // 
            // radioButton_Loot_VendorPrice
            // 
            radioButton_Loot_VendorPrice.AutoSize = true;
            flowLayoutPanel_Loot.SetFlowBreak(radioButton_Loot_VendorPrice, true);
            radioButton_Loot_VendorPrice.Location = new Point(89, 68);
            radioButton_Loot_VendorPrice.Name = "radioButton_Loot_VendorPrice";
            radioButton_Loot_VendorPrice.Size = new Size(92, 19);
            radioButton_Loot_VendorPrice.TabIndex = 28;
            radioButton_Loot_VendorPrice.Text = "Trader Prices";
            radioButton_Loot_VendorPrice.UseVisualStyleBackColor = true;
            radioButton_Loot_VendorPrice.CheckedChanged += radioButton_Loot_VendorPrice_CheckedChanged;
            // 
            // checkBox_HideCorpses
            // 
            checkBox_HideCorpses.AutoSize = true;
            checkBox_HideCorpses.Location = new Point(3, 93);
            checkBox_HideCorpses.Name = "checkBox_HideCorpses";
            checkBox_HideCorpses.Size = new Size(96, 19);
            checkBox_HideCorpses.TabIndex = 22;
            checkBox_HideCorpses.Text = "Hide Corpses";
            checkBox_HideCorpses.UseVisualStyleBackColor = true;
            checkBox_HideCorpses.CheckedChanged += checkBox_HideCorpses_CheckedChanged;
            // 
            // checkBox_ShowMeds
            // 
            checkBox_ShowMeds.AutoSize = true;
            checkBox_ShowMeds.Location = new Point(105, 93);
            checkBox_ShowMeds.Name = "checkBox_ShowMeds";
            checkBox_ShowMeds.Size = new Size(87, 19);
            checkBox_ShowMeds.TabIndex = 23;
            checkBox_ShowMeds.Text = "Show Meds";
            checkBox_ShowMeds.UseVisualStyleBackColor = true;
            checkBox_ShowMeds.CheckedChanged += checkBox_ShowMeds_CheckedChanged;
            // 
            // checkBox_ShowFood
            // 
            checkBox_ShowFood.AutoSize = true;
            checkBox_ShowFood.Location = new Point(198, 93);
            checkBox_ShowFood.Name = "checkBox_ShowFood";
            checkBox_ShowFood.Size = new Size(85, 19);
            checkBox_ShowFood.TabIndex = 24;
            checkBox_ShowFood.Text = "Show Food";
            checkBox_ShowFood.UseVisualStyleBackColor = true;
            checkBox_ShowFood.CheckedChanged += checkBox_ShowFood_CheckedChanged;
            // 
            // checkBox_ShowBackpacks
            // 
            checkBox_ShowBackpacks.AutoSize = true;
            flowLayoutPanel_Loot.SetFlowBreak(checkBox_ShowBackpacks, true);
            checkBox_ShowBackpacks.Location = new Point(289, 93);
            checkBox_ShowBackpacks.Name = "checkBox_ShowBackpacks";
            checkBox_ShowBackpacks.Size = new Size(113, 19);
            checkBox_ShowBackpacks.TabIndex = 26;
            checkBox_ShowBackpacks.Text = "Show Backpacks";
            checkBox_ShowBackpacks.UseVisualStyleBackColor = true;
            checkBox_ShowBackpacks.CheckedChanged += checkBox_ShowBackpacks_CheckedChanged;
            // 
            // textBox_LootFilterByName
            // 
            textBox_LootFilterByName.Location = new Point(3, 118);
            textBox_LootFilterByName.MaxLength = 512;
            textBox_LootFilterByName.Name = "textBox_LootFilterByName";
            textBox_LootFilterByName.PlaceholderText = "Loot Search (ex: Item1,Item2,etc.)";
            textBox_LootFilterByName.Size = new Size(317, 23);
            textBox_LootFilterByName.TabIndex = 5;
            textBox_LootFilterByName.TextAlign = HorizontalAlignment.Center;
            textBox_LootFilterByName.TextChanged += textBox_LootFilterByName_TextChanged;
            // 
            // button_Loot
            // 
            button_Loot.Location = new Point(85, 6);
            button_Loot.Name = "button_Loot";
            button_Loot.Size = new Size(44, 25);
            button_Loot.TabIndex = 12;
            button_Loot.Text = "Loot";
            button_Loot.UseVisualStyleBackColor = true;
            button_Loot.Visible = false;
            button_Loot.Click += button_LootFilter_Click;
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
            skglControl_Radar.VSync = true;
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
            flowLayoutPanel_Settings.Controls.Add(flowLayoutPanel5);
            flowLayoutPanel_Settings.Controls.Add(flowLayoutPanel1);
            flowLayoutPanel_Settings.Controls.Add(flowLayoutPanel_WebRadarSettings);
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
            flowLayoutPanel_RadarSettings.Controls.Add(label24);
            flowLayoutPanel_RadarSettings.Controls.Add(linkLabel_CheckForUpdates);
            flowLayoutPanel_RadarSettings.Controls.Add(label1);
            flowLayoutPanel_RadarSettings.Controls.Add(button_Restart);
            flowLayoutPanel_RadarSettings.Controls.Add(button_HotkeyManager);
            flowLayoutPanel_RadarSettings.Controls.Add(button_Radar_ColorPicker);
            flowLayoutPanel_RadarSettings.Controls.Add(button_BackupConfig);
            flowLayoutPanel_RadarSettings.Controls.Add(label_AimlineLength);
            flowLayoutPanel_RadarSettings.Controls.Add(trackBar_AimlineLength);
            flowLayoutPanel_RadarSettings.Controls.Add(label_MaxDist);
            flowLayoutPanel_RadarSettings.Controls.Add(trackBar_MaxDist);
            flowLayoutPanel_RadarSettings.Controls.Add(label_UIScale);
            flowLayoutPanel_RadarSettings.Controls.Add(trackBar_UIScale);
            flowLayoutPanel_RadarSettings.Controls.Add(label_ContainerDist);
            flowLayoutPanel_RadarSettings.Controls.Add(trackBar_ContainerDist);
            flowLayoutPanel_RadarSettings.Controls.Add(checkBox_MapSetup);
            flowLayoutPanel_RadarSettings.Controls.Add(checkBox_Aimview);
            flowLayoutPanel_RadarSettings.Controls.Add(checkBox_ShowInfoTab);
            flowLayoutPanel_RadarSettings.Controls.Add(checkBox_GrpConnect);
            flowLayoutPanel_RadarSettings.Controls.Add(checkBox_HideNames);
            flowLayoutPanel_RadarSettings.Controls.Add(checkBox_ShowMines);
            flowLayoutPanel_RadarSettings.Controls.Add(checkBox_TeammateAimlines);
            flowLayoutPanel_RadarSettings.Controls.Add(checkBox_AIAimlines);
            flowLayoutPanel_RadarSettings.Dock = DockStyle.Top;
            flowLayoutPanel_Settings.SetFlowBreak(flowLayoutPanel_RadarSettings, true);
            flowLayoutPanel_RadarSettings.Location = new Point(3, 3);
            flowLayoutPanel_RadarSettings.Name = "flowLayoutPanel_RadarSettings";
            flowLayoutPanel_RadarSettings.Size = new Size(1230, 177);
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
            // label24
            // 
            label24.Location = new Point(3, 21);
            label24.Name = "label24";
            label24.Size = new Size(0, 0);
            label24.TabIndex = 46;
            label24.Text = "label24";
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
            linkLabel_CheckForUpdates.TabIndex = 61;
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
            label_AimlineLength.Location = new Point(633, 66);
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
            trackBar_AimlineLength.Location = new Point(729, 51);
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
            // label_MaxDist
            // 
            label_MaxDist.Anchor = AnchorStyles.Right;
            label_MaxDist.AutoSize = true;
            label_MaxDist.Location = new Point(828, 66);
            label_MaxDist.Name = "label_MaxDist";
            label_MaxDist.Size = new Size(73, 15);
            label_MaxDist.TabIndex = 35;
            label_MaxDist.Text = "Max Dist 350";
            // 
            // trackBar_MaxDist
            // 
            trackBar_MaxDist.Anchor = AnchorStyles.Right;
            trackBar_MaxDist.BackColor = SystemColors.Window;
            trackBar_MaxDist.LargeChange = 20;
            trackBar_MaxDist.Location = new Point(907, 51);
            trackBar_MaxDist.Maximum = 1500;
            trackBar_MaxDist.Minimum = 100;
            trackBar_MaxDist.Name = "trackBar_MaxDist";
            trackBar_MaxDist.Size = new Size(92, 45);
            trackBar_MaxDist.TabIndex = 36;
            trackBar_MaxDist.TickStyle = TickStyle.None;
            trackBar_MaxDist.Value = 350;
            // 
            // label_UIScale
            // 
            label_UIScale.Anchor = AnchorStyles.Right;
            label_UIScale.AutoSize = true;
            label_UIScale.Location = new Point(1005, 66);
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
            trackBar_UIScale.LargeChange = 10;
            trackBar_UIScale.Location = new Point(1083, 51);
            trackBar_UIScale.Maximum = 200;
            trackBar_UIScale.Minimum = 50;
            trackBar_UIScale.Name = "trackBar_UIScale";
            trackBar_UIScale.Size = new Size(92, 45);
            trackBar_UIScale.TabIndex = 27;
            trackBar_UIScale.TickStyle = TickStyle.None;
            trackBar_UIScale.Value = 100;
            // 
            // label_ContainerDist
            // 
            label_ContainerDist.Anchor = AnchorStyles.Right;
            label_ContainerDist.AutoSize = true;
            label_ContainerDist.Location = new Point(3, 117);
            label_ContainerDist.Name = "label_ContainerDist";
            label_ContainerDist.Size = new Size(103, 15);
            label_ContainerDist.TabIndex = 59;
            label_ContainerDist.Text = "Container Dist 150";
            // 
            // trackBar_ContainerDist
            // 
            trackBar_ContainerDist.Anchor = AnchorStyles.Right;
            trackBar_ContainerDist.BackColor = SystemColors.Window;
            flowLayoutPanel_RadarSettings.SetFlowBreak(trackBar_ContainerDist, true);
            trackBar_ContainerDist.Location = new Point(112, 102);
            trackBar_ContainerDist.Maximum = 1000;
            trackBar_ContainerDist.Minimum = 5;
            trackBar_ContainerDist.Name = "trackBar_ContainerDist";
            trackBar_ContainerDist.Size = new Size(78, 45);
            trackBar_ContainerDist.TabIndex = 58;
            trackBar_ContainerDist.TickStyle = TickStyle.None;
            trackBar_ContainerDist.Value = 150;
            // 
            // checkBox_MapSetup
            // 
            checkBox_MapSetup.AutoSize = true;
            checkBox_MapSetup.Location = new Point(3, 153);
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
            checkBox_Aimview.Location = new Point(162, 153);
            checkBox_Aimview.Name = "checkBox_Aimview";
            checkBox_Aimview.Size = new Size(86, 19);
            checkBox_Aimview.TabIndex = 19;
            checkBox_Aimview.Text = "ESP Widget";
            checkBox_Aimview.UseVisualStyleBackColor = true;
            checkBox_Aimview.CheckedChanged += checkBox_Aimview_CheckedChanged;
            // 
            // checkBox_ShowInfoTab
            // 
            checkBox_ShowInfoTab.AutoSize = true;
            checkBox_ShowInfoTab.Location = new Point(254, 153);
            checkBox_ShowInfoTab.Name = "checkBox_ShowInfoTab";
            checkBox_ShowInfoTab.Size = new Size(123, 19);
            checkBox_ShowInfoTab.TabIndex = 31;
            checkBox_ShowInfoTab.Text = "Player Info Widget";
            checkBox_ShowInfoTab.UseVisualStyleBackColor = true;
            // 
            // checkBox_GrpConnect
            // 
            checkBox_GrpConnect.AutoSize = true;
            checkBox_GrpConnect.Location = new Point(383, 153);
            checkBox_GrpConnect.Name = "checkBox_GrpConnect";
            checkBox_GrpConnect.Size = new Size(112, 19);
            checkBox_GrpConnect.TabIndex = 33;
            checkBox_GrpConnect.Text = "Connect Groups";
            checkBox_GrpConnect.UseVisualStyleBackColor = true;
            // 
            // checkBox_HideNames
            // 
            checkBox_HideNames.AutoSize = true;
            checkBox_HideNames.Location = new Point(501, 153);
            checkBox_HideNames.Name = "checkBox_HideNames";
            checkBox_HideNames.Size = new Size(91, 19);
            checkBox_HideNames.TabIndex = 26;
            checkBox_HideNames.Text = "Hide Names";
            checkBox_HideNames.UseVisualStyleBackColor = true;
            checkBox_HideNames.CheckedChanged += checkBox_HideNames_CheckedChanged;
            // 
            // checkBox_ShowMines
            // 
            checkBox_ShowMines.AutoSize = true;
            checkBox_ShowMines.Location = new Point(598, 153);
            checkBox_ShowMines.Name = "checkBox_ShowMines";
            checkBox_ShowMines.Size = new Size(90, 19);
            checkBox_ShowMines.TabIndex = 38;
            checkBox_ShowMines.Text = "Show Mines";
            checkBox_ShowMines.UseVisualStyleBackColor = true;
            // 
            // checkBox_TeammateAimlines
            // 
            checkBox_TeammateAimlines.AutoSize = true;
            checkBox_TeammateAimlines.Location = new Point(694, 153);
            checkBox_TeammateAimlines.Name = "checkBox_TeammateAimlines";
            checkBox_TeammateAimlines.Size = new Size(131, 19);
            checkBox_TeammateAimlines.TabIndex = 39;
            checkBox_TeammateAimlines.Text = "Teammate Aimlines";
            checkBox_TeammateAimlines.UseVisualStyleBackColor = true;
            checkBox_TeammateAimlines.CheckedChanged += checkBox_TeammateAimlines_CheckedChanged;
            // 
            // checkBox_AIAimlines
            // 
            checkBox_AIAimlines.AutoSize = true;
            checkBox_AIAimlines.Checked = true;
            checkBox_AIAimlines.CheckState = CheckState.Checked;
            flowLayoutPanel_RadarSettings.SetFlowBreak(checkBox_AIAimlines, true);
            checkBox_AIAimlines.Location = new Point(831, 153);
            checkBox_AIAimlines.Name = "checkBox_AIAimlines";
            checkBox_AIAimlines.Size = new Size(86, 19);
            checkBox_AIAimlines.TabIndex = 60;
            checkBox_AIAimlines.Text = "AI Aimlines";
            checkBox_AIAimlines.UseVisualStyleBackColor = true;
            checkBox_AIAimlines.CheckedChanged += checkBox_AIAimlines_CheckedChanged;
            // 
            // flowLayoutPanel5
            // 
            flowLayoutPanel5.AutoSize = true;
            flowLayoutPanel5.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel5.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel5.Controls.Add(label29);
            flowLayoutPanel5.Controls.Add(checkBox_Loot);
            flowLayoutPanel5.Controls.Add(checkBox_LootWishlist);
            flowLayoutPanel5.Controls.Add(checkBox_ShowContainers);
            flowLayoutPanel5.Controls.Add(flowLayoutPanel_Loot_Containers);
            flowLayoutPanel5.Dock = DockStyle.Top;
            flowLayoutPanel_Settings.SetFlowBreak(flowLayoutPanel5, true);
            flowLayoutPanel5.Location = new Point(3, 186);
            flowLayoutPanel5.Name = "flowLayoutPanel5";
            flowLayoutPanel5.Size = new Size(1230, 185);
            flowLayoutPanel5.TabIndex = 61;
            // 
            // label29
            // 
            label29.AutoSize = true;
            flowLayoutPanel5.SetFlowBreak(label29, true);
            label29.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label29.Location = new Point(3, 0);
            label29.Name = "label29";
            label29.Size = new Size(44, 21);
            label29.TabIndex = 46;
            label29.Text = "Loot";
            // 
            // checkBox_Loot
            // 
            checkBox_Loot.Anchor = AnchorStyles.Right;
            checkBox_Loot.AutoSize = true;
            checkBox_Loot.Location = new Point(3, 94);
            checkBox_Loot.Name = "checkBox_Loot";
            checkBox_Loot.Size = new Size(105, 19);
            checkBox_Loot.TabIndex = 17;
            checkBox_Loot.Text = "Show Loot (F3)";
            checkBox_Loot.UseVisualStyleBackColor = true;
            checkBox_Loot.CheckedChanged += checkBox_Loot_CheckedChanged;
            // 
            // checkBox_LootWishlist
            // 
            checkBox_LootWishlist.Anchor = AnchorStyles.Right;
            checkBox_LootWishlist.AutoSize = true;
            checkBox_LootWishlist.Location = new Point(114, 94);
            checkBox_LootWishlist.Name = "checkBox_LootWishlist";
            checkBox_LootWishlist.Size = new Size(126, 19);
            checkBox_LootWishlist.TabIndex = 61;
            checkBox_LootWishlist.Text = "Show Loot Wishlist";
            checkBox_LootWishlist.UseVisualStyleBackColor = true;
            checkBox_LootWishlist.CheckedChanged += checkBox_LootWishlist_CheckedChanged;
            // 
            // checkBox_ShowContainers
            // 
            checkBox_ShowContainers.Anchor = AnchorStyles.Right;
            checkBox_ShowContainers.AutoSize = true;
            checkBox_ShowContainers.Location = new Point(246, 94);
            checkBox_ShowContainers.Name = "checkBox_ShowContainers";
            checkBox_ShowContainers.Size = new Size(147, 19);
            checkBox_ShowContainers.TabIndex = 47;
            checkBox_ShowContainers.Text = "Show Static Containers";
            checkBox_ShowContainers.UseVisualStyleBackColor = true;
            checkBox_ShowContainers.CheckedChanged += checkBox_ShowContainers_CheckedChanged;
            // 
            // flowLayoutPanel_Loot_Containers
            // 
            flowLayoutPanel_Loot_Containers.AutoSize = true;
            flowLayoutPanel_Loot_Containers.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_Loot_Containers.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel_Loot_Containers.Controls.Add(label30);
            flowLayoutPanel_Loot_Containers.Controls.Add(checkBox_Containers_SelectAll);
            flowLayoutPanel_Loot_Containers.Controls.Add(label31);
            flowLayoutPanel_Loot_Containers.Controls.Add(checkBox_Containers_HideSearched);
            flowLayoutPanel_Loot_Containers.Controls.Add(checkedListBox_Containers);
            flowLayoutPanel_Loot_Containers.Enabled = false;
            flowLayoutPanel_Loot_Containers.Location = new Point(399, 28);
            flowLayoutPanel_Loot_Containers.Name = "flowLayoutPanel_Loot_Containers";
            flowLayoutPanel_Loot_Containers.Size = new Size(304, 152);
            flowLayoutPanel_Loot_Containers.TabIndex = 60;
            // 
            // label30
            // 
            label30.AutoSize = true;
            flowLayoutPanel_Loot_Containers.SetFlowBreak(label30, true);
            label30.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label30.Location = new Point(3, 0);
            label30.Name = "label30";
            label30.Size = new Size(96, 15);
            label30.TabIndex = 52;
            label30.Text = "Static Containers";
            // 
            // checkBox_Containers_SelectAll
            // 
            checkBox_Containers_SelectAll.AutoSize = true;
            checkBox_Containers_SelectAll.Checked = true;
            checkBox_Containers_SelectAll.CheckState = CheckState.Checked;
            checkBox_Containers_SelectAll.Location = new Point(3, 28);
            checkBox_Containers_SelectAll.Name = "checkBox_Containers_SelectAll";
            checkBox_Containers_SelectAll.Size = new Size(74, 19);
            checkBox_Containers_SelectAll.TabIndex = 61;
            checkBox_Containers_SelectAll.Text = "Select All";
            checkBox_Containers_SelectAll.UseVisualStyleBackColor = true;
            // 
            // label31
            // 
            label31.AutoSize = true;
            label31.Location = new Point(83, 25);
            label31.Name = "label31";
            label31.Size = new Size(0, 15);
            label31.TabIndex = 62;
            // 
            // checkBox_Containers_HideSearched
            // 
            checkBox_Containers_HideSearched.AutoSize = true;
            flowLayoutPanel_Loot_Containers.SetFlowBreak(checkBox_Containers_HideSearched, true);
            checkBox_Containers_HideSearched.Location = new Point(89, 28);
            checkBox_Containers_HideSearched.Name = "checkBox_Containers_HideSearched";
            checkBox_Containers_HideSearched.Size = new Size(102, 19);
            checkBox_Containers_HideSearched.TabIndex = 63;
            checkBox_Containers_HideSearched.Text = "Hide Searched";
            checkBox_Containers_HideSearched.UseVisualStyleBackColor = true;
            checkBox_Containers_HideSearched.CheckedChanged += checkBox_Containers_HideSearched_CheckedChanged;
            // 
            // checkedListBox_Containers
            // 
            checkedListBox_Containers.FormattingEnabled = true;
            checkedListBox_Containers.Location = new Point(3, 53);
            checkedListBox_Containers.Name = "checkedListBox_Containers";
            checkedListBox_Containers.Size = new Size(296, 94);
            checkedListBox_Containers.TabIndex = 60;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel1.Controls.Add(label34);
            flowLayoutPanel1.Controls.Add(checkBox_QuestHelper_Enabled);
            flowLayoutPanel1.Controls.Add(checkedListBox_QuestHelper);
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel_Settings.SetFlowBreak(flowLayoutPanel1, true);
            flowLayoutPanel1.Location = new Point(3, 377);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1230, 181);
            flowLayoutPanel1.TabIndex = 62;
            // 
            // label34
            // 
            label34.AutoSize = true;
            flowLayoutPanel1.SetFlowBreak(label34, true);
            label34.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label34.Location = new Point(3, 0);
            label34.Name = "label34";
            label34.Size = new Size(107, 21);
            label34.TabIndex = 46;
            label34.Text = "Quest Helper";
            // 
            // checkBox_QuestHelper_Enabled
            // 
            checkBox_QuestHelper_Enabled.Anchor = AnchorStyles.Right;
            checkBox_QuestHelper_Enabled.AutoSize = true;
            checkBox_QuestHelper_Enabled.Checked = true;
            checkBox_QuestHelper_Enabled.CheckState = CheckState.Checked;
            checkBox_QuestHelper_Enabled.Location = new Point(3, 92);
            checkBox_QuestHelper_Enabled.Name = "checkBox_QuestHelper_Enabled";
            checkBox_QuestHelper_Enabled.Size = new Size(68, 19);
            checkBox_QuestHelper_Enabled.TabIndex = 37;
            checkBox_QuestHelper_Enabled.Text = "Enabled";
            checkBox_QuestHelper_Enabled.UseVisualStyleBackColor = true;
            checkBox_QuestHelper_Enabled.CheckedChanged += checkBox_QuestHelper_CheckedChanged;
            // 
            // checkedListBox_QuestHelper
            // 
            checkedListBox_QuestHelper.Anchor = AnchorStyles.Right;
            checkedListBox_QuestHelper.FormattingEnabled = true;
            checkedListBox_QuestHelper.Location = new Point(77, 28);
            checkedListBox_QuestHelper.Name = "checkedListBox_QuestHelper";
            checkedListBox_QuestHelper.Size = new Size(343, 148);
            checkedListBox_QuestHelper.TabIndex = 48;
            // 
            // flowLayoutPanel_WebRadarSettings
            // 
            flowLayoutPanel_WebRadarSettings.AutoSize = true;
            flowLayoutPanel_WebRadarSettings.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_WebRadarSettings.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel_WebRadarSettings.Controls.Add(label9);
            flowLayoutPanel_WebRadarSettings.Controls.Add(label25);
            flowLayoutPanel_WebRadarSettings.Controls.Add(button_WebRadarStart);
            flowLayoutPanel_WebRadarSettings.Controls.Add(checkBox_WebRadarUPNP);
            flowLayoutPanel_WebRadarSettings.Controls.Add(label_WebRadarHost);
            flowLayoutPanel_WebRadarSettings.Controls.Add(textBox_WebRadarBindIP);
            flowLayoutPanel_WebRadarSettings.Controls.Add(label_WebRadarPort);
            flowLayoutPanel_WebRadarSettings.Controls.Add(textBox_WebRadarPort);
            flowLayoutPanel_WebRadarSettings.Controls.Add(label_WebRadarTickRate);
            flowLayoutPanel_WebRadarSettings.Controls.Add(textBox_WebRadarTickRate);
            flowLayoutPanel_WebRadarSettings.Controls.Add(label_WebRadarPassword);
            flowLayoutPanel_WebRadarSettings.Controls.Add(textBox_WebRadarPassword);
            flowLayoutPanel_WebRadarSettings.Controls.Add(linkLabel_WebRadarLink);
            flowLayoutPanel_WebRadarSettings.Dock = DockStyle.Top;
            flowLayoutPanel_WebRadarSettings.Enabled = false;
            flowLayoutPanel_Settings.SetFlowBreak(flowLayoutPanel_WebRadarSettings, true);
            flowLayoutPanel_WebRadarSettings.Location = new Point(3, 564);
            flowLayoutPanel_WebRadarSettings.Name = "flowLayoutPanel_WebRadarSettings";
            flowLayoutPanel_WebRadarSettings.Size = new Size(1230, 60);
            flowLayoutPanel_WebRadarSettings.TabIndex = 5;
            // 
            // label9
            // 
            label9.AutoSize = true;
            flowLayoutPanel_WebRadarSettings.SetFlowBreak(label9, true);
            label9.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(3, 0);
            label9.Name = "label9";
            label9.Size = new Size(142, 21);
            label9.TabIndex = 46;
            label9.Text = "Web Radar Server";
            // 
            // label25
            // 
            label25.Location = new Point(3, 21);
            label25.Name = "label25";
            label25.Size = new Size(0, 0);
            label25.TabIndex = 60;
            label25.Text = "label25";
            // 
            // button_WebRadarStart
            // 
            button_WebRadarStart.Location = new Point(9, 24);
            button_WebRadarStart.Name = "button_WebRadarStart";
            button_WebRadarStart.Size = new Size(96, 31);
            button_WebRadarStart.TabIndex = 56;
            button_WebRadarStart.Text = "Start";
            button_WebRadarStart.UseVisualStyleBackColor = true;
            button_WebRadarStart.Click += button_WebRadarStart_Click;
            // 
            // checkBox_WebRadarUPNP
            // 
            checkBox_WebRadarUPNP.Anchor = AnchorStyles.Right;
            checkBox_WebRadarUPNP.AutoSize = true;
            checkBox_WebRadarUPNP.Checked = true;
            checkBox_WebRadarUPNP.CheckState = CheckState.Checked;
            checkBox_WebRadarUPNP.Location = new Point(111, 30);
            checkBox_WebRadarUPNP.Name = "checkBox_WebRadarUPNP";
            checkBox_WebRadarUPNP.Size = new Size(55, 19);
            checkBox_WebRadarUPNP.TabIndex = 57;
            checkBox_WebRadarUPNP.Text = "UPnP";
            checkBox_WebRadarUPNP.UseVisualStyleBackColor = true;
            checkBox_WebRadarUPNP.CheckedChanged += checkBox_WebRadarUPNP_CheckedChanged;
            // 
            // label_WebRadarHost
            // 
            label_WebRadarHost.Anchor = AnchorStyles.Right;
            label_WebRadarHost.AutoSize = true;
            label_WebRadarHost.Location = new Point(172, 32);
            label_WebRadarHost.Name = "label_WebRadarHost";
            label_WebRadarHost.Size = new Size(44, 15);
            label_WebRadarHost.TabIndex = 49;
            label_WebRadarHost.Text = "Bind IP";
            // 
            // textBox_WebRadarBindIP
            // 
            textBox_WebRadarBindIP.Anchor = AnchorStyles.Right;
            textBox_WebRadarBindIP.Location = new Point(222, 28);
            textBox_WebRadarBindIP.MaxLength = 128;
            textBox_WebRadarBindIP.Name = "textBox_WebRadarBindIP";
            textBox_WebRadarBindIP.Size = new Size(154, 23);
            textBox_WebRadarBindIP.TabIndex = 48;
            textBox_WebRadarBindIP.TextChanged += textBox_WebRadarHost_TextChanged;
            // 
            // label_WebRadarPort
            // 
            label_WebRadarPort.Anchor = AnchorStyles.Right;
            label_WebRadarPort.AutoSize = true;
            label_WebRadarPort.Location = new Point(382, 32);
            label_WebRadarPort.Name = "label_WebRadarPort";
            label_WebRadarPort.Size = new Size(29, 15);
            label_WebRadarPort.TabIndex = 51;
            label_WebRadarPort.Text = "Port";
            // 
            // textBox_WebRadarPort
            // 
            textBox_WebRadarPort.Anchor = AnchorStyles.Right;
            textBox_WebRadarPort.Location = new Point(417, 28);
            textBox_WebRadarPort.MaxLength = 5;
            textBox_WebRadarPort.Name = "textBox_WebRadarPort";
            textBox_WebRadarPort.Size = new Size(57, 23);
            textBox_WebRadarPort.TabIndex = 50;
            textBox_WebRadarPort.TextChanged += textBox_WebRadarPort_TextChanged;
            // 
            // label_WebRadarTickRate
            // 
            label_WebRadarTickRate.Anchor = AnchorStyles.Right;
            label_WebRadarTickRate.AutoSize = true;
            label_WebRadarTickRate.Location = new Point(480, 32);
            label_WebRadarTickRate.Name = "label_WebRadarTickRate";
            label_WebRadarTickRate.Size = new Size(80, 15);
            label_WebRadarTickRate.TabIndex = 59;
            label_WebRadarTickRate.Text = "Tick Rate (Hz)";
            // 
            // textBox_WebRadarTickRate
            // 
            textBox_WebRadarTickRate.Anchor = AnchorStyles.Right;
            textBox_WebRadarTickRate.Location = new Point(566, 28);
            textBox_WebRadarTickRate.MaxLength = 3;
            textBox_WebRadarTickRate.Name = "textBox_WebRadarTickRate";
            textBox_WebRadarTickRate.Size = new Size(43, 23);
            textBox_WebRadarTickRate.TabIndex = 58;
            textBox_WebRadarTickRate.Text = "60";
            textBox_WebRadarTickRate.TextChanged += textBox_WebRadarTickRate_TextChanged;
            // 
            // label_WebRadarPassword
            // 
            label_WebRadarPassword.Anchor = AnchorStyles.Right;
            label_WebRadarPassword.AutoSize = true;
            label_WebRadarPassword.Location = new Point(615, 32);
            label_WebRadarPassword.Name = "label_WebRadarPassword";
            label_WebRadarPassword.Size = new Size(57, 15);
            label_WebRadarPassword.TabIndex = 53;
            label_WebRadarPassword.Text = "Password";
            // 
            // textBox_WebRadarPassword
            // 
            textBox_WebRadarPassword.Anchor = AnchorStyles.Right;
            textBox_WebRadarPassword.Location = new Point(678, 28);
            textBox_WebRadarPassword.MaxLength = 10;
            textBox_WebRadarPassword.Name = "textBox_WebRadarPassword";
            textBox_WebRadarPassword.ReadOnly = true;
            textBox_WebRadarPassword.Size = new Size(104, 23);
            textBox_WebRadarPassword.TabIndex = 52;
            // 
            // linkLabel_WebRadarLink
            // 
            linkLabel_WebRadarLink.Anchor = AnchorStyles.Right;
            linkLabel_WebRadarLink.AutoSize = true;
            linkLabel_WebRadarLink.Location = new Point(788, 32);
            linkLabel_WebRadarLink.Name = "linkLabel_WebRadarLink";
            linkLabel_WebRadarLink.Size = new Size(42, 15);
            linkLabel_WebRadarLink.TabIndex = 55;
            linkLabel_WebRadarLink.TabStop = true;
            linkLabel_WebRadarLink.Text = "http://";
            linkLabel_WebRadarLink.LinkClicked += linkLabel_WebRadarLink_LinkClicked;
            // 
            // flowLayoutPanel_MemWriteCheckbox
            // 
            flowLayoutPanel_MemWriteCheckbox.AutoSize = true;
            flowLayoutPanel_MemWriteCheckbox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_MemWriteCheckbox.Controls.Add(checkBox_EnableMemWrite);
            flowLayoutPanel_Settings.SetFlowBreak(flowLayoutPanel_MemWriteCheckbox, true);
            flowLayoutPanel_MemWriteCheckbox.Location = new Point(3, 630);
            flowLayoutPanel_MemWriteCheckbox.Name = "flowLayoutPanel_MemWriteCheckbox";
            flowLayoutPanel_MemWriteCheckbox.Size = new Size(190, 25);
            flowLayoutPanel_MemWriteCheckbox.TabIndex = 62;
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
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_AntiPage);
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_AimBotEnabled);
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_NoRecoilSway);
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_Chams);
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_InfStamina);
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_MoveSpeed);
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_FastWeaponOps);
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_FastLoadUnload);
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_LTW);
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_WideLean);
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_NoVisor);
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_AlwaysDaySunny);
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_NoWepMalf);
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_FullBright);
            flowLayoutPanel_MemWrites.Controls.Add(checkBox_RageMode);
            flowLayoutPanel_MemWrites.Controls.Add(button_AntiAfk);
            flowLayoutPanel_MemWrites.Controls.Add(button_GymHack);
            flowLayoutPanel_MemWrites.Controls.Add(label22);
            flowLayoutPanel_MemWrites.Controls.Add(flowLayoutPanel_Aimbot);
            flowLayoutPanel_MemWrites.Controls.Add(flowLayoutPanel_NoRecoil);
            flowLayoutPanel_MemWrites.Controls.Add(flowLayoutPanel_Chams);
            flowLayoutPanel_MemWrites.Controls.Add(flowLayoutPanel_LTW);
            flowLayoutPanel_MemWrites.Controls.Add(flowLayoutPanel_WideLean);
            flowLayoutPanel_MemWrites.Dock = DockStyle.Top;
            flowLayoutPanel_Settings.SetFlowBreak(flowLayoutPanel_MemWrites, true);
            flowLayoutPanel_MemWrites.Location = new Point(3, 661);
            flowLayoutPanel_MemWrites.Name = "flowLayoutPanel_MemWrites";
            flowLayoutPanel_MemWrites.Size = new Size(1230, 328);
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
            checkBox_AdvancedMemWrites.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkBox_AdvancedMemWrites.Location = new Point(3, 28);
            checkBox_AdvancedMemWrites.Name = "checkBox_AdvancedMemWrites";
            checkBox_AdvancedMemWrites.Size = new Size(246, 19);
            checkBox_AdvancedMemWrites.TabIndex = 74;
            checkBox_AdvancedMemWrites.Text = "Enable Advanced MemWrites (Very Risky)";
            checkBox_AdvancedMemWrites.UseVisualStyleBackColor = true;
            // 
            // checkBox_AntiPage
            // 
            checkBox_AntiPage.AutoSize = true;
            checkBox_AntiPage.Enabled = false;
            flowLayoutPanel_MemWrites.SetFlowBreak(checkBox_AntiPage, true);
            checkBox_AntiPage.Location = new Point(255, 28);
            checkBox_AntiPage.Name = "checkBox_AntiPage";
            checkBox_AntiPage.Size = new Size(79, 19);
            checkBox_AntiPage.TabIndex = 77;
            checkBox_AntiPage.Text = "Anti-Page";
            checkBox_AntiPage.UseVisualStyleBackColor = true;
            checkBox_AntiPage.CheckedChanged += checkBox_AntiPage_CheckedChanged;
            // 
            // checkBox_AimBotEnabled
            // 
            checkBox_AimBotEnabled.Anchor = AnchorStyles.Right;
            checkBox_AimBotEnabled.AutoSize = true;
            checkBox_AimBotEnabled.Location = new Point(3, 53);
            checkBox_AimBotEnabled.Name = "checkBox_AimBotEnabled";
            checkBox_AimBotEnabled.Size = new Size(104, 19);
            checkBox_AimBotEnabled.TabIndex = 41;
            checkBox_AimBotEnabled.Text = "Aimbot (Risky)";
            checkBox_AimBotEnabled.UseVisualStyleBackColor = true;
            checkBox_AimBotEnabled.CheckedChanged += checkBox_AimBot_CheckedChanged;
            // 
            // checkBox_NoRecoilSway
            // 
            checkBox_NoRecoilSway.Anchor = AnchorStyles.Right;
            checkBox_NoRecoilSway.AutoSize = true;
            checkBox_NoRecoilSway.Location = new Point(113, 53);
            checkBox_NoRecoilSway.Name = "checkBox_NoRecoilSway";
            checkBox_NoRecoilSway.Size = new Size(147, 19);
            checkBox_NoRecoilSway.TabIndex = 34;
            checkBox_NoRecoilSway.Text = "No Recoil/Sway (Risky)";
            checkBox_NoRecoilSway.UseVisualStyleBackColor = true;
            checkBox_NoRecoilSway.CheckedChanged += checkBox_NoRecoil_CheckedChanged;
            // 
            // checkBox_Chams
            // 
            checkBox_Chams.Anchor = AnchorStyles.Right;
            checkBox_Chams.AutoSize = true;
            checkBox_Chams.Location = new Point(266, 53);
            checkBox_Chams.Name = "checkBox_Chams";
            checkBox_Chams.Size = new Size(63, 19);
            checkBox_Chams.TabIndex = 42;
            checkBox_Chams.Text = "Chams";
            checkBox_Chams.UseVisualStyleBackColor = true;
            checkBox_Chams.CheckedChanged += checkBox_Chams_CheckedChanged;
            // 
            // checkBox_InfStamina
            // 
            checkBox_InfStamina.Anchor = AnchorStyles.Right;
            checkBox_InfStamina.AutoSize = true;
            checkBox_InfStamina.Location = new Point(335, 53);
            checkBox_InfStamina.Name = "checkBox_InfStamina";
            checkBox_InfStamina.Size = new Size(147, 19);
            checkBox_InfStamina.TabIndex = 35;
            checkBox_InfStamina.Text = "Infinite Stamina (Risky)";
            checkBox_InfStamina.UseVisualStyleBackColor = true;
            checkBox_InfStamina.CheckedChanged += checkBox_InfStamina_CheckedChanged;
            // 
            // checkBox_MoveSpeed
            // 
            checkBox_MoveSpeed.Anchor = AnchorStyles.Right;
            checkBox_MoveSpeed.AutoSize = true;
            checkBox_MoveSpeed.Location = new Point(488, 53);
            checkBox_MoveSpeed.Name = "checkBox_MoveSpeed";
            checkBox_MoveSpeed.Size = new Size(152, 19);
            checkBox_MoveSpeed.TabIndex = 68;
            checkBox_MoveSpeed.Text = "1.2x Move Speed (Risky)";
            checkBox_MoveSpeed.UseVisualStyleBackColor = true;
            checkBox_MoveSpeed.CheckedChanged += checkBox_MoveSpeed_CheckedChanged;
            // 
            // checkBox_FastWeaponOps
            // 
            checkBox_FastWeaponOps.Anchor = AnchorStyles.Right;
            checkBox_FastWeaponOps.AutoSize = true;
            checkBox_FastWeaponOps.Location = new Point(646, 53);
            checkBox_FastWeaponOps.Name = "checkBox_FastWeaponOps";
            checkBox_FastWeaponOps.Size = new Size(118, 19);
            checkBox_FastWeaponOps.TabIndex = 75;
            checkBox_FastWeaponOps.Text = "Fast Weapon Ops";
            checkBox_FastWeaponOps.UseVisualStyleBackColor = true;
            checkBox_FastWeaponOps.CheckedChanged += checkBox_FastWeaponOps_CheckedChanged;
            // 
            // checkBox_FastLoadUnload
            // 
            checkBox_FastLoadUnload.Anchor = AnchorStyles.Right;
            checkBox_FastLoadUnload.AutoSize = true;
            checkBox_FastLoadUnload.Location = new Point(770, 53);
            checkBox_FastLoadUnload.Name = "checkBox_FastLoadUnload";
            checkBox_FastLoadUnload.Size = new Size(119, 19);
            checkBox_FastLoadUnload.TabIndex = 76;
            checkBox_FastLoadUnload.Text = "Fast Load/Unload";
            checkBox_FastLoadUnload.UseVisualStyleBackColor = true;
            checkBox_FastLoadUnload.CheckedChanged += checkBox_FastLoadUnload_CheckedChanged;
            // 
            // checkBox_LTW
            // 
            checkBox_LTW.Anchor = AnchorStyles.Right;
            checkBox_LTW.AutoSize = true;
            checkBox_LTW.Location = new Point(895, 53);
            checkBox_LTW.Name = "checkBox_LTW";
            checkBox_LTW.Size = new Size(168, 19);
            checkBox_LTW.TabIndex = 69;
            checkBox_LTW.Text = "Loot Through Walls (Risky)";
            checkBox_LTW.UseVisualStyleBackColor = true;
            checkBox_LTW.CheckedChanged += checkBox_LTW_CheckedChanged;
            // 
            // checkBox_WideLean
            // 
            checkBox_WideLean.Anchor = AnchorStyles.Right;
            checkBox_WideLean.AutoSize = true;
            checkBox_WideLean.Location = new Point(1069, 53);
            checkBox_WideLean.Name = "checkBox_WideLean";
            checkBox_WideLean.Size = new Size(119, 19);
            checkBox_WideLean.TabIndex = 65;
            checkBox_WideLean.Text = "Wide Lean (Risky)";
            checkBox_WideLean.UseVisualStyleBackColor = true;
            checkBox_WideLean.CheckedChanged += checkBox_WideLean_CheckedChanged;
            // 
            // checkBox_NoVisor
            // 
            checkBox_NoVisor.Anchor = AnchorStyles.Right;
            checkBox_NoVisor.AutoSize = true;
            checkBox_NoVisor.Location = new Point(3, 80);
            checkBox_NoVisor.Name = "checkBox_NoVisor";
            checkBox_NoVisor.Size = new Size(71, 19);
            checkBox_NoVisor.TabIndex = 37;
            checkBox_NoVisor.Text = "No Visor";
            checkBox_NoVisor.UseVisualStyleBackColor = true;
            checkBox_NoVisor.CheckedChanged += checkBox_NoVisor_CheckedChanged;
            // 
            // checkBox_AlwaysDaySunny
            // 
            checkBox_AlwaysDaySunny.Anchor = AnchorStyles.Right;
            checkBox_AlwaysDaySunny.AutoSize = true;
            checkBox_AlwaysDaySunny.Location = new Point(80, 80);
            checkBox_AlwaysDaySunny.Name = "checkBox_AlwaysDaySunny";
            checkBox_AlwaysDaySunny.Size = new Size(124, 19);
            checkBox_AlwaysDaySunny.TabIndex = 45;
            checkBox_AlwaysDaySunny.Text = "Always Day/Sunny";
            checkBox_AlwaysDaySunny.UseVisualStyleBackColor = true;
            checkBox_AlwaysDaySunny.CheckedChanged += checkBox_AlwaysDay_CheckedChanged;
            // 
            // checkBox_NoWepMalf
            // 
            checkBox_NoWepMalf.Anchor = AnchorStyles.Right;
            checkBox_NoWepMalf.AutoSize = true;
            checkBox_NoWepMalf.Location = new Point(210, 80);
            checkBox_NoWepMalf.Name = "checkBox_NoWepMalf";
            checkBox_NoWepMalf.Size = new Size(142, 19);
            checkBox_NoWepMalf.TabIndex = 58;
            checkBox_NoWepMalf.Text = "No Wep Malfunctions";
            checkBox_NoWepMalf.UseVisualStyleBackColor = true;
            checkBox_NoWepMalf.CheckedChanged += checkBox_NoWepMalf_CheckedChanged;
            // 
            // checkBox_FullBright
            // 
            checkBox_FullBright.Anchor = AnchorStyles.Right;
            checkBox_FullBright.AutoSize = true;
            checkBox_FullBright.Location = new Point(358, 80);
            checkBox_FullBright.Name = "checkBox_FullBright";
            checkBox_FullBright.Size = new Size(80, 19);
            checkBox_FullBright.TabIndex = 71;
            checkBox_FullBright.Text = "Full Bright";
            checkBox_FullBright.UseVisualStyleBackColor = true;
            checkBox_FullBright.CheckedChanged += checkBox_FullBright_CheckedChanged;
            // 
            // checkBox_RageMode
            // 
            checkBox_RageMode.Anchor = AnchorStyles.Right;
            checkBox_RageMode.AutoSize = true;
            checkBox_RageMode.Location = new Point(444, 80);
            checkBox_RageMode.Name = "checkBox_RageMode";
            checkBox_RageMode.Size = new Size(124, 19);
            checkBox_RageMode.TabIndex = 55;
            checkBox_RageMode.Text = "Rage Mode (Risky)";
            checkBox_RageMode.UseVisualStyleBackColor = true;
            checkBox_RageMode.CheckedChanged += checkBox_RageMode_CheckedChanged;
            // 
            // button_AntiAfk
            // 
            button_AntiAfk.Anchor = AnchorStyles.Right;
            button_AntiAfk.Location = new Point(574, 78);
            button_AntiAfk.Name = "button_AntiAfk";
            button_AntiAfk.Size = new Size(96, 23);
            button_AntiAfk.TabIndex = 57;
            button_AntiAfk.Text = "Anti-AFK";
            button_AntiAfk.UseVisualStyleBackColor = true;
            button_AntiAfk.Click += button_AntiAfk_Click;
            // 
            // button_GymHack
            // 
            button_GymHack.Anchor = AnchorStyles.Right;
            flowLayoutPanel_MemWrites.SetFlowBreak(button_GymHack, true);
            button_GymHack.Location = new Point(676, 78);
            button_GymHack.Name = "button_GymHack";
            button_GymHack.Size = new Size(96, 23);
            button_GymHack.TabIndex = 70;
            button_GymHack.Text = "Gym Hack";
            button_GymHack.UseVisualStyleBackColor = true;
            button_GymHack.Click += button_GymHack_Click;
            // 
            // label22
            // 
            label22.Location = new Point(3, 104);
            label22.Name = "label22";
            label22.Size = new Size(0, 0);
            label22.TabIndex = 66;
            label22.Text = "label22";
            // 
            // flowLayoutPanel_Aimbot
            // 
            flowLayoutPanel_Aimbot.AutoSize = true;
            flowLayoutPanel_Aimbot.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_Aimbot.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel_Aimbot.Controls.Add(label13);
            flowLayoutPanel_Aimbot.Controls.Add(label23);
            flowLayoutPanel_Aimbot.Controls.Add(checkBox_SA_SafeLock);
            flowLayoutPanel_Aimbot.Controls.Add(checkBox_SA_AutoBone);
            flowLayoutPanel_Aimbot.Controls.Add(radioButton_AimTarget_FOV);
            flowLayoutPanel_Aimbot.Controls.Add(radioButton_AimTarget_CQB);
            flowLayoutPanel_Aimbot.Controls.Add(checkBox_AimbotDisableReLock);
            flowLayoutPanel_Aimbot.Controls.Add(label_AimFOV);
            flowLayoutPanel_Aimbot.Controls.Add(trackBar_AimFOV);
            flowLayoutPanel_Aimbot.Controls.Add(label10);
            flowLayoutPanel_Aimbot.Controls.Add(comboBox_AimbotTarget);
            flowLayoutPanel_Aimbot.Controls.Add(checkBox_AimHeadAI);
            flowLayoutPanel_Aimbot.Controls.Add(checkBox_AimRandomBone);
            flowLayoutPanel_Aimbot.Controls.Add(button_RandomBoneCfg);
            flowLayoutPanel_Aimbot.Enabled = false;
            flowLayoutPanel_Aimbot.Location = new Point(9, 107);
            flowLayoutPanel_Aimbot.Name = "flowLayoutPanel_Aimbot";
            flowLayoutPanel_Aimbot.Size = new Size(542, 102);
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
            // label23
            // 
            label23.Location = new Point(3, 15);
            label23.Name = "label23";
            label23.Size = new Size(0, 0);
            label23.TabIndex = 70;
            label23.Text = "label23";
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
            radioButton_AimTarget_FOV.CheckedChanged += radioButton_AimbotDefaultMode_CheckedChanged;
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
            radioButton_AimTarget_CQB.CheckedChanged += radioButton_AimbotCQBMode_CheckedChanged;
            // 
            // checkBox_AimbotDisableReLock
            // 
            checkBox_AimbotDisableReLock.Anchor = AnchorStyles.Right;
            checkBox_AimbotDisableReLock.AutoSize = true;
            checkBox_AimbotDisableReLock.Location = new Point(287, 31);
            checkBox_AimbotDisableReLock.Name = "checkBox_AimbotDisableReLock";
            checkBox_AimbotDisableReLock.Size = new Size(110, 19);
            checkBox_AimbotDisableReLock.TabIndex = 59;
            checkBox_AimbotDisableReLock.Text = "Disable Re-Lock";
            checkBox_AimbotDisableReLock.UseVisualStyleBackColor = true;
            checkBox_AimbotDisableReLock.CheckedChanged += checkBox_AimbotDisableReLock_CheckedChanged;
            // 
            // label_AimFOV
            // 
            label_AimFOV.Anchor = AnchorStyles.Right;
            label_AimFOV.AutoSize = true;
            label_AimFOV.Location = new Point(403, 33);
            label_AimFOV.Name = "label_AimFOV";
            label_AimFOV.Size = new Size(50, 15);
            label_AimFOV.TabIndex = 57;
            label_AimFOV.Text = "FOV 150";
            // 
            // trackBar_AimFOV
            // 
            trackBar_AimFOV.Anchor = AnchorStyles.Right;
            trackBar_AimFOV.BackColor = SystemColors.Window;
            flowLayoutPanel_Aimbot.SetFlowBreak(trackBar_AimFOV, true);
            trackBar_AimFOV.Location = new Point(459, 18);
            trackBar_AimFOV.Maximum = 500;
            trackBar_AimFOV.Minimum = 5;
            trackBar_AimFOV.Name = "trackBar_AimFOV";
            trackBar_AimFOV.Size = new Size(78, 45);
            trackBar_AimFOV.TabIndex = 56;
            trackBar_AimFOV.TickStyle = TickStyle.None;
            trackBar_AimFOV.Value = 150;
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
            // checkBox_AimHeadAI
            // 
            checkBox_AimHeadAI.Anchor = AnchorStyles.Right;
            checkBox_AimHeadAI.AutoSize = true;
            checkBox_AimHeadAI.Checked = true;
            checkBox_AimHeadAI.CheckState = CheckState.Checked;
            checkBox_AimHeadAI.Location = new Point(176, 73);
            checkBox_AimHeadAI.Name = "checkBox_AimHeadAI";
            checkBox_AimHeadAI.Size = new Size(91, 19);
            checkBox_AimHeadAI.TabIndex = 2;
            checkBox_AimHeadAI.Text = "Headshot AI";
            checkBox_AimHeadAI.UseVisualStyleBackColor = true;
            checkBox_AimHeadAI.CheckedChanged += checkBox_HeadAI_CheckedChanged;
            // 
            // checkBox_AimRandomBone
            // 
            checkBox_AimRandomBone.Anchor = AnchorStyles.Right;
            checkBox_AimRandomBone.AutoSize = true;
            checkBox_AimRandomBone.Location = new Point(273, 73);
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
            button_RandomBoneCfg.Location = new Point(380, 69);
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
            flowLayoutPanel_NoRecoil.Location = new Point(557, 107);
            flowLayoutPanel_NoRecoil.Name = "flowLayoutPanel_NoRecoil";
            flowLayoutPanel_NoRecoil.Size = new Size(277, 68);
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
            label_Recoil.Size = new Size(48, 15);
            label_Recoil.TabIndex = 50;
            label_Recoil.Text = "Recoil 0";
            // 
            // trackBar_NoRecoil
            // 
            trackBar_NoRecoil.Anchor = AnchorStyles.Right;
            trackBar_NoRecoil.BackColor = SystemColors.Window;
            trackBar_NoRecoil.Location = new Point(57, 18);
            trackBar_NoRecoil.Maximum = 100;
            trackBar_NoRecoil.Name = "trackBar_NoRecoil";
            trackBar_NoRecoil.Size = new Size(80, 45);
            trackBar_NoRecoil.TabIndex = 48;
            trackBar_NoRecoil.TickStyle = TickStyle.None;
            // 
            // label_Sway
            // 
            label_Sway.Anchor = AnchorStyles.Right;
            label_Sway.AutoSize = true;
            label_Sway.Location = new Point(143, 33);
            label_Sway.Name = "label_Sway";
            label_Sway.Size = new Size(43, 15);
            label_Sway.TabIndex = 51;
            label_Sway.Text = "Sway 0";
            // 
            // trackBar_NoSway
            // 
            trackBar_NoSway.Anchor = AnchorStyles.Right;
            trackBar_NoSway.BackColor = SystemColors.Window;
            trackBar_NoSway.Location = new Point(192, 18);
            trackBar_NoSway.Maximum = 100;
            trackBar_NoSway.Name = "trackBar_NoSway";
            trackBar_NoSway.Size = new Size(80, 45);
            trackBar_NoSway.TabIndex = 49;
            trackBar_NoSway.TickStyle = TickStyle.None;
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
            flowLayoutPanel_Chams.Controls.Add(flowLayoutPanel_AdvancedChams);
            flowLayoutPanel_Chams.Enabled = false;
            flowLayoutPanel_Chams.Location = new Point(3, 215);
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
            radioButton_Chams_Basic.CheckedChanged += radioButton_Chams_Normal_CheckedChanged;
            // 
            // radioButton_Chams_Visible
            // 
            radioButton_Chams_Visible.Anchor = AnchorStyles.Right;
            radioButton_Chams_Visible.AutoSize = true;
            radioButton_Chams_Visible.Enabled = false;
            radioButton_Chams_Visible.Location = new Point(61, 56);
            radioButton_Chams_Visible.Name = "radioButton_Chams_Visible";
            radioButton_Chams_Visible.Size = new Size(59, 19);
            radioButton_Chams_Visible.TabIndex = 6;
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
            // flowLayoutPanel_AdvancedChams
            // 
            flowLayoutPanel_AdvancedChams.Anchor = AnchorStyles.Right;
            flowLayoutPanel_AdvancedChams.AutoSize = true;
            flowLayoutPanel_AdvancedChams.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_AdvancedChams.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel_AdvancedChams.Controls.Add(label14);
            flowLayoutPanel_AdvancedChams.Controls.Add(label15);
            flowLayoutPanel_AdvancedChams.Controls.Add(textBox_VischeckVisColor);
            flowLayoutPanel_AdvancedChams.Controls.Add(button_VischeckVisColorPick);
            flowLayoutPanel_AdvancedChams.Controls.Add(label33);
            flowLayoutPanel_AdvancedChams.Controls.Add(textBox_VischeckInvisColor);
            flowLayoutPanel_AdvancedChams.Controls.Add(button_VischeckInvisColorPick);
            flowLayoutPanel_AdvancedChams.Enabled = false;
            flowLayoutPanel_AdvancedChams.Location = new Point(203, 28);
            flowLayoutPanel_AdvancedChams.Name = "flowLayoutPanel_AdvancedChams";
            flowLayoutPanel_AdvancedChams.Size = new Size(249, 75);
            flowLayoutPanel_AdvancedChams.TabIndex = 5;
            // 
            // label14
            // 
            label14.AutoSize = true;
            flowLayoutPanel_AdvancedChams.SetFlowBreak(label14, true);
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
            flowLayoutPanel_AdvancedChams.SetFlowBreak(button_VischeckVisColorPick, true);
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
            // flowLayoutPanel_LTW
            // 
            flowLayoutPanel_LTW.AutoSize = true;
            flowLayoutPanel_LTW.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_LTW.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel_LTW.Controls.Add(label28);
            flowLayoutPanel_LTW.Controls.Add(label_LTWAmount);
            flowLayoutPanel_LTW.Controls.Add(trackBar_LTWAmount);
            flowLayoutPanel_LTW.Enabled = false;
            flowLayoutPanel_LTW.Location = new Point(466, 215);
            flowLayoutPanel_LTW.Name = "flowLayoutPanel_LTW";
            flowLayoutPanel_LTW.Size = new Size(236, 68);
            flowLayoutPanel_LTW.TabIndex = 5;
            // 
            // label28
            // 
            label28.AutoSize = true;
            flowLayoutPanel_LTW.SetFlowBreak(label28, true);
            label28.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label28.Location = new Point(3, 0);
            label28.Name = "label28";
            label28.Size = new Size(112, 15);
            label28.TabIndex = 3;
            label28.Text = "Loot Through Walls";
            // 
            // label_LTWAmount
            // 
            label_LTWAmount.Anchor = AnchorStyles.Right;
            label_LTWAmount.AutoSize = true;
            label_LTWAmount.Location = new Point(3, 33);
            label_LTWAmount.Name = "label_LTWAmount";
            label_LTWAmount.Size = new Size(110, 15);
            label_LTWAmount.TabIndex = 59;
            label_LTWAmount.Text = "Zoom Amount 2.00";
            // 
            // trackBar_LTWAmount
            // 
            trackBar_LTWAmount.Anchor = AnchorStyles.Right;
            trackBar_LTWAmount.BackColor = SystemColors.Window;
            flowLayoutPanel_LTW.SetFlowBreak(trackBar_LTWAmount, true);
            trackBar_LTWAmount.LargeChange = 10;
            trackBar_LTWAmount.Location = new Point(119, 18);
            trackBar_LTWAmount.Maximum = 300;
            trackBar_LTWAmount.Minimum = 100;
            trackBar_LTWAmount.Name = "trackBar_LTWAmount";
            trackBar_LTWAmount.Size = new Size(78, 45);
            trackBar_LTWAmount.TabIndex = 58;
            trackBar_LTWAmount.TickStyle = TickStyle.None;
            trackBar_LTWAmount.Value = 200;
            // 
            // flowLayoutPanel_WideLean
            // 
            flowLayoutPanel_WideLean.AutoSize = true;
            flowLayoutPanel_WideLean.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_WideLean.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel_WideLean.Controls.Add(label_WideLean);
            flowLayoutPanel_WideLean.Controls.Add(label21);
            flowLayoutPanel_WideLean.Controls.Add(comboBox_WideLeanMode);
            flowLayoutPanel_WideLean.Controls.Add(label_WideLeanAmt);
            flowLayoutPanel_WideLean.Controls.Add(trackBar_WideLeanAmt);
            flowLayoutPanel_WideLean.Enabled = false;
            flowLayoutPanel_WideLean.Location = new Point(708, 215);
            flowLayoutPanel_WideLean.Name = "flowLayoutPanel_WideLean";
            flowLayoutPanel_WideLean.Size = new Size(340, 68);
            flowLayoutPanel_WideLean.TabIndex = 64;
            // 
            // label_WideLean
            // 
            label_WideLean.AutoSize = true;
            flowLayoutPanel_WideLean.SetFlowBreak(label_WideLean, true);
            label_WideLean.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label_WideLean.Location = new Point(3, 0);
            label_WideLean.Name = "label_WideLean";
            label_WideLean.Size = new Size(63, 15);
            label_WideLean.TabIndex = 3;
            label_WideLean.Text = "Wide Lean";
            // 
            // label21
            // 
            label21.Anchor = AnchorStyles.Right;
            label21.AutoSize = true;
            label21.Location = new Point(3, 33);
            label21.Name = "label21";
            label21.Size = new Size(38, 15);
            label21.TabIndex = 63;
            label21.Text = "Mode";
            // 
            // comboBox_WideLeanMode
            // 
            comboBox_WideLeanMode.Anchor = AnchorStyles.Right;
            comboBox_WideLeanMode.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_WideLeanMode.FormattingEnabled = true;
            comboBox_WideLeanMode.Location = new Point(47, 29);
            comboBox_WideLeanMode.Name = "comboBox_WideLeanMode";
            comboBox_WideLeanMode.Size = new Size(121, 23);
            comboBox_WideLeanMode.TabIndex = 62;
            // 
            // label_WideLeanAmt
            // 
            label_WideLeanAmt.Anchor = AnchorStyles.Right;
            label_WideLeanAmt.AutoSize = true;
            label_WideLeanAmt.Location = new Point(174, 33);
            label_WideLeanAmt.Name = "label_WideLeanAmt";
            label_WideLeanAmt.Size = new Size(75, 15);
            label_WideLeanAmt.TabIndex = 65;
            label_WideLeanAmt.Text = "Amount 0.50";
            // 
            // trackBar_WideLeanAmt
            // 
            trackBar_WideLeanAmt.Anchor = AnchorStyles.Right;
            trackBar_WideLeanAmt.BackColor = SystemColors.Window;
            trackBar_WideLeanAmt.Location = new Point(255, 18);
            trackBar_WideLeanAmt.Maximum = 100;
            trackBar_WideLeanAmt.Minimum = 1;
            trackBar_WideLeanAmt.Name = "trackBar_WideLeanAmt";
            trackBar_WideLeanAmt.Size = new Size(80, 45);
            trackBar_WideLeanAmt.TabIndex = 64;
            trackBar_WideLeanAmt.TickStyle = TickStyle.None;
            trackBar_WideLeanAmt.Value = 50;
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
            flowLayoutPanel_MonitorSettings.Location = new Point(3, 995);
            flowLayoutPanel_MonitorSettings.Name = "flowLayoutPanel_MonitorSettings";
            flowLayoutPanel_MonitorSettings.Size = new Size(1230, 70);
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
            flowLayoutPanel_ESPSettings.Controls.Add(label26);
            flowLayoutPanel_ESPSettings.Controls.Add(button_StartESP);
            flowLayoutPanel_ESPSettings.Controls.Add(button_EspColorPicker);
            flowLayoutPanel_ESPSettings.Controls.Add(label_ESPFPSCap);
            flowLayoutPanel_ESPSettings.Controls.Add(textBox_EspFpsCap);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_AutoFS);
            flowLayoutPanel_ESPSettings.Controls.Add(comboBox_ESPAutoFS);
            flowLayoutPanel_ESPSettings.Controls.Add(label_ESP_HighAlert);
            flowLayoutPanel_ESPSettings.Controls.Add(comboBox_ESP_HighAlert);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_Loot);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_Exfils);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_Explosives);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_ShowMag);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_Dist);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_ShowMines);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_FireportAim);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_AimFov);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_AimLock);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_RaidStats);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_StatusText);
            flowLayoutPanel_ESPSettings.Controls.Add(checkBox_ESP_FPS);
            flowLayoutPanel_ESPSettings.Controls.Add(label27);
            flowLayoutPanel_ESPSettings.Controls.Add(flowLayoutPanel_ESP_PlayerRender);
            flowLayoutPanel_ESPSettings.Controls.Add(flowLayoutPanel_ESP_AIRender);
            flowLayoutPanel_ESPSettings.Controls.Add(flowLayoutPanel4);
            flowLayoutPanel_ESPSettings.Dock = DockStyle.Top;
            flowLayoutPanel_Settings.SetFlowBreak(flowLayoutPanel_ESPSettings, true);
            flowLayoutPanel_ESPSettings.Location = new Point(3, 1071);
            flowLayoutPanel_ESPSettings.Name = "flowLayoutPanel_ESPSettings";
            flowLayoutPanel_ESPSettings.Size = new Size(1230, 311);
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
            // label26
            // 
            label26.Location = new Point(3, 21);
            label26.Name = "label26";
            label26.Size = new Size(0, 0);
            label26.TabIndex = 70;
            label26.Text = "label26";
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
            comboBox_ESPAutoFS.FormattingEnabled = true;
            comboBox_ESPAutoFS.Location = new Point(474, 33);
            comboBox_ESPAutoFS.Name = "comboBox_ESPAutoFS";
            comboBox_ESPAutoFS.Size = new Size(121, 23);
            comboBox_ESPAutoFS.TabIndex = 56;
            comboBox_ESPAutoFS.SelectedIndexChanged += comboBox_ESPAutoFS_SelectedIndexChanged;
            // 
            // label_ESP_HighAlert
            // 
            label_ESP_HighAlert.Anchor = AnchorStyles.Right;
            label_ESP_HighAlert.AutoSize = true;
            label_ESP_HighAlert.Location = new Point(601, 37);
            label_ESP_HighAlert.Name = "label_ESP_HighAlert";
            label_ESP_HighAlert.Size = new Size(61, 15);
            label_ESP_HighAlert.TabIndex = 61;
            label_ESP_HighAlert.Text = "High Alert";
            // 
            // comboBox_ESP_HighAlert
            // 
            comboBox_ESP_HighAlert.Anchor = AnchorStyles.Right;
            comboBox_ESP_HighAlert.DropDownStyle = ComboBoxStyle.DropDownList;
            flowLayoutPanel_ESPSettings.SetFlowBreak(comboBox_ESP_HighAlert, true);
            comboBox_ESP_HighAlert.FormattingEnabled = true;
            comboBox_ESP_HighAlert.Location = new Point(668, 33);
            comboBox_ESP_HighAlert.Name = "comboBox_ESP_HighAlert";
            comboBox_ESP_HighAlert.Size = new Size(121, 23);
            comboBox_ESP_HighAlert.TabIndex = 62;
            comboBox_ESP_HighAlert.SelectedIndexChanged += comboBox_ESP_HighAlert_SelectedIndexChanged;
            // 
            // checkBox_ESP_Loot
            // 
            checkBox_ESP_Loot.AutoSize = true;
            checkBox_ESP_Loot.Location = new Point(3, 71);
            checkBox_ESP_Loot.Name = "checkBox_ESP_Loot";
            checkBox_ESP_Loot.Size = new Size(82, 19);
            checkBox_ESP_Loot.TabIndex = 58;
            checkBox_ESP_Loot.Text = "Show Loot";
            checkBox_ESP_Loot.UseVisualStyleBackColor = true;
            checkBox_ESP_Loot.CheckedChanged += checkBox_ESP_Loot_CheckedChanged;
            // 
            // checkBox_ESP_Exfils
            // 
            checkBox_ESP_Exfils.AutoSize = true;
            checkBox_ESP_Exfils.Location = new Point(91, 71);
            checkBox_ESP_Exfils.Name = "checkBox_ESP_Exfils";
            checkBox_ESP_Exfils.Size = new Size(84, 19);
            checkBox_ESP_Exfils.TabIndex = 7;
            checkBox_ESP_Exfils.Text = "Show Exfils";
            checkBox_ESP_Exfils.UseVisualStyleBackColor = true;
            checkBox_ESP_Exfils.CheckedChanged += checkBox_ESP_Exfils_CheckedChanged;
            // 
            // checkBox_ESP_Explosives
            // 
            checkBox_ESP_Explosives.AutoSize = true;
            checkBox_ESP_Explosives.Location = new Point(181, 71);
            checkBox_ESP_Explosives.Name = "checkBox_ESP_Explosives";
            checkBox_ESP_Explosives.Size = new Size(111, 19);
            checkBox_ESP_Explosives.TabIndex = 8;
            checkBox_ESP_Explosives.Text = "Show Explosives";
            checkBox_ESP_Explosives.UseVisualStyleBackColor = true;
            checkBox_ESP_Explosives.CheckedChanged += checkBox_ESP_Explosives_CheckedChanged;
            // 
            // checkBox_ESP_ShowMag
            // 
            checkBox_ESP_ShowMag.AutoSize = true;
            checkBox_ESP_ShowMag.Location = new Point(298, 71);
            checkBox_ESP_ShowMag.Name = "checkBox_ESP_ShowMag";
            checkBox_ESP_ShowMag.Size = new Size(109, 19);
            checkBox_ESP_ShowMag.TabIndex = 60;
            checkBox_ESP_ShowMag.Text = "Show Magazine";
            checkBox_ESP_ShowMag.UseVisualStyleBackColor = true;
            checkBox_ESP_ShowMag.CheckedChanged += checkBox_ESP_ShowMag_CheckedChanged;
            // 
            // checkBox_ESP_Dist
            // 
            checkBox_ESP_Dist.AutoSize = true;
            checkBox_ESP_Dist.Location = new Point(413, 71);
            checkBox_ESP_Dist.Name = "checkBox_ESP_Dist";
            checkBox_ESP_Dist.Size = new Size(108, 19);
            checkBox_ESP_Dist.TabIndex = 48;
            checkBox_ESP_Dist.Text = "Show Distances";
            checkBox_ESP_Dist.UseVisualStyleBackColor = true;
            checkBox_ESP_Dist.CheckedChanged += checkBox_ESP_Dist_CheckedChanged;
            // 
            // checkBox_ESP_ShowMines
            // 
            checkBox_ESP_ShowMines.AutoSize = true;
            checkBox_ESP_ShowMines.Location = new Point(527, 71);
            checkBox_ESP_ShowMines.Name = "checkBox_ESP_ShowMines";
            checkBox_ESP_ShowMines.Size = new Size(90, 19);
            checkBox_ESP_ShowMines.TabIndex = 59;
            checkBox_ESP_ShowMines.Text = "Show Mines";
            checkBox_ESP_ShowMines.UseVisualStyleBackColor = true;
            checkBox_ESP_ShowMines.CheckedChanged += checkBox_ESP_ShowMines_CheckedChanged;
            // 
            // checkBox_ESP_FireportAim
            // 
            checkBox_ESP_FireportAim.AutoSize = true;
            checkBox_ESP_FireportAim.Location = new Point(623, 71);
            checkBox_ESP_FireportAim.Name = "checkBox_ESP_FireportAim";
            checkBox_ESP_FireportAim.Size = new Size(124, 19);
            checkBox_ESP_FireportAim.TabIndex = 71;
            checkBox_ESP_FireportAim.Text = "Show Fireport Aim";
            checkBox_ESP_FireportAim.UseVisualStyleBackColor = true;
            checkBox_ESP_FireportAim.CheckedChanged += checkBox_ESP_FireportAim_CheckedChanged;
            // 
            // checkBox_ESP_AimFov
            // 
            checkBox_ESP_AimFov.AutoSize = true;
            checkBox_ESP_AimFov.Location = new Point(753, 71);
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
            checkBox_ESP_AimLock.Location = new Point(864, 71);
            checkBox_ESP_AimLock.Name = "checkBox_ESP_AimLock";
            checkBox_ESP_AimLock.Size = new Size(126, 19);
            checkBox_ESP_AimLock.TabIndex = 49;
            checkBox_ESP_AimLock.Text = "Show Aimbot Lock";
            checkBox_ESP_AimLock.UseVisualStyleBackColor = true;
            checkBox_ESP_AimLock.CheckedChanged += checkBox_ESP_AimLock_CheckedChanged;
            // 
            // checkBox_ESP_RaidStats
            // 
            checkBox_ESP_RaidStats.AutoSize = true;
            checkBox_ESP_RaidStats.Location = new Point(996, 71);
            checkBox_ESP_RaidStats.Name = "checkBox_ESP_RaidStats";
            checkBox_ESP_RaidStats.Size = new Size(109, 19);
            checkBox_ESP_RaidStats.TabIndex = 68;
            checkBox_ESP_RaidStats.Text = "Show Raid Stats";
            checkBox_ESP_RaidStats.UseVisualStyleBackColor = true;
            checkBox_ESP_RaidStats.CheckedChanged += checkBox_ESP_RaidStats_CheckedChanged;
            // 
            // checkBox_ESP_StatusText
            // 
            checkBox_ESP_StatusText.AutoSize = true;
            checkBox_ESP_StatusText.Location = new Point(1111, 71);
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
            checkBox_ESP_FPS.Location = new Point(3, 96);
            checkBox_ESP_FPS.Name = "checkBox_ESP_FPS";
            checkBox_ESP_FPS.Size = new Size(86, 19);
            checkBox_ESP_FPS.TabIndex = 5;
            checkBox_ESP_FPS.Text = "Display FPS";
            checkBox_ESP_FPS.UseVisualStyleBackColor = true;
            checkBox_ESP_FPS.CheckedChanged += checkBox_ESP_FPS_CheckedChanged;
            // 
            // label27
            // 
            label27.AutoSize = true;
            flowLayoutPanel_ESPSettings.SetFlowBreak(label27, true);
            label27.Location = new Point(95, 93);
            label27.Name = "label27";
            label27.Size = new Size(0, 15);
            label27.TabIndex = 72;
            // 
            // flowLayoutPanel_ESP_PlayerRender
            // 
            flowLayoutPanel_ESP_PlayerRender.AutoSize = true;
            flowLayoutPanel_ESP_PlayerRender.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_ESP_PlayerRender.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel_ESP_PlayerRender.Controls.Add(label18);
            flowLayoutPanel_ESP_PlayerRender.Controls.Add(radioButton_ESPRender_None);
            flowLayoutPanel_ESP_PlayerRender.Controls.Add(radioButton_ESPRender_Bones);
            flowLayoutPanel_ESP_PlayerRender.Controls.Add(radioButton_ESPRender_Box);
            flowLayoutPanel_ESP_PlayerRender.Controls.Add(radioButton_ESPRender_Presence);
            flowLayoutPanel_ESP_PlayerRender.Controls.Add(checkBox_ESPRender_Labels);
            flowLayoutPanel_ESP_PlayerRender.Controls.Add(checkBox_ESPRender_Weapons);
            flowLayoutPanel_ESP_PlayerRender.Controls.Add(checkBox_ESPRender_Dist);
            flowLayoutPanel_ESP_PlayerRender.Location = new Point(3, 121);
            flowLayoutPanel_ESP_PlayerRender.Name = "flowLayoutPanel_ESP_PlayerRender";
            flowLayoutPanel_ESP_PlayerRender.Size = new Size(253, 77);
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
            radioButton_ESPRender_Bones.Location = new Point(63, 28);
            radioButton_ESPRender_Bones.Name = "radioButton_ESPRender_Bones";
            radioButton_ESPRender_Bones.Size = new Size(57, 19);
            radioButton_ESPRender_Bones.TabIndex = 64;
            radioButton_ESPRender_Bones.TabStop = true;
            radioButton_ESPRender_Bones.Text = "Bones";
            radioButton_ESPRender_Bones.UseVisualStyleBackColor = true;
            radioButton_ESPRender_Bones.CheckedChanged += radioButton_ESPRender_Bones_CheckedChanged;
            // 
            // radioButton_ESPRender_Box
            // 
            radioButton_ESPRender_Box.AutoSize = true;
            radioButton_ESPRender_Box.Location = new Point(126, 28);
            radioButton_ESPRender_Box.Name = "radioButton_ESPRender_Box";
            radioButton_ESPRender_Box.Size = new Size(44, 19);
            radioButton_ESPRender_Box.TabIndex = 65;
            radioButton_ESPRender_Box.Text = "Box";
            radioButton_ESPRender_Box.UseVisualStyleBackColor = true;
            radioButton_ESPRender_Box.CheckedChanged += radioButton_ESPRender_Box_CheckedChanged;
            // 
            // radioButton_ESPRender_Presence
            // 
            radioButton_ESPRender_Presence.AutoSize = true;
            flowLayoutPanel_ESP_PlayerRender.SetFlowBreak(radioButton_ESPRender_Presence, true);
            radioButton_ESPRender_Presence.Location = new Point(176, 28);
            radioButton_ESPRender_Presence.Name = "radioButton_ESPRender_Presence";
            radioButton_ESPRender_Presence.Size = new Size(72, 19);
            radioButton_ESPRender_Presence.TabIndex = 67;
            radioButton_ESPRender_Presence.TabStop = true;
            radioButton_ESPRender_Presence.Text = "Presence";
            radioButton_ESPRender_Presence.UseVisualStyleBackColor = true;
            radioButton_ESPRender_Presence.CheckedChanged += radioButton_ESPRender_Presence_CheckedChanged;
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
            // flowLayoutPanel_ESP_AIRender
            // 
            flowLayoutPanel_ESP_AIRender.AutoSize = true;
            flowLayoutPanel_ESP_AIRender.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_ESP_AIRender.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel_ESP_AIRender.Controls.Add(label19);
            flowLayoutPanel_ESP_AIRender.Controls.Add(radioButton_ESPAIRender_None);
            flowLayoutPanel_ESP_AIRender.Controls.Add(radioButton_ESPAIRender_Bones);
            flowLayoutPanel_ESP_AIRender.Controls.Add(radioButton_ESPAIRender_Box);
            flowLayoutPanel_ESP_AIRender.Controls.Add(radioButton_ESPAIRender_Presence);
            flowLayoutPanel_ESP_AIRender.Controls.Add(checkBox_ESPAIRender_Labels);
            flowLayoutPanel_ESP_AIRender.Controls.Add(checkBox_ESPAIRender_Weapons);
            flowLayoutPanel_ESP_AIRender.Controls.Add(checkBox_ESPAIRender_Dist);
            flowLayoutPanel_ESPSettings.SetFlowBreak(flowLayoutPanel_ESP_AIRender, true);
            flowLayoutPanel_ESP_AIRender.Location = new Point(262, 121);
            flowLayoutPanel_ESP_AIRender.Name = "flowLayoutPanel_ESP_AIRender";
            flowLayoutPanel_ESP_AIRender.Size = new Size(253, 77);
            flowLayoutPanel_ESP_AIRender.TabIndex = 5;
            // 
            // label19
            // 
            label19.AutoSize = true;
            flowLayoutPanel_ESP_AIRender.SetFlowBreak(label19, true);
            label19.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label19.Location = new Point(3, 0);
            label19.Name = "label19";
            label19.Size = new Size(93, 15);
            label19.TabIndex = 74;
            label19.Text = "AI Render Mode";
            // 
            // radioButton_ESPAIRender_None
            // 
            radioButton_ESPAIRender_None.AutoSize = true;
            radioButton_ESPAIRender_None.Location = new Point(3, 28);
            radioButton_ESPAIRender_None.Name = "radioButton_ESPAIRender_None";
            radioButton_ESPAIRender_None.Size = new Size(54, 19);
            radioButton_ESPAIRender_None.TabIndex = 63;
            radioButton_ESPAIRender_None.Text = "None";
            radioButton_ESPAIRender_None.UseVisualStyleBackColor = true;
            radioButton_ESPAIRender_None.CheckedChanged += radioButton_ESPAIRender_None_CheckedChanged;
            // 
            // radioButton_ESPAIRender_Bones
            // 
            radioButton_ESPAIRender_Bones.AutoSize = true;
            radioButton_ESPAIRender_Bones.Checked = true;
            radioButton_ESPAIRender_Bones.Location = new Point(63, 28);
            radioButton_ESPAIRender_Bones.Name = "radioButton_ESPAIRender_Bones";
            radioButton_ESPAIRender_Bones.Size = new Size(57, 19);
            radioButton_ESPAIRender_Bones.TabIndex = 64;
            radioButton_ESPAIRender_Bones.TabStop = true;
            radioButton_ESPAIRender_Bones.Text = "Bones";
            radioButton_ESPAIRender_Bones.UseVisualStyleBackColor = true;
            radioButton_ESPAIRender_Bones.CheckedChanged += radioButton_ESPAIRender_Bones_CheckedChanged;
            // 
            // radioButton_ESPAIRender_Box
            // 
            radioButton_ESPAIRender_Box.AutoSize = true;
            radioButton_ESPAIRender_Box.Location = new Point(126, 28);
            radioButton_ESPAIRender_Box.Name = "radioButton_ESPAIRender_Box";
            radioButton_ESPAIRender_Box.Size = new Size(44, 19);
            radioButton_ESPAIRender_Box.TabIndex = 65;
            radioButton_ESPAIRender_Box.Text = "Box";
            radioButton_ESPAIRender_Box.UseVisualStyleBackColor = true;
            radioButton_ESPAIRender_Box.CheckedChanged += radioButton_ESPAIRender_Box_CheckedChanged;
            // 
            // radioButton_ESPAIRender_Presence
            // 
            radioButton_ESPAIRender_Presence.AutoSize = true;
            flowLayoutPanel_ESP_AIRender.SetFlowBreak(radioButton_ESPAIRender_Presence, true);
            radioButton_ESPAIRender_Presence.Location = new Point(176, 28);
            radioButton_ESPAIRender_Presence.Name = "radioButton_ESPAIRender_Presence";
            radioButton_ESPAIRender_Presence.Size = new Size(72, 19);
            radioButton_ESPAIRender_Presence.TabIndex = 67;
            radioButton_ESPAIRender_Presence.Text = "Presence";
            radioButton_ESPAIRender_Presence.UseVisualStyleBackColor = true;
            radioButton_ESPAIRender_Presence.CheckedChanged += radioButton_ESPAIRender_Presence_CheckedChanged;
            // 
            // checkBox_ESPAIRender_Labels
            // 
            checkBox_ESPAIRender_Labels.AutoSize = true;
            checkBox_ESPAIRender_Labels.Location = new Point(3, 53);
            checkBox_ESPAIRender_Labels.Name = "checkBox_ESPAIRender_Labels";
            checkBox_ESPAIRender_Labels.Size = new Size(59, 19);
            checkBox_ESPAIRender_Labels.TabIndex = 71;
            checkBox_ESPAIRender_Labels.Text = "Labels";
            checkBox_ESPAIRender_Labels.UseVisualStyleBackColor = true;
            checkBox_ESPAIRender_Labels.CheckedChanged += checkBox_ESPAIRender_Labels_CheckedChanged;
            // 
            // checkBox_ESPAIRender_Weapons
            // 
            checkBox_ESPAIRender_Weapons.AutoSize = true;
            checkBox_ESPAIRender_Weapons.Location = new Point(68, 53);
            checkBox_ESPAIRender_Weapons.Name = "checkBox_ESPAIRender_Weapons";
            checkBox_ESPAIRender_Weapons.Size = new Size(75, 19);
            checkBox_ESPAIRender_Weapons.TabIndex = 72;
            checkBox_ESPAIRender_Weapons.Text = "Weapons";
            checkBox_ESPAIRender_Weapons.UseVisualStyleBackColor = true;
            checkBox_ESPAIRender_Weapons.CheckedChanged += checkBox_ESPAIRender_Weapons_CheckedChanged;
            // 
            // checkBox_ESPAIRender_Dist
            // 
            checkBox_ESPAIRender_Dist.AutoSize = true;
            checkBox_ESPAIRender_Dist.Location = new Point(149, 53);
            checkBox_ESPAIRender_Dist.Name = "checkBox_ESPAIRender_Dist";
            checkBox_ESPAIRender_Dist.Size = new Size(46, 19);
            checkBox_ESPAIRender_Dist.TabIndex = 73;
            checkBox_ESPAIRender_Dist.Text = "Dist";
            checkBox_ESPAIRender_Dist.UseVisualStyleBackColor = true;
            checkBox_ESPAIRender_Dist.CheckedChanged += checkBox_ESPAIRender_Dist_CheckedChanged;
            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.AutoSize = true;
            flowLayoutPanel4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel4.Controls.Add(label_EspLootDist);
            flowLayoutPanel4.Controls.Add(trackBar_EspLootDist);
            flowLayoutPanel4.Controls.Add(label_EspImpLootDist);
            flowLayoutPanel4.Controls.Add(trackBar_EspImpLootDist);
            flowLayoutPanel4.Controls.Add(label_ESPContainerDist);
            flowLayoutPanel4.Controls.Add(trackBar_ESPContainerDist);
            flowLayoutPanel4.Controls.Add(label_EspQuestHelperDist);
            flowLayoutPanel4.Controls.Add(trackBar_EspQuestHelperDist);
            flowLayoutPanel4.Controls.Add(label_EspExplosivesDist);
            flowLayoutPanel4.Controls.Add(trackBar_EspGrenadeDist);
            flowLayoutPanel4.Controls.Add(label_EspFontScale);
            flowLayoutPanel4.Controls.Add(trackBar_EspFontScale);
            flowLayoutPanel4.Controls.Add(label_EspLineScale);
            flowLayoutPanel4.Controls.Add(trackBar_EspLineScale);
            flowLayoutPanel4.Location = new Point(3, 204);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new Size(714, 102);
            flowLayoutPanel4.TabIndex = 4;
            // 
            // label_EspLootDist
            // 
            label_EspLootDist.Anchor = AnchorStyles.Right;
            label_EspLootDist.AutoSize = true;
            label_EspLootDist.Location = new Point(3, 18);
            label_EspLootDist.Name = "label_EspLootDist";
            label_EspLootDist.Size = new Size(69, 15);
            label_EspLootDist.TabIndex = 44;
            label_EspLootDist.Text = "Loot Dist 50";
            // 
            // trackBar_EspLootDist
            // 
            trackBar_EspLootDist.Anchor = AnchorStyles.Right;
            trackBar_EspLootDist.BackColor = SystemColors.Window;
            trackBar_EspLootDist.Location = new Point(78, 3);
            trackBar_EspLootDist.Maximum = 1000;
            trackBar_EspLootDist.Minimum = 5;
            trackBar_EspLootDist.Name = "trackBar_EspLootDist";
            trackBar_EspLootDist.Size = new Size(82, 45);
            trackBar_EspLootDist.TabIndex = 12;
            trackBar_EspLootDist.TickStyle = TickStyle.None;
            trackBar_EspLootDist.Value = 50;
            // 
            // label_EspImpLootDist
            // 
            label_EspImpLootDist.Anchor = AnchorStyles.Right;
            label_EspImpLootDist.AutoSize = true;
            label_EspImpLootDist.Location = new Point(166, 18);
            label_EspImpLootDist.Name = "label_EspImpLootDist";
            label_EspImpLootDist.Size = new Size(93, 15);
            label_EspImpLootDist.TabIndex = 51;
            label_EspImpLootDist.Text = "Imp Loot Dist 50";
            // 
            // trackBar_EspImpLootDist
            // 
            trackBar_EspImpLootDist.Anchor = AnchorStyles.Right;
            trackBar_EspImpLootDist.BackColor = SystemColors.Window;
            trackBar_EspImpLootDist.Location = new Point(265, 3);
            trackBar_EspImpLootDist.Maximum = 1000;
            trackBar_EspImpLootDist.Minimum = 5;
            trackBar_EspImpLootDist.Name = "trackBar_EspImpLootDist";
            trackBar_EspImpLootDist.Size = new Size(82, 45);
            trackBar_EspImpLootDist.TabIndex = 52;
            trackBar_EspImpLootDist.TickStyle = TickStyle.None;
            trackBar_EspImpLootDist.Value = 50;
            // 
            // label_ESPContainerDist
            // 
            label_ESPContainerDist.Anchor = AnchorStyles.Right;
            label_ESPContainerDist.AutoSize = true;
            label_ESPContainerDist.Location = new Point(353, 18);
            label_ESPContainerDist.Name = "label_ESPContainerDist";
            label_ESPContainerDist.Size = new Size(97, 15);
            label_ESPContainerDist.TabIndex = 76;
            label_ESPContainerDist.Text = "Container Dist 50";
            // 
            // trackBar_ESPContainerDist
            // 
            trackBar_ESPContainerDist.Anchor = AnchorStyles.Right;
            trackBar_ESPContainerDist.BackColor = SystemColors.Window;
            flowLayoutPanel4.SetFlowBreak(trackBar_ESPContainerDist, true);
            trackBar_ESPContainerDist.Location = new Point(456, 3);
            trackBar_ESPContainerDist.Maximum = 1000;
            trackBar_ESPContainerDist.Minimum = 5;
            trackBar_ESPContainerDist.Name = "trackBar_ESPContainerDist";
            trackBar_ESPContainerDist.Size = new Size(78, 45);
            trackBar_ESPContainerDist.TabIndex = 75;
            trackBar_ESPContainerDist.TickStyle = TickStyle.None;
            trackBar_ESPContainerDist.Value = 50;
            // 
            // label_EspQuestHelperDist
            // 
            label_EspQuestHelperDist.Anchor = AnchorStyles.Right;
            label_EspQuestHelperDist.AutoSize = true;
            label_EspQuestHelperDist.Location = new Point(3, 69);
            label_EspQuestHelperDist.Name = "label_EspQuestHelperDist";
            label_EspQuestHelperDist.Size = new Size(82, 15);
            label_EspQuestHelperDist.TabIndex = 53;
            label_EspQuestHelperDist.Text = "Quest Dist 100";
            // 
            // trackBar_EspQuestHelperDist
            // 
            trackBar_EspQuestHelperDist.Anchor = AnchorStyles.Right;
            trackBar_EspQuestHelperDist.BackColor = SystemColors.Window;
            trackBar_EspQuestHelperDist.Location = new Point(91, 54);
            trackBar_EspQuestHelperDist.Maximum = 1000;
            trackBar_EspQuestHelperDist.Minimum = 5;
            trackBar_EspQuestHelperDist.Name = "trackBar_EspQuestHelperDist";
            trackBar_EspQuestHelperDist.Size = new Size(82, 45);
            trackBar_EspQuestHelperDist.TabIndex = 54;
            trackBar_EspQuestHelperDist.TickStyle = TickStyle.None;
            trackBar_EspQuestHelperDist.Value = 100;
            // 
            // label_EspExplosivesDist
            // 
            label_EspExplosivesDist.Anchor = AnchorStyles.Right;
            label_EspExplosivesDist.AutoSize = true;
            label_EspExplosivesDist.Location = new Point(179, 69);
            label_EspExplosivesDist.Name = "label_EspExplosivesDist";
            label_EspExplosivesDist.Size = new Size(98, 15);
            label_EspExplosivesDist.TabIndex = 42;
            label_EspExplosivesDist.Text = "Explosives Dist 50";
            // 
            // trackBar_EspGrenadeDist
            // 
            trackBar_EspGrenadeDist.Anchor = AnchorStyles.Right;
            trackBar_EspGrenadeDist.BackColor = SystemColors.Window;
            trackBar_EspGrenadeDist.Location = new Point(283, 54);
            trackBar_EspGrenadeDist.Maximum = 1000;
            trackBar_EspGrenadeDist.Minimum = 15;
            trackBar_EspGrenadeDist.Name = "trackBar_EspGrenadeDist";
            trackBar_EspGrenadeDist.Size = new Size(82, 45);
            trackBar_EspGrenadeDist.TabIndex = 43;
            trackBar_EspGrenadeDist.TickStyle = TickStyle.None;
            trackBar_EspGrenadeDist.Value = 50;
            // 
            // label_EspFontScale
            // 
            label_EspFontScale.Anchor = AnchorStyles.Right;
            label_EspFontScale.AutoSize = true;
            label_EspFontScale.Location = new Point(371, 69);
            label_EspFontScale.Name = "label_EspFontScale";
            label_EspFontScale.Size = new Size(85, 15);
            label_EspFontScale.TabIndex = 16;
            label_EspFontScale.Text = "Font Scale 1.00";
            // 
            // trackBar_EspFontScale
            // 
            trackBar_EspFontScale.Anchor = AnchorStyles.Right;
            trackBar_EspFontScale.BackColor = SystemColors.Window;
            trackBar_EspFontScale.Location = new Point(462, 54);
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
            label_EspLineScale.Location = new Point(545, 69);
            label_EspLineScale.Name = "label_EspLineScale";
            label_EspLineScale.Size = new Size(83, 15);
            label_EspLineScale.TabIndex = 55;
            label_EspLineScale.Text = "Line Scale 1.00";
            // 
            // trackBar_EspLineScale
            // 
            trackBar_EspLineScale.Anchor = AnchorStyles.Right;
            trackBar_EspLineScale.BackColor = SystemColors.Window;
            trackBar_EspLineScale.Location = new Point(634, 54);
            trackBar_EspLineScale.Maximum = 200;
            trackBar_EspLineScale.Minimum = 10;
            trackBar_EspLineScale.Name = "trackBar_EspLineScale";
            trackBar_EspLineScale.Size = new Size(77, 45);
            trackBar_EspLineScale.TabIndex = 56;
            trackBar_EspLineScale.TickStyle = TickStyle.None;
            trackBar_EspLineScale.Value = 100;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(richTextBox_PlayersInfo);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(1256, 653);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Player Loadouts";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // richTextBox_PlayersInfo
            // 
            richTextBox_PlayersInfo.Dock = DockStyle.Fill;
            richTextBox_PlayersInfo.Font = new Font("Arial", 9F);
            richTextBox_PlayersInfo.Location = new Point(0, 0);
            richTextBox_PlayersInfo.Name = "richTextBox_PlayersInfo";
            richTextBox_PlayersInfo.ReadOnly = true;
            richTextBox_PlayersInfo.Size = new Size(1256, 653);
            richTextBox_PlayersInfo.TabIndex = 0;
            richTextBox_PlayersInfo.Text = "";
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(dataGridView_PlayerHistory);
            tabPage4.Location = new Point(4, 24);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new Size(1256, 653);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Player History";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // dataGridView_PlayerHistory
            // 
            dataGridView_PlayerHistory.AllowUserToAddRows = false;
            dataGridView_PlayerHistory.AllowUserToDeleteRows = false;
            dataGridView_PlayerHistory.AllowUserToResizeColumns = false;
            dataGridView_PlayerHistory.AllowUserToResizeRows = false;
            dataGridView_PlayerHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_PlayerHistory.Columns.AddRange(new DataGridViewColumn[] { PHistory_Name, PHistory_AccountID, PHistory_Acct, PHistory_Type, PHistory_KD, PHistory_Hours, PHistory_Raids, PHistory_SR, PHistory_Group, PHistory_Alerts });
            dataGridView_PlayerHistory.Dock = DockStyle.Fill;
            dataGridView_PlayerHistory.Location = new Point(0, 0);
            dataGridView_PlayerHistory.MultiSelect = false;
            dataGridView_PlayerHistory.Name = "dataGridView_PlayerHistory";
            dataGridView_PlayerHistory.ReadOnly = true;
            dataGridView_PlayerHistory.RowHeadersVisible = false;
            dataGridView_PlayerHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_PlayerHistory.Size = new Size(1256, 653);
            dataGridView_PlayerHistory.TabIndex = 0;
            // 
            // PHistory_Name
            // 
            PHistory_Name.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            PHistory_Name.DataPropertyName = "Name";
            PHistory_Name.HeaderText = "Name";
            PHistory_Name.MaxInputLength = 64;
            PHistory_Name.Name = "PHistory_Name";
            PHistory_Name.ReadOnly = true;
            PHistory_Name.SortMode = DataGridViewColumnSortMode.NotSortable;
            PHistory_Name.Width = 45;
            // 
            // PHistory_AccountID
            // 
            PHistory_AccountID.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            PHistory_AccountID.DataPropertyName = "ID";
            PHistory_AccountID.HeaderText = "Account ID";
            PHistory_AccountID.MaxInputLength = 16;
            PHistory_AccountID.Name = "PHistory_AccountID";
            PHistory_AccountID.ReadOnly = true;
            PHistory_AccountID.SortMode = DataGridViewColumnSortMode.NotSortable;
            PHistory_AccountID.Width = 72;
            // 
            // PHistory_Acct
            // 
            PHistory_Acct.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            PHistory_Acct.DataPropertyName = "Acct";
            PHistory_Acct.HeaderText = "Acct";
            PHistory_Acct.MaxInputLength = 10;
            PHistory_Acct.Name = "PHistory_Acct";
            PHistory_Acct.ReadOnly = true;
            PHistory_Acct.Width = 56;
            // 
            // PHistory_Type
            // 
            PHistory_Type.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            PHistory_Type.DataPropertyName = "Type";
            PHistory_Type.HeaderText = "Type";
            PHistory_Type.MaxInputLength = 128;
            PHistory_Type.Name = "PHistory_Type";
            PHistory_Type.ReadOnly = true;
            PHistory_Type.SortMode = DataGridViewColumnSortMode.NotSortable;
            PHistory_Type.Width = 38;
            // 
            // PHistory_KD
            // 
            PHistory_KD.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            PHistory_KD.DataPropertyName = "KD";
            PHistory_KD.HeaderText = "K/D";
            PHistory_KD.Name = "PHistory_KD";
            PHistory_KD.ReadOnly = true;
            PHistory_KD.Width = 52;
            // 
            // PHistory_Hours
            // 
            PHistory_Hours.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            PHistory_Hours.DataPropertyName = "Hours";
            PHistory_Hours.HeaderText = "Total Hours";
            PHistory_Hours.MaxInputLength = 6;
            PHistory_Hours.Name = "PHistory_Hours";
            PHistory_Hours.ReadOnly = true;
            PHistory_Hours.Width = 93;
            // 
            // PHistory_Raids
            // 
            PHistory_Raids.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            PHistory_Raids.DataPropertyName = "Raids";
            PHistory_Raids.HeaderText = "Raids";
            PHistory_Raids.MaxInputLength = 6;
            PHistory_Raids.Name = "PHistory_Raids";
            PHistory_Raids.ReadOnly = true;
            PHistory_Raids.Width = 60;
            // 
            // PHistory_SR
            // 
            PHistory_SR.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            PHistory_SR.DataPropertyName = "SR";
            PHistory_SR.HeaderText = "S/R %";
            PHistory_SR.Name = "PHistory_SR";
            PHistory_SR.ReadOnly = true;
            PHistory_SR.Width = 63;
            // 
            // PHistory_Group
            // 
            PHistory_Group.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            PHistory_Group.DataPropertyName = "Group";
            PHistory_Group.HeaderText = "Group";
            PHistory_Group.MaxInputLength = 16;
            PHistory_Group.Name = "PHistory_Group";
            PHistory_Group.ReadOnly = true;
            PHistory_Group.SortMode = DataGridViewColumnSortMode.NotSortable;
            PHistory_Group.Width = 46;
            // 
            // PHistory_Alerts
            // 
            PHistory_Alerts.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            PHistory_Alerts.DataPropertyName = "Alerts";
            PHistory_Alerts.HeaderText = "Alerts";
            PHistory_Alerts.MaxInputLength = 512;
            PHistory_Alerts.Name = "PHistory_Alerts";
            PHistory_Alerts.ReadOnly = true;
            PHistory_Alerts.SortMode = DataGridViewColumnSortMode.NotSortable;
            PHistory_Alerts.Width = 43;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(dataGridView_Watchlist);
            tabPage5.Location = new Point(4, 24);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(1256, 653);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "Player Watchlist";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // dataGridView_Watchlist
            // 
            dataGridView_Watchlist.AllowUserToResizeColumns = false;
            dataGridView_Watchlist.AllowUserToResizeRows = false;
            dataGridView_Watchlist.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_Watchlist.Columns.AddRange(new DataGridViewColumn[] { Column_PWAcctID, Column_PWReason, Column_PWTimestamp });
            dataGridView_Watchlist.Dock = DockStyle.Fill;
            dataGridView_Watchlist.Location = new Point(3, 3);
            dataGridView_Watchlist.MultiSelect = false;
            dataGridView_Watchlist.Name = "dataGridView_Watchlist";
            dataGridView_Watchlist.Size = new Size(1250, 647);
            dataGridView_Watchlist.TabIndex = 0;
            // 
            // Column_PWAcctID
            // 
            Column_PWAcctID.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Column_PWAcctID.DataPropertyName = "AcctID";
            Column_PWAcctID.HeaderText = "Account ID";
            Column_PWAcctID.MaxInputLength = 12;
            Column_PWAcctID.Name = "Column_PWAcctID";
            Column_PWAcctID.ToolTipText = "Player's Account ID (as obtained from Player History)";
            Column_PWAcctID.Width = 91;
            // 
            // Column_PWReason
            // 
            Column_PWReason.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Column_PWReason.DataPropertyName = "Reason";
            Column_PWReason.HeaderText = "Reason";
            Column_PWReason.MaxInputLength = 1000;
            Column_PWReason.Name = "Column_PWReason";
            Column_PWReason.ToolTipText = "Reason for entering this player into the Watchlist (ex: Cheater, streamer name)";
            Column_PWReason.Width = 70;
            // 
            // Column_PWTimestamp
            // 
            Column_PWTimestamp.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Column_PWTimestamp.DataPropertyName = "Timestamp";
            Column_PWTimestamp.HeaderText = "Timestamp";
            Column_PWTimestamp.MaxInputLength = 1000;
            Column_PWTimestamp.Name = "Column_PWTimestamp";
            Column_PWTimestamp.ReadOnly = true;
            Column_PWTimestamp.ToolTipText = "Date/Time that this entry was added.";
            Column_PWTimestamp.Width = 92;
            // 
            // tabPage6
            // 
            tabPage6.Controls.Add(tableLayoutPanel_ImportantLoot);
            tabPage6.Location = new Point(4, 24);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new Padding(3);
            tabPage6.Size = new Size(1256, 653);
            tabPage6.TabIndex = 5;
            tabPage6.Text = "Loot Filters";
            tabPage6.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel_ImportantLoot
            // 
            tableLayoutPanel_ImportantLoot.AutoScroll = true;
            tableLayoutPanel_ImportantLoot.ColumnCount = 1;
            tableLayoutPanel_ImportantLoot.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel_ImportantLoot.Controls.Add(flowLayoutPanel2, 0, 0);
            tableLayoutPanel_ImportantLoot.Controls.Add(dataGridView_Loot, 0, 1);
            tableLayoutPanel_ImportantLoot.Dock = DockStyle.Fill;
            tableLayoutPanel_ImportantLoot.Location = new Point(3, 3);
            tableLayoutPanel_ImportantLoot.Name = "tableLayoutPanel_ImportantLoot";
            tableLayoutPanel_ImportantLoot.RowCount = 2;
            tableLayoutPanel_ImportantLoot.RowStyles.Add(new RowStyle());
            tableLayoutPanel_ImportantLoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel_ImportantLoot.Size = new Size(1250, 647);
            tableLayoutPanel_ImportantLoot.TabIndex = 1;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.AutoSize = true;
            flowLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel2.Controls.Add(checkBox_CurrentLootFilter_Enabled);
            flowLayoutPanel2.Controls.Add(comboBox_LootFilters);
            flowLayoutPanel2.Controls.Add(button_LootFilters_Add);
            flowLayoutPanel2.Controls.Add(button_LootFilters_Rename);
            flowLayoutPanel2.Controls.Add(button_LootFilters_Delete);
            flowLayoutPanel2.Controls.Add(comboBox_LootFiltersItem_Items);
            flowLayoutPanel2.Controls.Add(textBox_LootFiltersItem_Search);
            flowLayoutPanel2.Controls.Add(button_LootFiltersItem_Add);
            flowLayoutPanel2.Location = new Point(3, 3);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(822, 60);
            flowLayoutPanel2.TabIndex = 2;
            // 
            // checkBox_CurrentLootFilter_Enabled
            // 
            checkBox_CurrentLootFilter_Enabled.Anchor = AnchorStyles.Right;
            checkBox_CurrentLootFilter_Enabled.AutoSize = true;
            checkBox_CurrentLootFilter_Enabled.Location = new Point(3, 6);
            checkBox_CurrentLootFilter_Enabled.Name = "checkBox_CurrentLootFilter_Enabled";
            checkBox_CurrentLootFilter_Enabled.RightToLeft = RightToLeft.Yes;
            checkBox_CurrentLootFilter_Enabled.Size = new Size(68, 19);
            checkBox_CurrentLootFilter_Enabled.TabIndex = 8;
            checkBox_CurrentLootFilter_Enabled.Text = "Enabled";
            checkBox_CurrentLootFilter_Enabled.UseVisualStyleBackColor = true;
            checkBox_CurrentLootFilter_Enabled.CheckedChanged += checkBox_CurrentLootFilter_Enabled_CheckedChanged;
            // 
            // comboBox_LootFilters
            // 
            comboBox_LootFilters.Anchor = AnchorStyles.Right;
            comboBox_LootFilters.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_LootFilters.FormattingEnabled = true;
            comboBox_LootFilters.Location = new Point(77, 4);
            comboBox_LootFilters.Name = "comboBox_LootFilters";
            comboBox_LootFilters.Size = new Size(229, 23);
            comboBox_LootFilters.TabIndex = 4;
            comboBox_LootFilters.SelectedIndexChanged += comboBox_LootFilters_SelectedIndexChanged;
            // 
            // button_LootFilters_Add
            // 
            button_LootFilters_Add.Anchor = AnchorStyles.Right;
            button_LootFilters_Add.Location = new Point(312, 3);
            button_LootFilters_Add.Name = "button_LootFilters_Add";
            button_LootFilters_Add.Size = new Size(75, 24);
            button_LootFilters_Add.TabIndex = 5;
            button_LootFilters_Add.Text = "Add";
            button_LootFilters_Add.UseVisualStyleBackColor = true;
            button_LootFilters_Add.Click += button_LootFilters_Add_Click;
            // 
            // button_LootFilters_Rename
            // 
            button_LootFilters_Rename.Anchor = AnchorStyles.Right;
            button_LootFilters_Rename.Location = new Point(393, 3);
            button_LootFilters_Rename.Name = "button_LootFilters_Rename";
            button_LootFilters_Rename.Size = new Size(75, 24);
            button_LootFilters_Rename.TabIndex = 7;
            button_LootFilters_Rename.Text = "Rename";
            button_LootFilters_Rename.UseVisualStyleBackColor = true;
            button_LootFilters_Rename.Click += button_LootFilters_Rename_Click;
            // 
            // button_LootFilters_Delete
            // 
            button_LootFilters_Delete.Anchor = AnchorStyles.Right;
            flowLayoutPanel2.SetFlowBreak(button_LootFilters_Delete, true);
            button_LootFilters_Delete.Location = new Point(474, 3);
            button_LootFilters_Delete.Name = "button_LootFilters_Delete";
            button_LootFilters_Delete.Size = new Size(75, 25);
            button_LootFilters_Delete.TabIndex = 6;
            button_LootFilters_Delete.Text = "Delete";
            button_LootFilters_Delete.UseVisualStyleBackColor = true;
            button_LootFilters_Delete.Click += button_LootFilters_Delete_Click;
            // 
            // comboBox_LootFiltersItem_Items
            // 
            comboBox_LootFiltersItem_Items.Anchor = AnchorStyles.Right;
            comboBox_LootFiltersItem_Items.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_LootFiltersItem_Items.FormattingEnabled = true;
            comboBox_LootFiltersItem_Items.Location = new Point(3, 34);
            comboBox_LootFiltersItem_Items.Name = "comboBox_LootFiltersItem_Items";
            comboBox_LootFiltersItem_Items.Size = new Size(467, 23);
            comboBox_LootFiltersItem_Items.TabIndex = 1;
            // 
            // textBox_LootFiltersItem_Search
            // 
            textBox_LootFiltersItem_Search.Anchor = AnchorStyles.Right;
            textBox_LootFiltersItem_Search.Location = new Point(476, 34);
            textBox_LootFiltersItem_Search.MaxLength = 256;
            textBox_LootFiltersItem_Search.Name = "textBox_LootFiltersItem_Search";
            textBox_LootFiltersItem_Search.PlaceholderText = "Search by item name...";
            textBox_LootFiltersItem_Search.Size = new Size(250, 23);
            textBox_LootFiltersItem_Search.TabIndex = 2;
            textBox_LootFiltersItem_Search.TextChanged += textBox_ImpLoot_Search_TextChanged;
            // 
            // button_LootFiltersItem_Add
            // 
            button_LootFiltersItem_Add.Anchor = AnchorStyles.Right;
            button_LootFiltersItem_Add.Location = new Point(732, 34);
            button_LootFiltersItem_Add.Name = "button_LootFiltersItem_Add";
            button_LootFiltersItem_Add.Size = new Size(87, 23);
            button_LootFiltersItem_Add.TabIndex = 3;
            button_LootFiltersItem_Add.Text = "Add";
            button_LootFiltersItem_Add.UseVisualStyleBackColor = true;
            button_LootFiltersItem_Add.Click += button_ImpLoot_Add_Click;
            // 
            // dataGridView_Loot
            // 
            dataGridView_Loot.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_Loot.Columns.AddRange(new DataGridViewColumn[] { Column_LootEnabled, Column_LootItemID, Column_LootName, Column_LootType, Column_LootComment, Column_ItemColor, Column_LootColorPicker });
            dataGridView_Loot.Dock = DockStyle.Fill;
            dataGridView_Loot.Location = new Point(3, 69);
            dataGridView_Loot.MultiSelect = false;
            dataGridView_Loot.Name = "dataGridView_Loot";
            dataGridView_Loot.Size = new Size(1316, 575);
            dataGridView_Loot.TabIndex = 0;
            // 
            // Column_LootEnabled
            // 
            Column_LootEnabled.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Column_LootEnabled.DataPropertyName = "Enabled";
            Column_LootEnabled.HeaderText = "Enabled";
            Column_LootEnabled.Name = "Column_LootEnabled";
            Column_LootEnabled.SortMode = DataGridViewColumnSortMode.Automatic;
            Column_LootEnabled.Width = 74;
            // 
            // Column_LootItemID
            // 
            Column_LootItemID.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Column_LootItemID.DataPropertyName = "ItemID";
            Column_LootItemID.HeaderText = "Item ID";
            Column_LootItemID.MaxInputLength = 256;
            Column_LootItemID.Name = "Column_LootItemID";
            Column_LootItemID.ToolTipText = "Item's BSG ID (can be obtained from market.json)";
            Column_LootItemID.Width = 70;
            // 
            // Column_LootName
            // 
            Column_LootName.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Column_LootName.DataPropertyName = "Name";
            Column_LootName.HeaderText = "Item Name";
            Column_LootName.MaxInputLength = 1000;
            Column_LootName.Name = "Column_LootName";
            Column_LootName.ReadOnly = true;
            Column_LootName.Width = 91;
            // 
            // Column_LootType
            // 
            Column_LootType.DataPropertyName = "Type";
            Column_LootType.HeaderText = "Type";
            Column_LootType.Name = "Column_LootType";
            Column_LootType.Resizable = DataGridViewTriState.True;
            Column_LootType.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // Column_LootComment
            // 
            Column_LootComment.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Column_LootComment.DataPropertyName = "Comment";
            Column_LootComment.HeaderText = "Comment";
            Column_LootComment.MaxInputLength = 1000;
            Column_LootComment.Name = "Column_LootComment";
            Column_LootComment.ToolTipText = "Entry comment (name of item,etc.)";
            Column_LootComment.Width = 86;
            // 
            // Column_ItemColor
            // 
            Column_ItemColor.DataPropertyName = "Color";
            Column_ItemColor.HeaderText = "Color";
            Column_ItemColor.MaxInputLength = 12;
            Column_ItemColor.Name = "Column_ItemColor";
            // 
            // Column_LootColorPicker
            // 
            Column_LootColorPicker.HeaderText = "Pick Color";
            Column_LootColorPicker.Name = "Column_LootColorPicker";
            Column_LootColorPicker.Text = "Pick Color";
            Column_LootColorPicker.UseColumnTextForButtonValue = true;
            // 
            // toolTip1
            // 
            toolTip1.AutoPopDelay = 20000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 100;
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
            Text = "EFT DMA Radar";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            flowLayoutPanel_Loot.ResumeLayout(false);
            flowLayoutPanel_Loot.PerformLayout();
            groupBox_MapSetup.ResumeLayout(false);
            groupBox_MapSetup.PerformLayout();
            tabPage2.ResumeLayout(false);
            flowLayoutPanel_Settings.ResumeLayout(false);
            flowLayoutPanel_Settings.PerformLayout();
            flowLayoutPanel_RadarSettings.ResumeLayout(false);
            flowLayoutPanel_RadarSettings.PerformLayout();
            ((ISupportInitialize)trackBar_AimlineLength).EndInit();
            ((ISupportInitialize)trackBar_MaxDist).EndInit();
            ((ISupportInitialize)trackBar_UIScale).EndInit();
            ((ISupportInitialize)trackBar_ContainerDist).EndInit();
            flowLayoutPanel5.ResumeLayout(false);
            flowLayoutPanel5.PerformLayout();
            flowLayoutPanel_Loot_Containers.ResumeLayout(false);
            flowLayoutPanel_Loot_Containers.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            flowLayoutPanel_WebRadarSettings.ResumeLayout(false);
            flowLayoutPanel_WebRadarSettings.PerformLayout();
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
            flowLayoutPanel_AdvancedChams.ResumeLayout(false);
            flowLayoutPanel_AdvancedChams.PerformLayout();
            flowLayoutPanel_LTW.ResumeLayout(false);
            flowLayoutPanel_LTW.PerformLayout();
            ((ISupportInitialize)trackBar_LTWAmount).EndInit();
            flowLayoutPanel_WideLean.ResumeLayout(false);
            flowLayoutPanel_WideLean.PerformLayout();
            ((ISupportInitialize)trackBar_WideLeanAmt).EndInit();
            flowLayoutPanel_MonitorSettings.ResumeLayout(false);
            flowLayoutPanel_MonitorSettings.PerformLayout();
            flowLayoutPanel_ESPSettings.ResumeLayout(false);
            flowLayoutPanel_ESPSettings.PerformLayout();
            flowLayoutPanel_ESP_PlayerRender.ResumeLayout(false);
            flowLayoutPanel_ESP_PlayerRender.PerformLayout();
            flowLayoutPanel_ESP_AIRender.ResumeLayout(false);
            flowLayoutPanel_ESP_AIRender.PerformLayout();
            flowLayoutPanel4.ResumeLayout(false);
            flowLayoutPanel4.PerformLayout();
            ((ISupportInitialize)trackBar_EspLootDist).EndInit();
            ((ISupportInitialize)trackBar_EspImpLootDist).EndInit();
            ((ISupportInitialize)trackBar_ESPContainerDist).EndInit();
            ((ISupportInitialize)trackBar_EspQuestHelperDist).EndInit();
            ((ISupportInitialize)trackBar_EspGrenadeDist).EndInit();
            ((ISupportInitialize)trackBar_EspFontScale).EndInit();
            ((ISupportInitialize)trackBar_EspLineScale).EndInit();
            tabPage3.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            ((ISupportInitialize)dataGridView_PlayerHistory).EndInit();
            tabPage5.ResumeLayout(false);
            ((ISupportInitialize)dataGridView_Watchlist).EndInit();
            tabPage6.ResumeLayout(false);
            tableLayoutPanel_ImportantLoot.ResumeLayout(false);
            tableLayoutPanel_ImportantLoot.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            ((ISupportInitialize)dataGridView_Loot).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Label label_AimlineLength;
        private TrackBar trackBar_AimlineLength;
        private Label label_Pos;
        private Label label1;
        private CheckBox checkBox_Loot;
        private CheckBox checkBox_MapSetup;
        private Button button_Restart;
        private GroupBox groupBox_MapSetup;
        private Button button_MapSetupApply;
        private TextBox textBox_mapScale;
        private Label label5;
        private TextBox textBox_mapY;
        private Label label4;
        private TextBox textBox_mapX;
        private CheckBox checkBox_Aimview;
        private CheckBox checkBox_MapFree;
        private TabPage tabPage3;
        private RichTextBox richTextBox_PlayersInfo;
        private TabPage tabPage4;
        private CheckBox checkBox_HideNames;
        private TextBox textBox_LootFilterByName;
        private Label label8;
        private Label label7;
        private TextBox textBox_LootImpValue;
        private TextBox textBox_LootRegValue;
        private Label label6;
        private Button button_Loot;
        private Label label_UIScale;
        private TrackBar trackBar_UIScale;
        private CheckBox checkBox_HideCorpses;
        private CheckBox checkBox_ShowFood;
        private CheckBox checkBox_ShowMeds;
        private CheckBox checkBox_ShowInfoTab;
        private CheckBox checkBox_GrpConnect;
        private CheckBox checkBox_NoRecoilSway;
        private CheckBox checkBox_InfStamina;
        private CheckBox checkBox_NoVisor;
        private CheckBox checkBox_AimBotEnabled;
        private Button button_HotkeyManager;
        private Label label10;
        private ComboBox comboBox_AimbotTarget;
        private TabPage tabPage5;
        private TabPage tabPage6;
        private DataGridView dataGridView_Watchlist;
        private DataGridView dataGridView_Loot;
        private TableLayoutPanel tableLayoutPanel_ImportantLoot;
        private Button button_LootFiltersItem_Add;
        private TextBox textBox_LootFiltersItem_Search;
        private ComboBox comboBox_LootFiltersItem_Items;
        private TrackBar trackBar_MaxDist;
        private Label label_MaxDist;
        private DataGridView dataGridView_PlayerHistory;
        private DataGridViewTextBoxColumn Column_PWAcctID;
        private DataGridViewTextBoxColumn Column_PWReason;
        private DataGridViewTextBoxColumn Column_PWTimestamp;
        private CheckBox checkBox_QuestHelper_Enabled;
        private CheckBox checkBox_Chams;
        private RadioButton radioButton_AimTarget_CQB;
        private RadioButton radioButton_AimTarget_FOV;
        private Button button_StartESP;
        private CheckBox checkBox_ESP_Explosives;
        private CheckBox checkBox_ESP_Exfils;
        private CheckBox checkBox_ESP_FPS;
        private TrackBar trackBar_EspLootDist;
        private TrackBar trackBar_EspFontScale;
        private Label label_EspFontScale;
        private TrackBar trackBar_EspGrenadeDist;
        private Label label_EspExplosivesDist;
        private Label label_EspLootDist;
        private Label label_ESPFPSCap;
        private TextBox textBox_EspFpsCap;
        private Label label_AimFOV;
        private TrackBar trackBar_AimFOV;
        private CheckBox checkBox_ESP_AimFov;
        private Button button_EspColorPicker;
        private CheckBox checkBox_ESP_Dist;
        private CheckBox checkBox_ESP_AimLock;
        private CheckBox checkBox_LootPPS;
        private TrackBar trackBar_EspImpLootDist;
        private Label label_EspImpLootDist;
        private TextBox textBox_ResHeight;
        private TextBox textBox_ResWidth;
        private Button button_DetectRes;
        private Label label_Height;
        private Label label_Width;
        private CheckBox checkBox_AlwaysDaySunny;
        private TrackBar trackBar_NoSway;
        private TrackBar trackBar_NoRecoil;
        private Label label_Sway;
        private Label label_Recoil;
        private ComboBox comboBox_ESPAutoFS;
        private CheckBox checkBox_ESP_AutoFS;
        private CheckBox checkBox_ESP_Loot;
        private CheckBox checkBox_ESP_ShowMines;
        private CheckBox checkBox_ShowMines;
        private CheckBox checkBox_ESP_ShowMag;
        private CheckBox checkBox_AimbotDisableReLock;
        private Button button_BackupConfig;
        private ComboBox comboBox_ESP_HighAlert;
        private Label label_ESP_HighAlert;
        private Button button_Radar_ColorPicker;
        private ComboBox comboBox_LootFilters;
        private Button button_LootFilters_Add;
        private Button button_LootFilters_Delete;
        private ColorDialog colorPicker1;
        private DataGridViewCheckBoxColumn Column_LootEnabled;
        private DataGridViewTextBoxColumn Column_LootItemID;
        private DataGridViewTextBoxColumn Column_LootName;
        private DataGridViewComboBoxColumn Column_LootType;
        private DataGridViewTextBoxColumn Column_LootComment;
        private DataGridViewTextBoxColumn Column_ItemColor;
        private DataGridViewButtonColumn Column_LootColorPicker;
        private RadioButton radioButton_ESPRender_Box;
        private RadioButton radioButton_ESPRender_None;
        private RadioButton radioButton_ESPRender_Bones;
        private RadioButton radioButton_ESPAIRender_Presence;
        private RadioButton radioButton_ESPAIRender_Box;
        private RadioButton radioButton_ESPAIRender_None;
        private RadioButton radioButton_ESPAIRender_Bones;
        private RadioButton radioButton_ESPRender_Presence;
        private CheckBox checkBox_ESPRender_Dist;
        private CheckBox checkBox_ESPRender_Weapons;
        private CheckBox checkBox_ESPRender_Labels;
        private CheckBox checkBox_ESPAIRender_Dist;
        private CheckBox checkBox_ESPAIRender_Weapons;
        private CheckBox checkBox_ESPAIRender_Labels;
        private CheckBox checkBox_ESP_RaidStats;
        private RadioButton radioButton_Chams_Vischeck;
        private RadioButton radioButton_Chams_Basic;
        private ToolTip toolTip1;
        private CheckBox checkBox_CurrentLootFilter_Enabled;
        private Button button_LootFilters_Rename;
        private CheckBox checkBox_TeammateAimlines;
        private CheckBox checkBox_AimHeadAI;
        private CheckBox checkBox_SA_AutoBone;
        private CheckBox checkBox_SA_SafeLock;
        private Button button_RandomBoneCfg;
        private CheckBox checkBox_AimRandomBone;
        private CheckBox checkBox_EnableMemWrite;
        private CheckBox checkBox_RageMode;
        private Button button_AntiAfk;
        private CheckBox checkBox_NoWepMalf;
        private SKGLControl skglControl_Radar;
        private DataGridViewTextBoxColumn PHistory_Name;
        private DataGridViewTextBoxColumn PHistory_AccountID;
        private DataGridViewTextBoxColumn PHistory_Acct;
        private DataGridViewTextBoxColumn PHistory_Type;
        private DataGridViewTextBoxColumn PHistory_KD;
        private DataGridViewTextBoxColumn PHistory_Hours;
        private DataGridViewTextBoxColumn PHistory_Raids;
        private DataGridViewTextBoxColumn PHistory_SR;
        private DataGridViewTextBoxColumn PHistory_Group;
        private DataGridViewTextBoxColumn PHistory_Alerts;
        private FlowLayoutPanel flowLayoutPanel_Settings;
        private FlowLayoutPanel flowLayoutPanel_RadarSettings;
        private FlowLayoutPanel flowLayoutPanel_MemWrites;
        private FlowLayoutPanel flowLayoutPanel_MonitorSettings;
        private FlowLayoutPanel flowLayoutPanel_ESPSettings;
        private Label label2;
        private Label label3;
        private Label label11;
        private Label label12;
        private FlowLayoutPanel flowLayoutPanel_Aimbot;
        private Label label13;
        private FlowLayoutPanel flowLayoutPanel_NoRecoil;
        private Label label16;
        private FlowLayoutPanel flowLayoutPanel_Chams;
        private Label label17;
        private FlowLayoutPanel flowLayoutPanel4;
        private FlowLayoutPanel flowLayoutPanel_ESP_PlayerRender;
        private FlowLayoutPanel flowLayoutPanel_ESP_AIRender;
        private Label label18;
        private Label label19;
        private FlowLayoutPanel flowLayoutPanel_Loot;
        private Label label20;
        private FlowLayoutPanel flowLayoutPanel2;
        private FlowLayoutPanel flowLayoutPanel_WebRadarSettings;
        private Label label9;
        private Label label_WebRadarHost;
        private TextBox textBox_WebRadarBindIP;
        private Label label_WebRadarPort;
        private TextBox textBox_WebRadarPort;
        private Label label_WebRadarPassword;
        private TextBox textBox_WebRadarPassword;
        private LinkLabel linkLabel_WebRadarLink;
        private Button button_WebRadarStart;
        private CheckBox checkBox_WebRadarUPNP;
        private TextBox textBox_WebRadarTickRate;
        private Label label_WebRadarTickRate;
        private Label label_EspQuestHelperDist;
        private TrackBar trackBar_EspQuestHelperDist;
        private Label label21;
        private ComboBox comboBox_WideLeanMode;
        private FlowLayoutPanel flowLayoutPanel_WideLean;
        private Label label_WideLean;
        private Label label_WideLeanAmt;
        private TrackBar trackBar_WideLeanAmt;
        private CheckBox checkBox_WideLean;
        private Label label22;
        private Label label23;
        private Label label24;
        private Label label25;
        private Label label26;
        private CheckBox checkBox_MoveSpeed;
        private CheckBox checkBox_ESP_FireportAim;
        private Label label27;
        private Label label_EspLineScale;
        private TrackBar trackBar_EspLineScale;
        private CheckBox checkBox_ShowBackpacks;
        private CheckBox checkBox_ESP_StatusText;
        private CheckBox checkBox_LTW;
        private FlowLayoutPanel flowLayoutPanel_LTW;
        private Label label28;
        private Label label_LTWAmount;
        private TrackBar trackBar_LTWAmount;
        private FlowLayoutPanel flowLayoutPanel5;
        private Label label29;
        private CheckBox checkBox_ShowContainers;
        private Label label_ContainerDist;
        private TrackBar trackBar_ContainerDist;
        private Label label_ESPContainerDist;
        private TrackBar trackBar_ESPContainerDist;
        private FlowLayoutPanel flowLayoutPanel_Loot_Containers;
        private Label label30;
        private CheckBox checkBox_Containers_SelectAll;
        private Label label31;
        private CheckedListBox checkedListBox_Containers;
        private CheckBox checkBox_Containers_HideSearched;
        private Button button_GymHack;
        private CheckBox checkBox_FullBright;
        private CheckBox checkBox_AdvancedMemWrites;
        private CheckBox checkBox_FastWeaponOps;
        private CheckBox checkBox_FastLoadUnload;
        private FlowLayoutPanel flowLayoutPanel_AdvancedChams;
        private Label label14;
        private Label label15;
        private TextBox textBox_VischeckVisColor;
        private Button button_VischeckVisColorPick;
        private Label label33;
        private TextBox textBox_VischeckInvisColor;
        private Button button_VischeckInvisColorPick;
        private RadioButton radioButton_Chams_Visible;
        private FlowLayoutPanel flowLayoutPanel_MemWriteCheckbox;
        private RadioButton radioButton_Loot_FleaPrice;
        private RadioButton radioButton_Loot_VendorPrice;
        private CheckBox checkBox_AntiPage;
        private CheckBox checkBox_AIAimlines;
        private CheckBox checkBox_LootWishlist;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label34;
        private CheckedListBox checkedListBox_QuestHelper;
        private LinkLabel linkLabel_CheckForUpdates;
    }
}

