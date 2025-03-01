namespace arena_dma_radar.Features.MemoryWrites.UI
{
    sealed partial class AimbotRandomBoneForm
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(AimbotRandomBoneForm));
            groupBox_RandomBone = new GroupBox();
            button_OK = new Button();
            textBox_Legs = new TextBox();
            textBox_Arms = new TextBox();
            textBox_Torso = new TextBox();
            textBox_Head = new TextBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            groupBox_RandomBone.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox_RandomBone
            // 
            groupBox_RandomBone.Controls.Add(button_OK);
            groupBox_RandomBone.Controls.Add(textBox_Legs);
            groupBox_RandomBone.Controls.Add(textBox_Arms);
            groupBox_RandomBone.Controls.Add(textBox_Torso);
            groupBox_RandomBone.Controls.Add(textBox_Head);
            groupBox_RandomBone.Controls.Add(label4);
            groupBox_RandomBone.Controls.Add(label3);
            groupBox_RandomBone.Controls.Add(label2);
            groupBox_RandomBone.Controls.Add(label1);
            groupBox_RandomBone.Dock = DockStyle.Fill;
            groupBox_RandomBone.Location = new Point(0, 0);
            groupBox_RandomBone.Name = "groupBox_RandomBone";
            groupBox_RandomBone.Size = new Size(313, 192);
            groupBox_RandomBone.TabIndex = 0;
            groupBox_RandomBone.TabStop = false;
            groupBox_RandomBone.Text = "Body Zone Percentages";
            // 
            // button_OK
            // 
            button_OK.Location = new Point(116, 146);
            button_OK.Name = "button_OK";
            button_OK.Size = new Size(81, 34);
            button_OK.TabIndex = 8;
            button_OK.Text = "OK";
            button_OK.UseVisualStyleBackColor = true;
            button_OK.Click += button_OK_Click;
            // 
            // textBox_Legs
            // 
            textBox_Legs.Location = new Point(47, 115);
            textBox_Legs.MaxLength = 3;
            textBox_Legs.Name = "textBox_Legs";
            textBox_Legs.Size = new Size(64, 23);
            textBox_Legs.TabIndex = 7;
            textBox_Legs.Text = "26";
            textBox_Legs.TextChanged += textBox_Legs_TextChanged;
            // 
            // textBox_Arms
            // 
            textBox_Arms.Location = new Point(47, 86);
            textBox_Arms.MaxLength = 3;
            textBox_Arms.Name = "textBox_Arms";
            textBox_Arms.Size = new Size(64, 23);
            textBox_Arms.TabIndex = 6;
            textBox_Arms.Text = "26";
            textBox_Arms.TextChanged += textBox_Arms_TextChanged;
            // 
            // textBox_Torso
            // 
            textBox_Torso.Location = new Point(47, 55);
            textBox_Torso.MaxLength = 3;
            textBox_Torso.Name = "textBox_Torso";
            textBox_Torso.Size = new Size(64, 23);
            textBox_Torso.TabIndex = 5;
            textBox_Torso.Text = "38";
            textBox_Torso.TextChanged += textBox_Torso_TextChanged;
            // 
            // textBox_Head
            // 
            textBox_Head.Location = new Point(47, 26);
            textBox_Head.MaxLength = 3;
            textBox_Head.Name = "textBox_Head";
            textBox_Head.Size = new Size(64, 23);
            textBox_Head.TabIndex = 4;
            textBox_Head.Text = "10";
            textBox_Head.TextChanged += textBox_Head_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(10, 118);
            label4.Name = "label4";
            label4.Size = new Size(31, 15);
            label4.TabIndex = 3;
            label4.Text = "Legs";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 89);
            label3.Name = "label3";
            label3.Size = new Size(35, 15);
            label3.TabIndex = 2;
            label3.Text = "Arms";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 58);
            label2.Name = "label2";
            label2.Size = new Size(35, 15);
            label2.TabIndex = 1;
            label2.Text = "Torso";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 29);
            label1.Name = "label1";
            label1.Size = new Size(35, 15);
            label1.TabIndex = 0;
            label1.Text = "Head";
            // 
            // AimbotRandomBoneForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(313, 192);
            Controls.Add(groupBox_RandomBone);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AimbotRandomBoneForm";
            Text = "Random Bone";
            TopMost = true;
            groupBox_RandomBone.ResumeLayout(false);
            groupBox_RandomBone.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox_RandomBone;
        private TextBox textBox_Torso;
        private TextBox textBox_Head;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox textBox_Legs;
        private TextBox textBox_Arms;
        private Button button_OK;
    }
}