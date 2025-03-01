using DarkModeForms;

namespace eft_dma_radar.UI.Misc
{
    public partial class InputBox : Form
    {
        private readonly DarkModeCS _darkmode;
        /// <summary>
        /// The text that was entered into the Input Box.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Result { get; private set; }

        public InputBox(string title, string prompt)
        {
            InitializeComponent();
            _darkmode = new DarkModeCS(this);
            this.Text = title;
            label_Prompt.Text = prompt;
            this.AcceptButton = button_OK;
            this.CenterToScreen();
            textBox_Input.Select();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            Result = textBox_Input.Text;
            this.DialogResult = DialogResult.OK;
        }
    }
}
