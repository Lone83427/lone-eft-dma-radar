using DarkModeForms;

namespace arena_dma_radar.UI.ColorPicker
{
    public sealed partial class ColorPicker<TEnum, TClass> : Form
        where TEnum : Enum
        where TClass : ColorItem<TEnum>
    {
        private readonly Dictionary<TEnum, string> _colors;
        private readonly DarkModeCS _darkmode;
        private TEnum _selected;

        /// <summary>
        /// Form Result.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<TEnum, string> Result { get; private set; }

        public ColorPicker(string name, IDictionary<TEnum, string> existing)
        {
            _colors = new(existing);
            InitializeComponent();
            _darkmode = new DarkModeCS(this);
            PopulateOptions();
            if (comboBox_Colors.Items.Count > 0)
                comboBox_Colors.SelectedIndex = 0;
            this.Text = name;
        }

        /// <summary>
        /// Populate the ESP Color Options list.
        /// </summary>
        private void PopulateOptions()
        {

            var enumType = typeof(TEnum);

            foreach (var field in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attributes = field.GetCustomAttributes(inherit: false);
                if (attributes?.Any(x => x is ObsoleteAttribute) ?? false)
                    continue;
                var value = (TEnum)field.GetValue(null)!;
                comboBox_Colors.Items.Add(ColorItem<TEnum>.CreateInstance(value));
            }
        }

        private void comboBox_Colors_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selected = ((TClass)comboBox_Colors.SelectedItem)!.Option;
            textBox_ColorValue.Text = _colors[_selected];
        }

        private void textBox_ColorValue_TextChanged(object sender, EventArgs e)
        {
            var input = textBox_ColorValue.Text.Trim();
            _colors[_selected] = input;
        }

        private void button_Edit_Click(object sender, EventArgs e)
        {
            var dlg = colorDialog1.ShowDialog();
            if (dlg is DialogResult.OK)
            {
                var color = colorDialog1.Color.ToSKColor();
                textBox_ColorValue.Text = color.ToString();
            }
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            foreach (var color in _colors)
                if (!SKColor.TryParse(color.Value, out var skColor))
                    throw new Exception($"Invalid Color Value for {color.Key}!");
            this.Result = _colors;
            this.DialogResult = DialogResult.OK;
        }
    }
}