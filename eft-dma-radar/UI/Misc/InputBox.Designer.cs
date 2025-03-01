namespace eft_dma_radar.UI.Misc
{
    partial class InputBox
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(InputBox));
            button_OK = new Button();
            label_Prompt = new Label();
            textBox_Input = new TextBox();
            SuspendLayout();
            // 
            // button_OK
            // 
            button_OK.Location = new Point(482, 39);
            button_OK.Name = "button_OK";
            button_OK.Size = new Size(75, 23);
            button_OK.TabIndex = 0;
            button_OK.Text = "OK";
            button_OK.UseVisualStyleBackColor = true;
            button_OK.Click += button_OK_Click;
            // 
            // label_Prompt
            // 
            label_Prompt.AutoSize = true;
            label_Prompt.Location = new Point(12, 21);
            label_Prompt.Name = "label_Prompt";
            label_Prompt.Size = new Size(47, 15);
            label_Prompt.TabIndex = 1;
            label_Prompt.Text = "Prompt";
            // 
            // textBox_Input
            // 
            textBox_Input.Location = new Point(12, 39);
            textBox_Input.MaxLength = 512;
            textBox_Input.Name = "textBox_Input";
            textBox_Input.Size = new Size(450, 23);
            textBox_Input.TabIndex = 2;
            // 
            // InputBox
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(607, 107);
            Controls.Add(textBox_Input);
            Controls.Add(label_Prompt);
            Controls.Add(button_OK);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "InputBox";
            Text = "Input Box";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_OK;
        private Label label_Prompt;
        private TextBox textBox_Input;
    }
}