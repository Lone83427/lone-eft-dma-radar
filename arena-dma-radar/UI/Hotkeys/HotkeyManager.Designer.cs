namespace arena_dma_radar.UI.Hotkeys
{
    partial class HotkeyManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(HotkeyManager));
            groupBox_Main = new GroupBox();
            button_Save = new Button();
            button_Remove = new Button();
            button_Add = new Button();
            label2 = new Label();
            label1 = new Label();
            comboBox_Hotkeys = new ComboBox();
            comboBox_Actions = new ComboBox();
            listBox_Values = new ListBox();
            groupBox_Main.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox_Main
            // 
            groupBox_Main.Controls.Add(button_Save);
            groupBox_Main.Controls.Add(button_Remove);
            groupBox_Main.Controls.Add(button_Add);
            groupBox_Main.Controls.Add(label2);
            groupBox_Main.Controls.Add(label1);
            groupBox_Main.Controls.Add(comboBox_Hotkeys);
            groupBox_Main.Controls.Add(comboBox_Actions);
            groupBox_Main.Controls.Add(listBox_Values);
            groupBox_Main.Dock = DockStyle.Fill;
            groupBox_Main.Location = new Point(0, 0);
            groupBox_Main.Name = "groupBox_Main";
            groupBox_Main.Size = new Size(534, 313);
            groupBox_Main.TabIndex = 0;
            groupBox_Main.TabStop = false;
            groupBox_Main.Text = "Set Hotkeys In-Game";
            // 
            // button_Save
            // 
            button_Save.Location = new Point(444, 251);
            button_Save.Name = "button_Save";
            button_Save.Size = new Size(78, 49);
            button_Save.TabIndex = 7;
            button_Save.Text = "Save";
            button_Save.UseVisualStyleBackColor = true;
            button_Save.Click += button_Save_Click;
            // 
            // button_Remove
            // 
            button_Remove.Location = new Point(444, 69);
            button_Remove.Name = "button_Remove";
            button_Remove.Size = new Size(75, 23);
            button_Remove.TabIndex = 6;
            button_Remove.Text = "Remove";
            button_Remove.UseVisualStyleBackColor = true;
            button_Remove.Click += button_Remove_Click;
            // 
            // button_Add
            // 
            button_Add.Location = new Point(444, 40);
            button_Add.Name = "button_Add";
            button_Add.Size = new Size(75, 23);
            button_Add.TabIndex = 5;
            button_Add.Text = "Add";
            button_Add.UseVisualStyleBackColor = true;
            button_Add.Click += button_Add_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 19);
            label2.Name = "label2";
            label2.Size = new Size(47, 15);
            label2.TabIndex = 4;
            label2.Text = "Actions";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(257, 19);
            label1.Name = "label1";
            label1.Size = new Size(50, 15);
            label1.TabIndex = 3;
            label1.Text = "Hotkeys";
            // 
            // comboBox_Hotkeys
            // 
            comboBox_Hotkeys.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_Hotkeys.FormattingEnabled = true;
            comboBox_Hotkeys.Location = new Point(257, 40);
            comboBox_Hotkeys.Name = "comboBox_Hotkeys";
            comboBox_Hotkeys.Size = new Size(169, 23);
            comboBox_Hotkeys.TabIndex = 2;
            comboBox_Hotkeys.SelectedIndexChanged += comboBox_Hotkeys_SelectedIndexChanged;
            // 
            // comboBox_Actions
            // 
            comboBox_Actions.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_Actions.FormattingEnabled = true;
            comboBox_Actions.Location = new Point(12, 40);
            comboBox_Actions.Name = "comboBox_Actions";
            comboBox_Actions.Size = new Size(218, 23);
            comboBox_Actions.TabIndex = 1;
            comboBox_Actions.SelectedIndexChanged += comboBox_Actions_SelectedIndexChanged;
            // 
            // listBox_Values
            // 
            listBox_Values.FormattingEnabled = true;
            listBox_Values.ItemHeight = 15;
            listBox_Values.Location = new Point(12, 86);
            listBox_Values.Name = "listBox_Values";
            listBox_Values.Size = new Size(414, 214);
            listBox_Values.TabIndex = 0;
            listBox_Values.SelectedIndexChanged += listBox_Values_SelectedIndexChanged;
            // 
            // HotkeyManagerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(534, 313);
            Controls.Add(groupBox_Main);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "HotkeyManagerForm";
            Text = "Hotkey Manager";
            TopMost = true;
            groupBox_Main.ResumeLayout(false);
            groupBox_Main.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox_Main;
        private ListBox listBox_Values;
        private Button button_Remove;
        private Button button_Add;
        private Label label2;
        private Label label1;
        private ComboBox comboBox_Hotkeys;
        private ComboBox comboBox_Actions;
        private Button button_Save;
    }
}