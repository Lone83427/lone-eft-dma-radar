using eft_dma_radar.Tarkov.EFTPlayer;
using eft_dma_radar.Tarkov.Loot;
using eft_dma_radar.UI.Radar;
using eft_dma_radar.UI.SKWidgetControl;
using eft_dma_shared.Common.ESP;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Players;
using eft_dma_shared.Common.Unity;

namespace eft_dma_radar.UI.ESP
{
    public sealed class EspWidget : SKWidget
    {
        private readonly MainForm _parentForm;
        private SKBitmap _espBitmap;
        private SKCanvas _espCanvas;

        public EspWidget(SKGLControl parent, MainForm parentForm, SKRect location, bool minimized, float scale)
            : base(parent, "ESP", new SKPoint(location.Left, location.Top), new SKSize(location.Width, location.Height),
                scale)
        {
            _parentForm = parentForm;
            _espBitmap = new SKBitmap((int)location.Width, (int)location.Height, SKImageInfo.PlatformColorType,
                SKAlphaType.Premul);
            _espCanvas = new SKCanvas(_espBitmap);
            Minimized = minimized;
        }

        /// <summary>
        /// LocalPlayer (who is running Radar) 'Player' object.
        /// Returns the player the Current Window belongs to.
        /// </summary>
        private static LocalPlayer LocalPlayer =>
            Memory.LocalPlayer;

        /// <summary>
        /// All Players in Local Game World (including dead/exfil'd) 'Player' collection.
        /// </summary>
        private static IReadOnlyCollection<Player> AllPlayers => Memory.Players;

        /// <summary>
        /// Radar has found Local Game World, and a Raid Instance is active.
        /// </summary>
        private static bool InRaid => Memory.InRaid;

        /// <summary>
        /// Contains all filtered loot in Local Game World.
        /// </summary>
        private static IEnumerable<LootItem> Loot => Memory.Loot?.FilteredLoot;

        /// <summary>
        /// All Static Containers on the map.
        /// </summary>
        private static IEnumerable<StaticLootContainer> Containers => Memory.Loot?.StaticLootContainers;

        public override void Draw(SKCanvas canvas)
        {
            base.Draw(canvas);
            if (!Minimized)
                RenderESPWidget(canvas, ClientRectangle);
        }

