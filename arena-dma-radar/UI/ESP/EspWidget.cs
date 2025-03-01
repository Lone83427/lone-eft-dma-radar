using arena_dma_radar.Arena.ArenaPlayer;
using arena_dma_radar.UI.SKWidgetControl;
using eft_dma_shared.Common.ESP;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Players;
using eft_dma_shared.Common.Unity;

namespace arena_dma_radar.UI.ESP
{
    public sealed class EspWidget : SKWidget
    {
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

        private SKBitmap _espBitmap;
        private SKCanvas _espCanvas;

        public EspWidget(SKGLControl parent, SKRect location, bool minimized, float scale)
            : base(parent, "ESP", new(location.Left, location.Top), new(location.Width, location.Height), scale)
        {
            _espBitmap = new SKBitmap((int)location.Width, (int)location.Height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);
            _espCanvas = new SKCanvas(_espBitmap);
            Minimized = minimized;
        }

        public override void Draw(SKCanvas canvas)
        {
            base.Draw(canvas);
            if (!Minimized)
                RenderAimview(canvas, ClientRectangle);
        }

        /// <summary>
        /// Perform Aimview (Mini-ESP) Rendering.
        /// </summary>
        private void RenderAimview(SKCanvas parent, SKRect dest)
        {
            var size = Size;
            if (_espBitmap is null || _espCanvas is null ||
                _espBitmap.Width != size.Width || _espBitmap.Height != size.Height)
            {
                _espCanvas?.Dispose();
                _espCanvas = null;
                _espBitmap?.Dispose();
                _espBitmap = null;
                _espBitmap = new SKBitmap((int)size.Width, (int)size.Height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);
                _espCanvas = new SKCanvas(_espBitmap);
            }
            _espCanvas.Clear(SKColors.Transparent);
            try
            {
                bool inRaid = InRaid; // cache bool
                var localPlayer = LocalPlayer; // cache ref to current player
                if (inRaid && localPlayer is not null)
                {
                    var allPlayers = AllPlayers?
                        .Where(x => x.IsActive && x.IsAlive &&
                        x is not Arena.ArenaPlayer.LocalPlayer);
                    if (allPlayers is not null)
                    {
                        float scaleX = _espBitmap.Width / (float)CameraManagerBase.Viewport.Width;
                        float scaleY = _espBitmap.Height / (float)CameraManagerBase.Viewport.Height;
                        foreach (var player in allPlayers)
                        {
                            if (player.Skeleton.UpdateESPWidgetBuffer(scaleX, scaleY))
                                _espCanvas.DrawPoints(SKPointMode.Lines, Skeleton.ESPWidgetBuffer, player.GetAimviewPaint());
                        }
                    }
                    var bounds = _espBitmap.Info.Rect;
                    /// Draw Crosshair
                    float centerX = bounds.Left + bounds.Width / 2;
                    float centerY = bounds.Top + bounds.Height / 2;

                    _espCanvas.DrawLine(bounds.Left, centerY, bounds.Right, centerY, PaintAimviewCrosshair);
                    _espCanvas.DrawLine(centerX, bounds.Top, centerX, bounds.Bottom, PaintAimviewCrosshair);
                }
            }
            catch (Exception ex) // Log rendering errors
            {
                string error = $"CRITICAL ESP WIDGET RENDER ERROR: {ex}";
                LoneLogging.WriteLine(error);
            }
            _espCanvas.Flush();
            parent.DrawBitmap(_espBitmap, dest, SharedPaints.PaintBitmap);
        }

        public override void SetScaleFactor(float newScale)
        {
            base.SetScaleFactor(newScale);
            PaintAimviewCrosshair.StrokeWidth = 1 * newScale;
            PaintAimviewLocalPlayer.StrokeWidth = 1 * newScale;
            PaintAimviewPlayer.StrokeWidth = 1 * newScale;
            PaintAimviewAI.StrokeWidth = 1 * newScale;
            PaintAimviewStreamer.StrokeWidth = 1 * newScale;
            PaintAimviewTeammate.StrokeWidth = 1 * newScale;
            PaintAimviewAimbotLocked.StrokeWidth = 1 * newScale;
            PaintAimviewFocused.StrokeWidth = 1 * newScale;
        }

        public override void Dispose()
        {
            _espBitmap?.Dispose();
            _espCanvas?.Dispose();
            base.Dispose();
        }

        #region ESP Widget Paints
        private static readonly SKPaint PaintAimviewCrosshair = new SKPaint()
        {
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High,
            Color = SKColors.White,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke,
        };
        internal static readonly SKPaint PaintAimviewLocalPlayer = new SKPaint()
        {
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High,
            Color = SKColors.Green,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke,
        };
        internal static readonly SKPaint PaintAimviewAI = new SKPaint()
        {
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High,
            Color = SKColors.Yellow,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke,
        };
        internal static readonly SKPaint PaintAimviewPlayer = new SKPaint()
        {
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High,
            Color = SKColors.Red,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke,
        };
        internal static readonly SKPaint PaintAimviewStreamer = new SKPaint()
        {
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High,
            Color = SKColors.MediumPurple,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke,
        };
        internal static readonly SKPaint PaintAimviewTeammate = new SKPaint()
        {
            IsAntialias = true,
            FilterQuality = SKFilterQuality.High,
            Color = SKColors.LimeGreen,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke,
        };
        internal static readonly SKPaint PaintAimviewAimbotLocked = new SKPaint()
        {
            Color = SKColors.Blue,
            StrokeWidth = 1,
            Style = SKPaintStyle.Fill,
        };
        internal static readonly SKPaint PaintAimviewFocused = new SKPaint()
        {
            Color = SKColors.Coral,
            StrokeWidth = 1,
            Style = SKPaintStyle.Fill,
        };
        #endregion
    }

    public static class EspWidgetExtensions
    {
        /// <summary>
        /// Gets Aimview drawing paintbrush based on Player Type.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SKPaint GetAimviewPaint(this Player player)
        {
            if (player.IsAimbotLocked)
                return EspWidget.PaintAimviewAimbotLocked;
            if (player is LocalPlayer)
                return EspWidget.PaintAimviewLocalPlayer;
            if (player.IsFocused)
                return EspWidget.PaintAimviewFocused;
            switch (player.Type)
            {
                case Player.PlayerType.Teammate:
                    return EspWidget.PaintAimviewTeammate;
                case Player.PlayerType.Player:
                    return EspWidget.PaintAimviewPlayer;
                case Player.PlayerType.AI:
                    return EspWidget.PaintAimviewAI;
                case Player.PlayerType.Streamer:
                    return EspWidget.PaintAimviewStreamer;
                default:
                    return EspWidget.PaintAimviewPlayer;
            }
        }
    }
}
