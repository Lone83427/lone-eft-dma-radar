using eft_dma_shared.Common.Misc;

namespace eft_dma_radar.UI.Misc
{
    internal static class SKPaints
    {
        #region Radar Paints

        public static SKPaint PaintConnectorGroup { get; } = new()
        {
            Color = SKColors.LawnGreen.WithAlpha(60),
            StrokeWidth = 2.25f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintMouseoverGroup { get; } = new()
        {
            Color = SKColors.LawnGreen,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextMouseoverGroup { get; } = new()
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

        public static SKPaint PaintLocalPlayer { get; } = new()
        {
            Color = SKColors.Green,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextLocalPlayer { get; } = new()
        {
            SubpixelText = true,
            Color = SKColors.Green,
            IsStroke = false,
            TextSize = 12,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintTeammate { get; } = new()
        {
            Color = SKColors.LimeGreen,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextTeammate { get; } = new()
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

        public static SKPaint PaintPMC { get; } = new()
        {
            Color = SKColors.Red,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextPMC { get; } = new()
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

        public static SKPaint PaintWatchlist { get; } = new()
        {
            Color = SKColors.HotPink,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextWatchlist { get; } = new()
        {
            SubpixelText = true,
            Color = SKColors.HotPink,
            IsStroke = false,
            TextSize = 12,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintStreamer { get; } = new()
        {
            Color = SKColors.MediumPurple,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextStreamer { get; } = new()
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

        public static SKPaint PaintAimbotLocked { get; } = new()
        {
            Color = SKColors.Blue,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextAimbotLocked { get; } = new()
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

        public static SKPaint PaintScav { get; } = new()
        {
            Color = SKColors.Yellow,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextScav { get; } = new()
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

        public static SKPaint PaintRaider { get; } = new()
        {
            Color = SKColor.Parse("ffc70f"),
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextRaider { get; } = new()
        {
            SubpixelText = true,
            Color = SKColor.Parse("ffc70f"),
            IsStroke = false,
            TextSize = 12,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintBoss { get; } = new()
        {
            Color = SKColors.Fuchsia,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextBoss { get; } = new()
        {
            SubpixelText = true,
            Color = SKColors.Fuchsia,
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

        public static SKPaint PaintPScav { get; } = new()
        {
            Color = SKColors.White,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextPScav { get; } = new() // Player Scav Text , Tooltip Text
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

        public static SKPaint TextMouseover { get; } = new() // Tooltip Text
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

        public static SKPaint PaintDeathMarker { get; } = new()
        {
            Color = SKColors.Black,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        #endregion

        #region Loot Paints
        public static SKPaint PaintLoot { get; } = new()
        {
            Color = SKColors.WhiteSmoke,
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintImportantLoot { get; } = new()
        {
            Color = SKColors.Turquoise,
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintContainerLoot { get; } = new()
        {
            Color = SKColor.Parse("FFFFCC"),
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextLoot { get; } = new()
        {
            SubpixelText = true,
            Color = SKColors.WhiteSmoke,
            IsStroke = false,
            TextSize = 12,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextImportantLoot { get; } = new()
        {
            SubpixelText = true,
            Color = SKColors.Turquoise,
            IsStroke = false,
            TextSize = 12,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintCorpse { get; } = new()
        {
            Color = SKColors.Silver,
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextCorpse { get; } = new()
        {
            SubpixelText = true,
            Color = SKColors.Silver,
            IsStroke = false,
            TextSize = 12,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintMeds { get; } = new()
        {
            Color = SKColors.LightSalmon,
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextMeds { get; } = new()
        {
            SubpixelText = true,
            Color = SKColors.LightSalmon,
            IsStroke = false,
            TextSize = 12,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintFood { get; } = new()
        {
            Color = SKColors.CornflowerBlue,
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextFood { get; } = new()
        {
            SubpixelText = true,
            Color = SKColors.CornflowerBlue,
            IsStroke = false,
            TextSize = 12,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintBackpacks { get; } = new()
        {
            Color = SKColor.Parse("00b02c"),
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextBackpacks { get; } = new()
        {
            SubpixelText = true,
            Color = SKColor.Parse("00b02c"),
            IsStroke = false,
            TextSize = 12,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint QuestHelperPaint { get; } = new()
        {
            Color = SKColors.DeepPink,
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint QuestHelperText { get; } = new()
        {
            SubpixelText = true,
            Color = SKColors.DeepPink,
            IsStroke = false,
            TextSize = 12,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintQuestItem { get; } = new()
        {
            Color = SKColors.YellowGreen,
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextQuestItem { get; } = new()
        {
            SubpixelText = true,
            Color = SKColors.YellowGreen,
            IsStroke = false,
            TextSize = 12,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintWishlistItem { get; } = new()
        {
            Color = SKColors.Red,
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextWishlistItem { get; } = new()
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

        #endregion

        #region Render/Misc Paints

        public static SKPaint PaintTransparentBacker { get; } = new()
        {
            Color = SKColors.Black.WithAlpha(0xBE), // Transparent backer
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill
        };

        public static SKPaint TextRadarStatus { get; } = new()
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

        public static SKPaint PaintExplosives { get; } = new()
        {
            Color = SKColors.OrangeRed,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintExfilOpen { get; } = new()
        {
            Color = SKColors.LimeGreen,
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintExfilTransit { get; } = new()
        {
            Color = SKColors.Orange,
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintExfilPending { get; } = new()
        {
            Color = SKColors.Yellow,
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintExfilClosed { get; } = new()
        {
            Color = SKColors.Red,
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintExfilInactive { get; } = new()
        {
            Color = SKColors.Gray,
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextOutline { get; } = new()
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
        public static SKPaint ShapeOutline { get; } = new()
        {
            Color = SKColors.Black,
            /*StrokeWidth = ??,*/ // Compute before use
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        #endregion

        #region ESP Paints

        public static SKPaint PaintPMCESP { get; } = new()
        {
            StrokeWidth = 1.5f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextPMCESP { get; } = new()
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

        public static SKPaint PaintScavESP { get; } = new()
        {
            StrokeWidth = 1.5f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextScavESP { get; } = new()
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

        public static SKPaint PaintRaiderESP { get; } = new()
        {
            StrokeWidth = 1.5f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextRaiderESP { get; } = new()
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

        public static SKPaint PaintBossESP { get; } = new()
        {
            StrokeWidth = 1.5f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextBossESP { get; } = new()
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

        public static SKPaint PaintAimbotLockedESP { get; } = new()
        {
            StrokeWidth = 1.5f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextAimbotLockedESP { get; } = new()
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

        public static SKPaint PaintStreamerESP { get; } = new()
        {
            StrokeWidth = 1.5f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextStreamerESP { get; } = new()
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

        public static SKPaint PaintWatchlistESP { get; } = new()
        {
            StrokeWidth = 1.5f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextWatchlistESP { get; } = new()
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

        public static SKPaint PaintPlayerScavESP { get; } = new()
        {
            StrokeWidth = 1.5f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextPlayerScavESP { get; } = new()
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

        public static SKPaint PaintFriendlyESP { get; } = new()
        {
            StrokeWidth = 1.5f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextFriendlyESP { get; } = new()
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

        public static SKPaint PaintLootESP { get; } = new()
        {
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextLootESP { get; } = new()
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
        public static SKPaint PaintCorpseESP { get; } = new()
        {
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextCorpseESP { get; } = new()
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

        public static SKPaint PaintImpLootESP { get; } = new()
        {
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextImpLootESP { get; } = new()
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

        public static SKPaint PaintContainerLootESP { get; } = new()
        {
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextContainerLootESP { get; } = new()
        {
            SubpixelText = true,
            IsStroke = false,
            TextSize = 11f,
            TextAlign = SKTextAlign.Center,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyMedium,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintMedsESP { get; } = new()
        {
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextMedsESP { get; } = new()
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

        public static SKPaint PaintFoodESP { get; } = new()
        {
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextFoodESP { get; } = new()
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

        public static SKPaint PaintBackpackESP { get; } = new()
        {
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextBackpackESP { get; } = new()
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

        public static SKPaint PaintQuestItemESP { get; } = new()
        {
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextQuestItemESP { get; } = new()
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
        public static SKPaint PaintWishlistItemESP { get; } = new()
        {
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextWishlistItemESP { get; } = new()
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
        public static SKPaint PaintQuestHelperESP { get; } = new()
        {
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };
        public static SKPaint TextQuestHelperESP { get; } = new()
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

        public static SKPaint PaintGrenadeESP { get; } = new()
        {
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextExfilESP { get; } = new()
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

        public static SKPaint PaintCrosshairESP { get; } = new()
        {
            Color = SKColors.Red,
            StrokeWidth = 1.75f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintHighAlertAimlineESP { get; } = new()
        {
            Color = SKColors.Red,
            StrokeWidth = 1f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintHighAlertBorderESP { get; } = new()
        {
            Color = SKColors.Red,
            StrokeWidth = 3f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextMagazineESP { get; } = new()
        {
            SubpixelText = true,
            Color = SKColors.White,
            IsStroke = false,
            TextSize = 42f,
            TextAlign = SKTextAlign.Left,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyBold,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextMagazineInfoESP { get; } = new()
        {
            SubpixelText = true,
            Color = SKColors.White,
            IsStroke = false,
            TextSize = 16f,
            TextAlign = SKTextAlign.Left,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyItalic,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint PaintBasicESP { get; } = new()
        {
            Color = SKColors.White,
            StrokeWidth = 1f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        public static SKPaint TextBasicESP { get; } = new()
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

        public static SKPaint TextBasicESPLeftAligned { get; } = new()
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

        public static SKPaint TextBasicESPRightAligned { get; } = new()
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