        /// <summary>
        /// Perform Aimview (Mini-ESP) Rendering.
        /// </summary>
        private void RenderESPWidget(SKCanvas parent, SKRect dest)
        {
            var size = Size;
            if (_espBitmap is null || _espCanvas is null ||
                _espBitmap.Width != size.Width || _espBitmap.Height != size.Height)
            {
                _espCanvas?.Dispose();
                _espCanvas = null;
                _espBitmap?.Dispose();
                _espBitmap = null;
                _espBitmap = new SKBitmap((int)size.Width, (int)size.Height, SKImageInfo.PlatformColorType,
                    SKAlphaType.Premul);
                _espCanvas = new SKCanvas(_espBitmap);
            }

            _espCanvas.Clear(SKColors.Transparent);
            try
            {
                var inRaid = InRaid; // cache bool
                var localPlayer = LocalPlayer; // cache ref to current player
                if (inRaid && localPlayer is not null)
                {
                    if (MainForm.Config.ShowLoot)
                    {
                        float boxHalf = 4f * ScaleFactor;
                        var loot = Loot;
                        if (loot is not null)
                        {
                            foreach (var item in loot)
                            {
                                var dist = Vector3.Distance(localPlayer.Position, item.Position);
                                if (dist >= 10f)
                                    continue;
                                if (!CameraManagerBase.WorldToScreen(ref item.Position, out var itemScrPos))
                                    continue;
                                var adjPos = ScaleESPPoint(itemScrPos);
                                var boxPt = new SKRect(adjPos.X - boxHalf, adjPos.Y + boxHalf,
                                    adjPos.X + boxHalf, adjPos.Y - boxHalf);
                                var textPt = new SKPoint(adjPos.X,
                                    adjPos.Y + 12.5f * ScaleFactor);
                                _espCanvas.DrawRect(boxPt, PaintESPWidgetLoot);
                                var label = item.GetUILabel(true) + $" ({dist.ToString("n1")}m)";
                                _espCanvas.DrawText(label, textPt, TextESPWidgetLoot);
                            }
                        }
                        if (MainForm.Config.Containers.Show)
                        {
                            var containers = Containers;
                            if (containers is not null)
                            {
                                foreach (var container in containers)
                                {
                                    if (MainForm.ContainerIsTracked(container.ID ?? "NULL"))
                                    {
                                        if (MainForm.Config.Containers.HideSearched && container.Searched)
                                        {
                                            continue;
                                        }
                                        var dist = Vector3.Distance(localPlayer.Position, container.Position);
                                        if (dist >= 10f)
                                            continue;
                                        if (!CameraManagerBase.WorldToScreen(ref container.Position, out var containerScrPos))
                                            continue;
                                        var adjPos = ScaleESPPoint(containerScrPos);
                                        var boxPt = new SKRect(adjPos.X - boxHalf, adjPos.Y + boxHalf,
                                            adjPos.X + boxHalf, adjPos.Y - boxHalf);
                                        var textPt = new SKPoint(adjPos.X,
                                            adjPos.Y + 12.5f * ScaleFactor);
                                        _espCanvas.DrawRect(boxPt, PaintESPWidgetLoot);
                                        var label = $"{container.Name} ({dist.ToString("n1")}m)";
                                        _espCanvas.DrawText(label, textPt, TextESPWidgetLoot);
                                    }
                                }
                            }
                        }
                    }

                    var allPlayers = AllPlayers?
                        .Where(x => x.IsActive && x.IsAlive &&
                                    x is not Tarkov.EFTPlayer.LocalPlayer);
                    if (allPlayers is not null)
                    {
                        var scaleX = _espBitmap.Width / (float)CameraManagerBase.Viewport.Width;
                        var scaleY = _espBitmap.Height / (float)CameraManagerBase.Viewport.Height;
                        foreach (var player in allPlayers)
                            if (player.Skeleton.UpdateESPWidgetBuffer(scaleX, scaleY))
                                _espCanvas.DrawPoints(SKPointMode.Lines, Skeleton.ESPWidgetBuffer, player.GetESPWidgetPaint());
                    }

                    var bounds = _espBitmap.Info.Rect;
                    /// Draw Crosshair
                    float centerX = bounds.Left + bounds.Width / 2;
                    float centerY = bounds.Top + bounds.Height / 2;

                    _espCanvas.DrawLine(bounds.Left, centerY, bounds.Right, centerY, PaintESPWidgetCrosshair);
                    _espCanvas.DrawLine(centerX, bounds.Top, centerX, bounds.Bottom, PaintESPWidgetCrosshair);
                }
            }
            catch (Exception ex) // Log rendering errors
            {
                var error = $"CRITICAL ESP WIDGET RENDER ERROR: {ex}";
                LoneLogging.WriteLine(error);
            }

            _espCanvas.Flush();
            parent.DrawBitmap(_espBitmap, dest, SharedPaints.PaintBitmap);
        }

        /// <summary>
        /// Scales larger Screen Coordinates to smaller Aimview Coordinates.
        /// </summary>
        /// <param name="original">Original W2S Screen Coords.</param>
        /// <returns>Adjusted Aimview Screen Coords.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private SKPoint ScaleESPPoint(SKPoint original)
        {
            var scaleX = _espBitmap.Width / (float)CameraManagerBase.Viewport.Width;
            var scaleY = _espBitmap.Height / (float)CameraManagerBase.Viewport.Height;

            var newX = original.X * scaleX;
            var newY = original.Y * scaleY;

            return new SKPoint(newX, newY);
        }

        public override void SetScaleFactor(float newScale)
        {
            base.SetScaleFactor(newScale);
            PaintESPWidgetCrosshair.StrokeWidth = 1 * newScale;
            PaintESPWidgetLocalPlayer.StrokeWidth = 1 * newScale;
            PaintESPWidgetPMC.StrokeWidth = 1 * newScale;
            PaintESPWidgetWatchlist.StrokeWidth = 1 * newScale;
            PaintESPWidgetStreamer.StrokeWidth = 1 * newScale;
            PaintESPWidgetTeammate.StrokeWidth = 1 * newScale;
            PaintESPWidgetBoss.StrokeWidth = 1 * newScale;
            PaintESPWidgetAimbotLocked.StrokeWidth = 1 * newScale;
            PaintESPWidgetScav.StrokeWidth = 1 * newScale;
            PaintESPWidgetRaider.StrokeWidth = 1 * newScale;
            PaintESPWidgetPScav.StrokeWidth = 1 * newScale;
            PaintESPWidgetFocused.StrokeWidth = 1 * newScale;
            PaintESPWidgetLoot.StrokeWidth = 0.75f * newScale;
            TextESPWidgetLoot.TextSize = 9f * newScale;
        }

