using eft_dma_shared.Common.Misc;

namespace arena_dma_radar.UI.Misc
{
    internal static class SKPaints
    {
        public static SKPaint PaintTransparentBacker { get; } = new SKPaint()
        {
            Color = SKColors.Black.WithAlpha(0xBE), // Transparent backer
            StrokeWidth = 0.1f,
            Style = SKPaintStyle.Fill,
        };
        public static SKPaint PaintMouseoverGroup { get; } = new SKPaint()
        {
            Color = SKColors.LawnGreen,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextMouseoverGroup { get; } = new SKPaint()
        {
            SubpixelText = true,
            Color = SKColors.LawnGreen,
            IsStroke = false,
            TextSize = 12,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextMouseover { get; } = new SKPaint() // Tooltip Text
        {
            SubpixelText = true,
            Color = SKColors.White,
            IsStroke = false,
            TextSize = 12,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextRadarStatus { get; } = new SKPaint()
        {
            SubpixelText = true,
            Color = SKColors.Red,
            IsStroke = false,
            TextSize = 48,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            TextAlign = SKTextAlign.Left
        };
        public static SKPaint PaintConnectorGroup { get; } = new SKPaint()
        {
            Color = SKColors.LawnGreen.WithAlpha(60),
            StrokeWidth = 2.25f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint PaintExplosives { get; } = new SKPaint()
        {
            Color = SKColors.OrangeRed,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint PaintLocalPlayer { get; } = new SKPaint()
        {
            Color = SKColors.Green,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint PaintTeammate { get; } = new SKPaint()
        {
            Color = SKColors.LimeGreen,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextTeammate { get; } = new SKPaint()
        {
            SubpixelText = true,
            Color = SKColors.LimeGreen,
            IsStroke = false,
            TextSize = 12,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint PaintAI { get; } = new SKPaint()
        {
            Color = SKColors.Yellow,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextAI { get; } = new SKPaint()
        {
            SubpixelText = true,
            Color = SKColors.Yellow,
            IsStroke = false,
            TextSize = 12,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint PaintPlayer { get; } = new SKPaint()
        {
            Color = SKColors.Red,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextPlayer { get; } = new SKPaint()
        {
            SubpixelText = true,
            Color = SKColors.Red,
            IsStroke = false,
            TextSize = 12,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint PaintStreamer { get; } = new SKPaint()
        {
            Color = SKColors.MediumPurple,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextStreamer { get; } = new SKPaint()
        {
            SubpixelText = true,
            Color = SKColors.MediumPurple,
            IsStroke = false,
            TextSize = 12,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint PaintAimbotLocked { get; } = new SKPaint()
        {
            Color = SKColors.Blue,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextAimbotLocked { get; } = new SKPaint()
        {
            SubpixelText = true,
            Color = SKColors.Blue,
            IsStroke = false,
            TextSize = 12,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint PaintFocused { get; } = new()
        {
            Color = SKColors.Coral,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextFocused { get; } = new()
        {
            SubpixelText = true,
            Color = SKColors.Coral,
            IsStroke = false,
            TextSize = 12,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint PaintDeathMarker { get; } = new SKPaint()
        {
            Color = SKColors.Black,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextOutline { get; } = new SKPaint()
        {
            SubpixelText = true,
            IsAntialias = true,
            Color = SKColors.Black,
            TextSize = 12f,
            IsStroke = true,
            StrokeWidth = 2f,
            Style = SKPaintStyle.Stroke,
            Typeface = CustomFonts.SKFontFamilyRegular
        };
        /// <summary>
        /// Only utilize this paint on the Radar UI Thread. StrokeWidth is modified prior to each draw call.
        /// *NOT* Thread safe to use!
        /// </summary>
        public static SKPaint ShapeOutline { get; } = new SKPaint()
        {
            Color = SKColors.Black,
            /*StrokeWidth = ??,*/ // Compute before use
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextStatusSmall { get; } = new SKPaint()
        {
            SubpixelText = true,
            Color = SKColors.Red,
            IsStroke = false,
            TextSize = 13,
            TextAlign = SKTextAlign.Center,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyMedium,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextStatusSmallEsp { get; } = new SKPaint()
        {
            SubpixelText = true,
            Color = SKColors.Red,
            IsStroke = false,
            TextSize = 13,
            TextAlign = SKTextAlign.Center,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyMedium,
            FilterQuality = SKFilterQuality.High
        };

        #region ESP Paints
        public static SKPaint PaintPlayerESP { get; } = new SKPaint()
        {
            StrokeWidth = 1.5f,
            Style = SKPaintStyle.StrokeAndFill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextPlayerESP { get; } = new SKPaint()
        {
            SubpixelText = true,
            IsStroke = false,
            TextSize = 12f,
            TextAlign = SKTextAlign.Center,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyMedium,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint PaintAIESP { get; } = new SKPaint()
        {
            StrokeWidth = 1.5f,
            Style = SKPaintStyle.StrokeAndFill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextAIESP { get; } = new SKPaint()
        {
            SubpixelText = true,
            IsStroke = false,
            TextSize = 12f,
            TextAlign = SKTextAlign.Center,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyMedium,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint PaintStreamerESP { get; } = new SKPaint()
        {
            StrokeWidth = 1.5f,
            Style = SKPaintStyle.StrokeAndFill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextStreamerESP { get; } = new SKPaint()
        {
            SubpixelText = true,
            IsStroke = false,
            TextSize = 12f,
            TextAlign = SKTextAlign.Center,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyMedium,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint PaintTeammateESP { get; } = new SKPaint()
        {
            StrokeWidth = 1.5f,
            Style = SKPaintStyle.StrokeAndFill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextTeammateESP { get; } = new SKPaint()
        {
            SubpixelText = true,
            IsStroke = false,
            TextSize = 12f,
            TextAlign = SKTextAlign.Center,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyMedium,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint PaintGrenadeESP { get; } = new SKPaint()
        {
            StrokeWidth = 1f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint PaintAimbotLockedESP { get; } = new SKPaint()
        {
            StrokeWidth = 1.5f,
            Style = SKPaintStyle.StrokeAndFill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextAimbotLockedESP { get; } = new SKPaint()
        {
            SubpixelText = true,
            IsStroke = false,
            TextSize = 12f,
            TextAlign = SKTextAlign.Center,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyMedium,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint PaintFocusedESP { get; } = new()
        {
            StrokeWidth = 1.5f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextFocusedESP { get; } = new()
        {
            SubpixelText = true,
            IsStroke = false,
            TextSize = 12f,
            TextAlign = SKTextAlign.Center,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyMedium,
            FilterQuality = SKFilterQuality.High
        };

        #region ESP Readonly Paints
        public static SKPaint PaintCrosshairESP { get; } = new SKPaint()
        {
            Color = SKColors.Red,
            StrokeWidth = 1.75f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint PaintHighAlertAimlineESP { get; } = new SKPaint()
        {
            Color = SKColors.Red,
            StrokeWidth = 1f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint PaintHighAlertBorderESP { get; } = new SKPaint()
        {
            Color = SKColors.Red,
            StrokeWidth = 3f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextMagazineESP { get; } = new SKPaint()
        {
            Color = SKColors.White,
            SubpixelText = true,
            IsStroke = false,
            TextSize = 42f,
            TextAlign = SKTextAlign.Left,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyBold,
            FilterQuality = SKFilterQuality.High,
        };
        public static SKPaint TextMagazineInfoESP { get; } = new SKPaint()
        {
            Color = SKColors.White,
            SubpixelText = true,
            IsStroke = false,
            TextSize = 16f,
            TextAlign = SKTextAlign.Left,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyItalic,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint PaintBasicESP { get; } = new SKPaint()
        {
            Color = SKColors.White,
            StrokeWidth = 1f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextBasicESP { get; } = new SKPaint()
        {
            SubpixelText = true,
            Color = SKColors.White,
            IsStroke = false,
            TextSize = 12f,
            TextAlign = SKTextAlign.Center,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyMedium,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextBasicESPLeftAligned { get; } = new SKPaint()
        {
            SubpixelText = true,
            Color = SKColors.White,
            IsStroke = false,
            TextSize = 12f,
            TextAlign = SKTextAlign.Left,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyMedium,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextBasicESPRightAligned { get; } = new SKPaint()
        {
            SubpixelText = true,
            Color = SKColors.White,
            IsStroke = false,
            TextSize = 12f,
            TextAlign = SKTextAlign.Right,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyMedium,
            FilterQuality = SKFilterQuality.High
        };
        #endregion

        #endregion
    }
}
