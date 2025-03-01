namespace arena_dma_radar.UI.ColorPicker
{
    /// <summary>
    /// Defines a Color Menu Item in Color Picker.
    /// </summary>
    /// <typeparam name="TEnum">Backing enum type.</typeparam>
    public class ColorItem<TEnum>
        where TEnum : Enum
    {
        public TEnum Option { get; }

        private ColorItem(TEnum option)
        {
            Option = option;
        }

        public override string ToString()
        {
            return Option.ToString();
        }

        public static ColorItem<TEnum> CreateInstance(TEnum option)
        {
            return new ColorItem<TEnum>(option);
        }
    }
}
