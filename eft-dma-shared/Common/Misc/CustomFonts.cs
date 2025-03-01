using SkiaSharp;
using System.Reflection;

namespace eft_dma_shared.Common.Misc
{
    public static class CustomFonts
    {
        /// <summary>
        /// Neo Sans Std Regular
        /// </summary>
        public static SKTypeface SKFontFamilyRegular { get; }
        /// <summary>
        /// Neo Sans Std Bold
        /// </summary>
        public static SKTypeface SKFontFamilyBold { get; }
        /// <summary>
        /// Neo Sans Std Italic
        /// </summary>
        public static SKTypeface SKFontFamilyItalic { get; }
        /// <summary>
        /// Neo Sans Std Medium
        /// </summary>
        public static SKTypeface SKFontFamilyMedium { get; }

        static CustomFonts()
        {
            try
            {
                byte[] fontFamilyRegular, fontFamilyBold, fontFamilyItalic, fontFamilyMedium;
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("eft-dma-shared.NeoSansStdRegular.otf"))
                {
                    fontFamilyRegular = new byte[stream!.Length];
                    stream.ReadExactly(fontFamilyRegular);
                }
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("eft-dma-shared.NeoSansStdBold.otf"))
                {
                    fontFamilyBold = new byte[stream!.Length];
                    stream.ReadExactly(fontFamilyBold);
                }
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("eft-dma-shared.NeoSansStdItalic.otf"))
                {
                    fontFamilyItalic = new byte[stream!.Length];
                    stream.ReadExactly(fontFamilyItalic);
                }
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("eft-dma-shared.NeoSansStdMedium.otf"))
                {
                    fontFamilyMedium = new byte[stream!.Length];
                    stream.ReadExactly(fontFamilyMedium);
                }
                SKFontFamilyRegular = SKTypeface.FromStream(new MemoryStream(fontFamilyRegular, false));
                SKFontFamilyBold = SKTypeface.FromStream(new MemoryStream(fontFamilyBold, false));
                SKFontFamilyItalic = SKTypeface.FromStream(new MemoryStream(fontFamilyItalic, false));
                SKFontFamilyMedium = SKTypeface.FromStream(new MemoryStream(fontFamilyMedium, false));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("ERROR Loading Custom Fonts!", ex);
            }
        }
    }
}
