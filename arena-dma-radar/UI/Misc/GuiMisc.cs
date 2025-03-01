using arena_dma_radar.Arena.ArenaPlayer;
using arena_dma_radar.UI.ESP;
using arena_dma_radar.UI.Radar;
using eft_dma_shared.Common.Maps;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Unity;

namespace arena_dma_radar.UI.Misc
{
    public sealed class ScreenEntry
    {
        private readonly int _screenNumber;

        /// <summary>
        /// Screen Index Number.
        /// </summary>
        public int ScreenNumber => _screenNumber;

        public ScreenEntry(int screenNumber)
        {
            _screenNumber = screenNumber;
        }

        public override string ToString() => $"Screen {_screenNumber}";
    }

    public sealed class BonesListItem
    {
        public string Name { get; }
        public Bones Bone { get; }
        public BonesListItem(Bones bone)
        {
            Name = bone.GetDescription();
            Bone = bone;
        }
        public override string ToString() => Name;
    }

    /// <summary>
    /// Serializable RectF Structure.
    /// </summary>
    public struct RectFSer
    {
        [JsonPropertyName("left")] public float Left { get; set; }
        [JsonPropertyName("top")] public float Top { get; set; }
        [JsonPropertyName("right")] public float Right { get; set; }
        [JsonPropertyName("bottom")] public float Bottom { get; set; }

        public RectFSer(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }

    public static class GuiExtensions
    {
        #region GUI Extensions

        /// <summary>
        /// Draw status text on the SK Canvas.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="paint"></param>
        /// <param name="clientArea"></param>
        /// <param name="spacing"></param>
        /// <param name="lines"></param>
        public static void DrawStatusText(this SKCanvas canvas, Rectangle clientArea, SKPaint paint, float spacing, params string[] lines)
        {
            var labelWidth = lines.Max(x => SKPaints.TextStatusSmall.MeasureText(x));
            var top = clientArea.Top + spacing;
            var labelHeight = paint.FontSpacing;
            var bgRect = new SKRect(
                clientArea.Width / 2 - labelWidth / 2,
                top,
                clientArea.Width / 2 + labelWidth / 2,
                top + (labelHeight * lines.Length) + spacing);
            canvas.DrawRect(bgRect, SKPaints.PaintTransparentBacker);
            var textLoc = new SKPoint(clientArea.Width / 2, top + labelHeight);
            foreach (var line in lines)
            {
                canvas.DrawText(line, textLoc, paint);
                textLoc.Y += labelHeight;
            }
        }

        /// <summary>
        /// Convert Unity Position (X,Y,Z) to an unzoomed Map Position.
        /// </summary>
        /// <param name="vector">Unity Vector3</param>
        /// <param name="map">Current Map</param>
        /// <returns>Unzoomed 2D Map Position.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ToMapPos(this Vector3 vector, LoneMapConfig map) =>
            new()
            {
                X = (map.X * map.SvgScale) + vector.X * (map.Scale * map.SvgScale),
                Y = (map.Y * map.SvgScale) - vector.Z * (map.Scale * map.SvgScale)
            };

        /// <summary>
        /// Convert an Unzoomed Map Position to a Zoomed Map Position ready for 2D Drawing.
        /// </summary>
        /// <param name="mapPos">Unzoomed Map Position.</param>
        /// <param name="mapParams">Current Map Parameters.</param>
        /// <returns>Zoomed 2D Map Position.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SKPoint ToZoomedPos(this Vector2 mapPos, LoneMapParams mapParams) =>
            new()
            {
                X = (mapPos.X - mapParams.Bounds.Left) * mapParams.XScale,
                Y = (mapPos.Y - mapParams.Bounds.Top) * mapParams.YScale
            };

        /// <summary>
        /// Draws Mouseover Text (with backer) on this zoomed location.
        /// </summary>
        public static void DrawMouseoverText(this SKPoint zoomedMapPos, SKCanvas canvas, IEnumerable<string> lines)
        {
            float maxLength = 0;
            foreach (var line in lines)
            {
                var length = SKPaints.TextMouseover.MeasureText(line);
                if (length > maxLength)
                    maxLength = length;
            }

            var backer = new SKRect
            {
                Bottom = zoomedMapPos.Y + (lines.Count() * 12f - 2) * MainForm.UIScale,
                Left = zoomedMapPos.X + 9 * MainForm.UIScale,
                Top = zoomedMapPos.Y - 9 * MainForm.UIScale,
                Right = zoomedMapPos.X + 9 * MainForm.UIScale + maxLength + 6 * MainForm.UIScale
            };
            canvas.DrawRect(backer, SKPaints.PaintTransparentBacker); // Draw tooltip backer
            zoomedMapPos.Offset(11 * MainForm.UIScale, 3 * MainForm.UIScale);
            foreach (var line in lines) // Draw tooltip text
            {
                if (string.IsNullOrEmpty(line?.Trim()))
                    continue;
                canvas.DrawText(line, zoomedMapPos, SKPaints.TextMouseover); // draw line text
                zoomedMapPos.Offset(0, 12f * MainForm.UIScale);
            }
        }

        /// <summary>
        /// Draw an ESP Text Label on an Entity.
        /// </summary>
        public static void DrawESPText(this SKPoint screenPos, SKCanvas canvas, IESPEntity entity, LocalPlayer localPlayer,
            bool printDist, SKPaint paint, params string[] lines)
        {
            if (printDist && lines.Length > 0)
            {
                var dist = Vector3.Distance(entity.Position, localPlayer.Position);
                string distStr;
                distStr = $" {(int)dist}m";
                lines[0] += distStr;
            }

            foreach (var x in lines)
            {
                if (string.IsNullOrEmpty(x?.Trim()))
                    continue;
                canvas.DrawText(x, screenPos, paint);
                screenPos.Y += paint.TextSize;
            }
        }

        #endregion
    }
}