        public override void Dispose()
        {
            _espBitmap?.Dispose();
            _espCanvas?.Dispose();
            base.Dispose();
        }

        #region Mini ESP Paints

        private static SKPaint PaintESPWidgetCrosshair { get; } = new()
        {
            Color = SKColors.White,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true
        };

        internal static SKPaint PaintESPWidgetLocalPlayer { get; } = new()
        {
            Color = SKColors.Green,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke
        };

        internal static SKPaint PaintESPWidgetPMC { get; } = new()
        {
            Color = SKColors.Red,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke
        };

        internal static SKPaint PaintESPWidgetAimbotLocked { get; } = new()
        {
            Color = SKColors.Blue,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke
        };

        internal static SKPaint PaintESPWidgetWatchlist { get; } = new()
        {
            Color = SKColors.HotPink,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke
        };

        internal static SKPaint PaintESPWidgetStreamer { get; } = new()
        {
            Color = SKColors.MediumPurple,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke
        };

        internal static SKPaint PaintESPWidgetTeammate { get; } = new()
        {
            Color = SKColors.LimeGreen,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke
        };

        internal static SKPaint PaintESPWidgetBoss { get; } = new()
        {
            Color = SKColors.Fuchsia,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke
        };

        internal static SKPaint PaintESPWidgetScav { get; } = new()
        {
            Color = SKColors.Yellow,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke
        };

        internal static SKPaint PaintESPWidgetRaider { get; } = new()
        {
            Color = SKColor.Parse("ffc70f"),
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke
        };

        internal static SKPaint PaintESPWidgetPScav { get; } = new()
        {
            Color = SKColors.White,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke
        };

        internal static SKPaint PaintESPWidgetFocused { get; } = new()
        {
            Color = SKColors.Coral,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke
        };

        internal static SKPaint PaintESPWidgetLoot { get; } = new()
        {
            Color = SKColors.WhiteSmoke,
            StrokeWidth = 0.75f,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High
        };

        internal static SKPaint TextESPWidgetLoot { get; } = new()
        {
            SubpixelText = true,
            Color = SKColors.WhiteSmoke,
            IsStroke = false,
            TextSize = 9f,
            TextAlign = SKTextAlign.Center,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = CustomFonts.SKFontFamilyRegular,
            FilterQuality = SKFilterQuality.High
        };

        #endregion
    }

    public static class ESPWidgetExtensions
    {
        /// <summary>
        /// Gets Aimview drawing paintbrush based on Player Type.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SKPaint GetESPWidgetPaint(this Player player)
        {
            if (player.IsAimbotLocked)
                return EspWidget.PaintESPWidgetAimbotLocked;
            if (player.IsFocused)
                return EspWidget.PaintESPWidgetFocused;
            if (player is LocalPlayer)
                return EspWidget.PaintESPWidgetLocalPlayer;
            switch (player.Type)
            {
                case Player.PlayerType.Teammate:
                    return EspWidget.PaintESPWidgetTeammate;
                case Player.PlayerType.PMC:
                    return EspWidget.PaintESPWidgetPMC;
                case Player.PlayerType.AIScav:
                    return EspWidget.PaintESPWidgetScav;
                case Player.PlayerType.AIRaider:
                    return EspWidget.PaintESPWidgetRaider;
                case Player.PlayerType.AIBoss:
                    return EspWidget.PaintESPWidgetBoss;
                case Player.PlayerType.PScav:
                    return EspWidget.PaintESPWidgetPScav;
                case Player.PlayerType.SpecialPlayer:
                    return EspWidget.PaintESPWidgetWatchlist;
                case Player.PlayerType.Streamer:
                    return EspWidget.PaintESPWidgetStreamer;
                default:
                    return EspWidget.PaintESPWidgetPMC;
            }
        }
    }
}