using arena_dma_radar.Arena.Features.MemoryWrites;
using arena_dma_radar.UI.Misc;

namespace arena_dma_radar.Features.MemoryWrites.UI
{
    public sealed partial class AimbotRandomBoneForm : Form
    {
        private static AimbotRandomBoneConfig Config { get; } = Aimbot.Config.RandomBone;

        public AimbotRandomBoneForm()
        {
            InitializeComponent();
            if (!Config.Is100Percent)
                Config.ResetDefaults();
            textBox_Head.Text = Config.HeadPercent.ToString();
            textBox_Torso.Text = Config.TorsoPercent.ToString();
            textBox_Arms.Text = Config.ArmsPercent.ToString();
            textBox_Legs.Text = Config.LegsPercent.ToString();
        }

        private void HeadDefault() => textBox_Head.Text = "10";
        private void TorsoDefault() => textBox_Torso.Text = "38";
        private void ArmsDefault() => textBox_Arms.Text = "26";
        private void LegsDefault() => textBox_Legs.Text = "26";

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (!Config.Is100Percent)
            {
                MessageBox.Show(this, "Values do not add up to exactly 100%! Please correct your values.", Program.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.DialogResult = DialogResult.OK;
        }
        private void textBox_Head_TextChanged(object sender, EventArgs e)
        {
            int value;
            while (!int.TryParse(textBox_Head.Text, out value) || value < 0)
                HeadDefault();
            Config.HeadPercent = value;
        }

        private void textBox_Torso_TextChanged(object sender, EventArgs e)
        {
            int value;
            while (!int.TryParse(textBox_Torso.Text, out value) || value < 0)
                TorsoDefault();
            Config.TorsoPercent = value;
        }

        private void textBox_Arms_TextChanged(object sender, EventArgs e)
        {
            int value;
            while (!int.TryParse(textBox_Arms.Text, out value) || value < 0)
                ArmsDefault();
            Config.ArmsPercent = value;
        }

        private void textBox_Legs_TextChanged(object sender, EventArgs e)
        {
            int value;
            while (!int.TryParse(textBox_Legs.Text, out value) || value < 0)
                LegsDefault();
            Config.LegsPercent = value;
        }
    }
}
