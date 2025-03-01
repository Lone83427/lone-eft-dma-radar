namespace arena_dma_radar.UI.ColorPicker
{
    sealed partial class ColorPicker<TEnum, TClass>
        where TEnum : Enum
        where TClass : ColorItem<TEnum>
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
            colorDialog1 = new ColorDialog();
            comboBox_Colors = new ComboBox();
            textBox_ColorValue = new TextBox();
            button_Edit = new Button();
            button_Save = new Button();
            SuspendLayout();
            // 
            // comboBox_Colors
            // 
            comboBox_Colors.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_Colors.FormattingEnabled = true;
            comboBox_Colors.Location = new Point(12, 26);
            comboBox_Colors.Name = "comboBox_Colors";
            comboBox_Colors.Size = new Size(170, 23);
            comboBox_Colors.TabIndex = 0;
            comboBox_Colors.SelectedIndexChanged += comboBox_Colors_SelectedIndexChanged;
            // 
            // textBox_ColorValue
            // 
            textBox_ColorValue.Location = new Point(200, 26);
            textBox_ColorValue.MaxLength = 12;
            textBox_ColorValue.Name = "textBox_ColorValue";
            textBox_ColorValue.Size = new Size(100, 23);
            textBox_ColorValue.TabIndex = 1;
            textBox_ColorValue.TextChanged += textBox_ColorValue_TextChanged;
            // 
            // button_Edit
            // 
            button_Edit.Location = new Point(315, 26);
            button_Edit.Name = "button_Edit";
            button_Edit.Size = new Size(47, 23);
            button_Edit.TabIndex = 2;
            button_Edit.Text = "Edit";
            button_Edit.UseVisualStyleBackColor = true;
            button_Edit.Click += button_Edit_Click;
            // 
            // button_Save
            // 
            button_Save.Location = new Point(133, 74);
            button_Save.Name = "button_Save";
            button_Save.Size = new Size(102, 44);
            button_Save.TabIndex = 3;
            button_Save.Text = "Save All";
            button_Save.UseVisualStyleBackColor = true;
            button_Save.Click += button_Save_Click;
            // 
            // ColorPicker
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(382, 140);
            Controls.Add(button_Save);
            Controls.Add(button_Edit);
            Controls.Add(textBox_ColorValue);
            Controls.Add(comboBox_Colors);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ColorPicker";
            Text = "Color Picker";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ColorDialog colorDialog1;
        private ComboBox comboBox_Colors;
        private TextBox textBox_ColorValue;
        private Button button_Edit;
        private Button button_Save;
    }
}